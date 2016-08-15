using System;
using System.ServiceModel;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    /// <summary>
    /// Represents class that contains logic of request distribution between master and slaves. The class  also implements contract for Wcf service.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class UserManagementSystem : IUserService
    {
        private UserService master;
        private UserService[] slaves;
        private int serviceCount;
        private int currentService = 0;

        public UserManagementSystem(UserService master, UserService[] slaves)
        {
            this.master = master;
            this.slaves = slaves;
            this.serviceCount = slaves.Length + 1;
        }

        public string AddUser(User user)
        {
            return this.master.AddUser(user);
        }

        public void RemoveUser(string id)
        {
            this.master.RemoveUser(id);
        }

        public string[] FindUser(Func<User, bool> predicate)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUser(predicate);
        }

        public User FindUser(string id)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUser(id);
        }

        public void CommitChanges()
        {
            this.master.CommitChanges();
        }

        public string[] FindUsersByFirstName(string firstName)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUsersByFirstName(firstName);
        }

        public string[] FindUsersByLastName(string lastName)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUsersByLastName(lastName);
        }

        public string[] FindUsersByBirthDate(DateTime birthDate)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUsersByBirthDate(birthDate);
        }

        public string[] FindUsersWhoseNameContains(string word)
        {
            this.currentService = (this.currentService + 1) % this.serviceCount;
            var service = this.currentService == (this.serviceCount - 1) ? this.master : this.slaves[this.currentService];

            return service.FindUsersWhoseNameContains(word);
        }
    }
}