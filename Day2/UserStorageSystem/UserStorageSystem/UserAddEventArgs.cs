using System;
using UserStorageSystem.Entities;

namespace UserStorageSystem
{
    /// <summary>
    ///     Used by master for notification of slaves when new user is added.
    /// </summary>
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