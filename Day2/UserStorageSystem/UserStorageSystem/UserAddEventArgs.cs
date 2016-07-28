using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem
{
    [Serializable]
    public class UserAddEventArgs: EventArgs
    {
        private User user;

        public User User { get { return user; } set { user = value; } }

        public UserAddEventArgs(User user)
        {
            this.user = user;
        }
    }
}