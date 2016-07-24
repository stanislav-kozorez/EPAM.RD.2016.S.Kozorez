using System;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class UserManagementSystem: IUserService
    {
        private UserService _master;
        private UserService[] _slaves;
        private int serviceCount;
        private int currentService = 0;

        public UserManagementSystem(UserService master, UserService[] slaves)
        {
            this._master = master;
            this._slaves = slaves;
            this.serviceCount = slaves.Length + 1;
        }

        public event EventHandler<UserAddEventArgs> OnUserAdd = delegate { };
        public event EventHandler<UserRemoveEventArgs> OnUserRemove = delegate { };

        public string AddUser(User user)
        {
            return _master.AddUser(user);
        }

        public void RemoveUser(string id)
        {
            _master.RemoveUser(id);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            currentService = (currentService + 1) / serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUser(predicate);
        }

        public User FindUser(string id)
        {
            currentService = (currentService + 1) / serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUser(id);
        }

        public void CommitChanges()
        {
            _master.CommitChanges();
        }
    }
}