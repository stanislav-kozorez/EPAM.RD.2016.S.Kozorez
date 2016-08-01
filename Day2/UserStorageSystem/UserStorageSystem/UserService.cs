using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class UserService : MarshalByRefObject, IUserService
    {
        private IUserStorage userStorage;
        private ILogger logger;
        private IUserValidator userValidator;
        private IIdGenerator idGenerator;
        private bool isMaster;
        private List<User> users;
        private ReaderWriterLockSlim locker;
        private bool usesTcp;
        private TcpInfo[] tcpInfos;

        public UserService(IUserStorage userStorage, ILogger logger, IUserValidator validator, IIdGenerator generator, bool isMaster = false, bool usesTcp = false, TcpInfo slaveInfo = null)
        {
            var storageInfo = userStorage.LoadUsers();
            if (storageInfo == null)
            {
                storageInfo = new StorageInfo() { Users = new List<User>() };
            }
            else
            {
                if (storageInfo.GeneratorTypeName != generator.GetType().FullName)
                {
                    throw new ArgumentException("Generator type differs from stored one");
                }

                generator.RestoreGeneratorState(storageInfo.LastId, storageInfo.GeneratorCallCount);
            }

            this.userStorage = userStorage;
            this.userValidator = validator;
            this.idGenerator = generator;
            this.isMaster = isMaster;
            this.users = storageInfo.Users;
            this.logger = logger;
            this.locker = new ReaderWriterLockSlim();
            this.usesTcp = usesTcp;
            if (usesTcp && !this.isMaster)
            {
                this.StartListener(slaveInfo.IpAddress, slaveInfo.Port);
            }
        }

        public event EventHandler<UserAddEventArgs> OnUserAdd = delegate { };

        public event EventHandler<UserRemoveEventArgs> OnUserRemove = delegate { };

        public string Name { get; internal set; }

        internal TcpInfo[] TcpInfos { get { return this.tcpInfos; } set { this.tcpInfos = value; } }

        public string AddUser(User user)
        {
            this.logger.Log(TraceEventType.Information, string.Format("Try to add new user to {0}, device: {1}", this.isMaster ? "Master" : "Slave", this.Name));
            if (user == null)
            {
                throw new ArgumentNullException("User is Null");
            }

            if (!this.isMaster)
            {
                throw new InvalidOperationException("It is forbidden to write to Slave");
            }

            if (!this.userValidator.UserIsValid(user))
            {
                throw new ArgumentException("User validation failed");
            }

            this.locker.EnterWriteLock();
            this.idGenerator.MoveNext();
            var id = this.idGenerator.Current.ToString();
            user.Id = id;
            this.users.Add(user);
            this.locker.ExitWriteLock();

            if (this.usesTcp)
            {
                this.NotifySlaves(new TcpBundle() { Command = TcpCommand.Add, User = user });
            }

            var eventArgs = new UserAddEventArgs(user);
            this.OnUserAdd(this, eventArgs);

            return id;
        }

        public void RemoveUser(string id)
        {
            this.logger.Log(TraceEventType.Information, string.Format("Try to remove user from {0}, device: {1}", this.isMaster ? "Master" : "Slave", this.Name));
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("Wrong id parameter");
            }

            if (!this.isMaster)
            {
                throw new InvalidOperationException("It is forbidden to remove users from Slave");
            }

            this.locker.EnterWriteLock();
            var index = this.GetUserIndex(id);
            if (index == -1)
            {
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            }

            this.users.RemoveAt(index);
            this.locker.ExitWriteLock();

            if (this.usesTcp)
            {
                this.NotifySlaves(new TcpBundle() { Command = TcpCommand.Delete, User = new User() { Id = id } });
            }

            var eventArgs = new UserRemoveEventArgs(index);
            this.OnUserRemove(this, eventArgs);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            this.logger.Log(TraceEventType.Information, string.Format("Try to find users by predicate from {0}, device: {1}", this.isMaster ? "Master" : "Slave", this.Name));
            var result = new ConcurrentBag<string>();

            this.locker.EnterReadLock();
            Parallel.ForEach<User>(this.users, user => 
            {
                if (predicate(user))
                {
                    result.Add(user.Id);
                }
            });
            this.locker.ExitReadLock();

            return result.ToArray();             
        }

        public User FindUser(string id)
        {
            this.logger.Log(TraceEventType.Information, string.Format("Try to find user by id from {0}, device: {1}", this.isMaster ? "Master" : "Slave", this.Name));

            this.locker.EnterReadLock();
            var index = this.GetUserIndex(id);
            if (index == -1)
            {
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            }

            var result = this.users[index];
            this.locker.ExitReadLock();

            return result;         
        }

        public void CommitChanges()
        {
            this.logger.Log(TraceEventType.Information, string.Format("Try to commit changes on {0}, device: {1}", this.isMaster ? "Master" : "Slave", this.Name));
            if (!this.isMaster)
            {
                throw new InvalidOperationException("Commit is forbidden for Slave");
            }

            this.locker.EnterReadLock();
            var storageInfo = new StorageInfo()
            {
                Users = this.users,
                GeneratorCallCount = this.idGenerator.CallNumber,
                GeneratorTypeName = this.idGenerator.GetType().FullName,
                LastId = this.idGenerator.Current.ToString()
            };
            this.userStorage.SaveUsers(storageInfo);
            this.locker.ExitReadLock();
        }

        public string[] FindUsersByFirstName(string firstName)
        {
            return this.FindUser(x => x.FirstName == firstName);
        }

        public string[] FindUsersByLastName(string lastName)
        {
            return this.FindUser(x => x.LastName == lastName);
        }

        public string[] FindUsersByBirthDate(DateTime birthDate)
        {
            return this.FindUser(x => x.BirthDate == birthDate);
        }

        public string[] FindUsersWhoseNameContains(string word)
        {
            return this.FindUser(x => x.FirstName.Contains(word));
        }

        internal void OnUserAddHandler(object sender, UserAddEventArgs e)
        {
            this.logger.Log(TraceEventType.Information, $"UserAddHandler on Slave {Name}");

            this.locker.EnterWriteLock();
            this.users.Add(e.User);
            this.locker.ExitWriteLock();
        }

        internal void OnUserRemoveHandler(object sender, UserRemoveEventArgs e)
        {
            this.logger.Log(TraceEventType.Information, $"UserRemoveHandler on Slave {Name}");

            this.locker.EnterWriteLock();
            this.users.RemoveAt(e.Index);
            this.locker.ExitWriteLock();
        }

        internal void NotifySlaves(TcpBundle bundle)
        {
            var formatter = new BinaryFormatter();
            foreach (var tcpInfo in this.tcpInfos)
            {
                this.logger.Log(TraceEventType.Information, $"Notify slave at {tcpInfo.IpAddress}:{tcpInfo.Port}");
                var client = new TcpClient(tcpInfo.IpAddress, tcpInfo.Port);
                var strm = client.GetStream();
                formatter.Serialize(strm, bundle);
                strm.Close();
                client.Close();
            }
        }

        internal void StartListener(string ipAddr, int port)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                TcpListener server = null;
                try
                {
                    IPAddress addr = IPAddress.Parse(ipAddr);
                    server = new TcpListener(addr, port);

                    server.Start();
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();

                        logger.Log(TraceEventType.Information, $"device {Name} got new update request");
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
                    logger.Log(TraceEventType.Error, $"SocketException at device {Name} : {e.Message}");
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
            for (int i = 0; i < this.users.Count; i++)
            {
                if (this.users[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
