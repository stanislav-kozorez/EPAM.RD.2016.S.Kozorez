using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class UserService
    {
        private readonly IUserStorage _userStorage;
        private IUserValidator _userValidator;
        private IIdGenerator _idGenerator;
        private readonly ILogger _logger;
        private bool _isMaster;
        private Dictionary<string, User> _users;
        public event EventHandler<UserAddEventArgs> OnUserAdd = delegate {};
        public event EventHandler<UserRemoveEventArgs> OnUserRemove = delegate { };

        public UserService(IUserStorage userStorage, bool isMaster, ILogger logger, IUserValidator validator, IIdGenerator generator)
        {
            this._userStorage = userStorage;
            this._userValidator = validator;
            this._idGenerator = generator;
            this._isMaster = isMaster;
            this._users = userStorage.LoadUsers();
            this._logger = logger;
        }

        public string AddUser(User user, IIdGenerator generator = null, IUserValidator validator = null)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to add new user to {0}", _isMaster ? "Master" : "Slave"));
            if (user == null)
                throw new ArgumentNullException("User is Null");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to write to Slave");
            var val = validator ?? _userValidator;
            if (!val.UserIsValid(user))
                throw new ArgumentException("User validation failed");
            var gen = generator ?? _idGenerator;
            gen.MoveNext();
            var id = gen.Current.ToString();
            _users[id] = user;
            var eventArgs = new UserAddEventArgs(user, id);
            OnUserAdd(this, eventArgs);
            return id;
        }

        public void RemoveUser(string id)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to remove user from {0}", _isMaster ? "Master" : "Slave"));
            if (String.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("Wrong id parameter");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to remove users from Slave");
            if (!_users.Remove(id))
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            var eventArgs = new UserRemoveEventArgs(id);
            OnUserRemove(this, eventArgs);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find users by predicate from {0}", _isMaster ? "Master" : "Slave"));
            List<string> result = new List<string>();
            foreach (var kvp in _users)
                if (predicate(kvp.Value))
                    result.Add(kvp.Key);
            return result.ToArray();
                
        }

        public User FindUser(string id)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find user by id from {0}", _isMaster ? "Master" : "Slave"));
            if (!_users.ContainsKey(id))
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            return _users[id];
            
        }

        public void CommitChanges()
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to commit changes on {0}", _isMaster ? "Master" : "Slave"));
            if (!_isMaster)
                throw new InvalidOperationException("Commit is forbidden for Slave");
            _userStorage.SaveUsers(_users);
        }

        internal void OnUserAddHandler(object sender, UserAddEventArgs e)
        {
            _logger.Log(TraceEventType.Information, "UserAddHandler on Slave");
            _users[e.Id] = e.User;
        }

        internal void OnUserRemoveHandler(object sender, UserRemoveEventArgs e)
        {
            _logger.Log(TraceEventType.Information, "UserRemoveHandler on Slave");
            _users.Remove(e.Id);
        }
    }
}
