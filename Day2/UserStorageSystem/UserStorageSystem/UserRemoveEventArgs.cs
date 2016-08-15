using System;

namespace UserStorageSystem
{
    /// <summary>
    ///     Used by master for notification of slaves when user is removed.
    /// </summary>
    [Serializable]
    public class UserRemoveEventArgs
    {
        private int index;

        public UserRemoveEventArgs(int index)
        {
            this.index = index;
        }

        public int Index { get { return this.index; } set { this.index = value; } }
    }
}