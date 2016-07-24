using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    public interface IUserService
    {
        event EventHandler<UserAddEventArgs> OnUserAdd;
        event EventHandler<UserRemoveEventArgs> OnUserRemove;

        string AddUser(User user);
        void RemoveUser(string id);
        string[] FindUser(Func<User, bool> predicate);
        User FindUser(string id);
        void CommitChanges();
    }
}
