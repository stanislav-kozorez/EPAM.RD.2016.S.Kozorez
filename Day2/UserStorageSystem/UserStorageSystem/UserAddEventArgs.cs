using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem
{
    public class UserAddEventArgs: EventArgs
    {
        private User user;

        public User User { get { return user; } }

        public UserAddEventArgs(User user)
        {
            this.user = user;
        }
    }
}