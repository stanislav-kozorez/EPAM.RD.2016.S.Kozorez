using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class UserService: IUserService
    {
        private readonly IUserStorage _userStorage;
        private IUserValidator _userValidator;
        private IIdGenerator _idGenerator;
        private readonly ILogger _logger;
        private bool _isMaster;
        private List<User> _users;
        private ReaderWriterLockSlim _locker;
        private bool _usesTcp;
        private TcpInfo[] tcpInfos;

        internal TcpInfo[] TcpInfos { get { return tcpInfos; } set { tcpInfos = value; } }
        public string Name { get; internal set; }

        public event EventHandler<UserAddEventArgs> OnUserAdd = delegate {};
        public event EventHandler<UserRemoveEventArgs> OnUserRemove = delegate { };

        public UserService(IUserStorage userStorage, ILogger logger, IUserValidator validator, IIdGenerator generator, bool isMaster = false, bool usesTcp = false, TcpInfo slaveInfo = null)
        {
            this._userStorage = userStorage;
            this._userValidator = validator;
            this._idGenerator = generator;
            this._isMaster = isMaster;
            this._users = userStorage.LoadUsers();
            this._logger = logger;
            this._locker = new ReaderWriterLockSlim();
            this._usesTcp = usesTcp;
            if (usesTcp && !_isMaster)
                StartListener(slaveInfo.IpAddress, slaveInfo.Port);
        }

        public string AddUser(User user)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to add new user to {0}, device: {1}", _isMaster ? "Master" : "Slave", Name));
            if (user == null)
                throw new ArgumentNullException("User is Null");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to write to Slave");
            if (_userValidator.UserIsValid(user))
                throw new ArgumentException("User validation failed");

            _locker.EnterWriteLock();
            _idGenerator.MoveNext();
            var id = _idGenerator.Current.ToString();
            user.Id = id;
            _users.Add(user);
            _locker.ExitWriteLock();

            if(_usesTcp)
            {
                NotifySlaves(new TcpBundle() { Command = TcpCommand.Add, User = user });
            }
            var eventArgs = new UserAddEventArgs(user);
            OnUserAdd(this, eventArgs);
            return id;
        }

        public void RemoveUser(string id)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to remove user from {0}, device: {1}", _isMaster ? "Master" : "Slave", Name));
            if (String.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("Wrong id parameter");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to remove users from Slave");

            _locker.EnterWriteLock();
            var index = GetUserIndex(id);            
            if (index == -1)
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            _users.RemoveAt(index);
            _locker.ExitWriteLock();

            var eventArgs = new UserRemoveEventArgs(index);
            OnUserRemove(this, eventArgs);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find users by predicate from {0}, device: {1}", _isMaster ? "Master" : "Slave", Name));
            List<string> result = new List<string>();

            _locker.EnterReadLock();
            foreach (var user in _users)
                if (predicate(user))
                    result.Add(user.Id);
            _locker.ExitReadLock();

            return result.ToArray();
                
        }

        public User FindUser(string id)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find user by id from {0}, device: {1}", _isMaster ? "Master" : "Slave", Name));

            _locker.EnterReadLock();
            var index = GetUserIndex(id);
            if (index == -1)
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            var result = _users[index];
            _locker.ExitReadLock();

            return result;
            
        }

        public void CommitChanges()
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to commit changes on {0}, device: {1}", _isMaster ? "Master" : "Slave", Name));
            if (!_isMaster)
                throw new InvalidOperationException("Commit is forbidden for Slave");

            _locker.EnterReadLock();
            _userStorage.SaveUsers(_users);
            _locker.ExitReadLock();
        }

        internal void OnUserAddHandler(object sender, UserAddEventArgs e)
        {
            _logger.Log(TraceEventType.Information, $"UserAddHandler on Slave {Name}");

            _locker.EnterWriteLock();
            _users.Add(e.User);
            _locker.ExitWriteLock();
        }

        internal void OnUserRemoveHandler(object sender, UserRemoveEventArgs e)
        {
            _logger.Log(TraceEventType.Information, $"UserRemoveHandler on Slave {Name}");

            _locker.EnterWriteLock();
            _users.RemoveAt(e.Index);
            _locker.ExitWriteLock();
        }

        internal void NotifySlaves(TcpBundle bundle)
        {
            var formatter = new BinaryFormatter();
            foreach (var tcpInfo in tcpInfos)
            {
                _logger.Log(TraceEventType.Information, $"Notify slave at {tcpInfo.IpAddress}:{tcpInfo.Port}");
                var client = new TcpClient(tcpInfo.IpAddress, tcpInfo.Port);
                var strm = client.GetStream();
                formatter.Serialize(strm, bundle);
                strm.Close();
                client.Close();
            }
        } 

        internal void StartListener(string ipAddr, int port)
        {
            Thread thread = new Thread(new ThreadStart(
                () => {
                    TcpListener server = null;
                    try
                    {
                        IPAddress addr = IPAddress.Parse(ipAddr);
                        server = new TcpListener(addr, port);

                        server.Start();
                        while (true)
                        {
                            TcpClient client = server.AcceptTcpClient();

                            _logger.Log(TraceEventType.Information, $"device {Name} got new update request");
                            var stream = client.GetStream();
                            var formatter = new BinaryFormatter();

                            TcpBundle bundle = (TcpBundle)formatter.Deserialize(stream);

                            if (bundle.Command == TcpCommand.Add)
                            {
                                OnUserAddHandler(this, new UserAddEventArgs(bundle.User));
                            }
                            else
                            {
                                var index = GetUserIndex(bundle.User.Id);
                                OnUserRemoveHandler(this, new UserRemoveEventArgs(index));
                            }
                            
                            stream.Close();
                            client.Close();
                        }
                    }
                    catch (SocketException e)
                    {
                        _logger.Log(TraceEventType.Error, $"SocketException at device {Name} : {e.Message}");
                    }
                    finally
                    {
                        server.Stop();
                    }
                }));
            thread.IsBackground = true;
            thread.Start();
        }

        private int GetUserIndex(string id)
        {
            for (int i = 0; i < _users.Count; i++)
                if (_users[i].Id == id)
                    return i;
            return -1;
        }
    }
}
