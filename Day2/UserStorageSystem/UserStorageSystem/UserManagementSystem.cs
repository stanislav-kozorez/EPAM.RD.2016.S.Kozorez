using System;
using System.ServiceModel;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
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
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUser(predicate);
        }

        public User FindUser(string id)
        {
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUser(id);
        }

        public void CommitChanges()
        {
            _master.CommitChanges();
        }

        public string[] FindUsersByFirstName(string firstName)
        {
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUsersByFirstName(firstName);
        }

        public string[] FindUsersByLastName(string lastName)
        {
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUsersByLastName(lastName);
        }

        public string[] FindUsersByBirthDate(DateTime birthDate)
        {
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUsersByBirthDate(birthDate);
        }

        public string[] FindUsersWhoseNameContains(string word)
        {
            currentService = (currentService + 1) % serviceCount;
            var service = currentService == (serviceCount - 1) ? _master : _slaves[currentService];

            return service.FindUsersWhoseNameContains(word);
        }
    }
}