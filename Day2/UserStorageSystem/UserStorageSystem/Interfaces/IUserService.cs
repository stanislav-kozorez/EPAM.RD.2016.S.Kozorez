using System;
using System.Linq.Expressions;
using System.ServiceModel;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string AddUser(User user);
        [OperationContract]
        void RemoveUser(string id);

        string[] FindUser(Func<User, bool> predicate);
        [OperationContract]
        User FindUser(string id);
        [OperationContract]
        void CommitChanges();

        [OperationContract]
        string[] FindUsersByFirstName(string firstName);
        [OperationContract]
        string[] FindUsersByLastName(string lastName);
        [OperationContract]
        string[] FindUsersByBirthDate(DateTime birthDate);
        [OperationContract]
        string[] FindUsersWhoseNameContains(string word);
    }
}
