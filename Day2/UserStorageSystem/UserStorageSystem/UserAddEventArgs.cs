using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem
{
    [Serializable]
    public class UserAddEventArgs : EventArgs
    {
        private User user;

        public UserAddEventArgs(User user)
        {
            this.user = user;
        }

        public User User { get { return this.user; } set { this.user = value; } }
    }
}