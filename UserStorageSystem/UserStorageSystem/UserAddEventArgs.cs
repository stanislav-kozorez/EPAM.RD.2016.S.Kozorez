using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem
{
    public class UserAddEventArgs: EventArgs
    {
        private User user;
        private string id;

        public User User { get { return user; } }
        public string Id { get { return id; } }

        public UserAddEventArgs(User user, string id)
        {
            this.user = user;
            this.id = id;
        }
    }
}