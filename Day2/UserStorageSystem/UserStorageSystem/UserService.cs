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
    public class UserService: IUserService
    {
        private readonly IUserStorage _userStorage;
        private IUserValidator _userValidator;
        private IIdGenerator _idGenerator;
        private readonly ILogger _logger;
        private bool _isMaster;
        private List<User> _users;
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

        public string AddUser(User user)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to add new user to {0}", _isMaster ? "Master" : "Slave"));
            if (user == null)
                throw new ArgumentNullException("User is Null");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to write to Slave");
            if (_userValidator.UserIsValid(user))
                throw new ArgumentException("User validation failed");
            _idGenerator.MoveNext();
            var id = _idGenerator.Current.ToString();
            user.Id = id;
            _users.Add(user);
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
            var index = GetUserIndex(id);            
            if (index == -1)
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            _users.RemoveAt(index);
            var eventArgs = new UserRemoveEventArgs(index);
            OnUserRemove(this, eventArgs);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find users by predicate from {0}", _isMaster ? "Master" : "Slave"));
            List<string> result = new List<string>();
            foreach (var user in _users)
                if (predicate(user))
                    result.Add(user.Id);
            return result.ToArray();
                
        }

        public User FindUser(string id)
        {
            _logger.Log(TraceEventType.Information, String.Format("Try to find user by id from {0}", _isMaster ? "Master" : "Slave"));
            var index = GetUserIndex(id);
            if (index == -1)
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            return _users[index];
            
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
            _users.Add(e.User);
        }

        internal void OnUserRemoveHandler(object sender, UserRemoveEventArgs e)
        {
            _logger.Log(TraceEventType.Information, "UserRemoveHandler on Slave");
            _users.RemoveAt(e.Index);
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
