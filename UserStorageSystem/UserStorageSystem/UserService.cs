using System;
using System.Collections.Generic;
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
        private bool _isMaster;
        private Dictionary<string, User> _users;
        public event EventHandler<UserAddEventArgs> OnUserAdd = delegate {};
        public event EventHandler<UserRemoveEventArgs> OnUserRemove = delegate { };

        public UserService(IUserStorage userStorage, bool isMaster)
        {
            this._userStorage = userStorage;
            this._isMaster = isMaster;
            this._users = userStorage.LoadUsers();
        }

        public string AddUser(User user, IIdGenerator generator = null, IUserValidator validator = null)
        {
            if (user == null)
                throw new ArgumentNullException("User is Null");
            if (!_isMaster)
                throw new InvalidOperationException("It is forbidden to write to Slave");
            var val = validator ?? _userValidator;
            if (!val.UserIsValid(user))
                throw new ArgumentException("User validation failed");
            var gen = generator ?? _idGenerator;
            var id = gen.GenerateUserId(user);
            _users[id] = user;
            var eventArgs = new UserAddEventArgs(user);
            OnUserAdd(this, eventArgs);
            return id;
        }

        public void RemoveUser(string id)
        {
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
            List<string> result = new List<string>();
            foreach (var kvp in _users)
                if (predicate(kvp.Value))
                    result.Add(kvp.Key);
            return result.ToArray();
                
        }

        public User FindUser(string id)
        {
            if (!_users.ContainsKey(id))
                throw new ArgumentException($"User with id \"{id}\" doesn't exist");
            return _users[id];
            
        }

        public void CommitChanges()
        {
            if (!_isMaster)
                throw new InvalidOperationException("Commit is forbidden for Slave");
            _userStorage.SaveUsers(_users);
        }
    }
}
