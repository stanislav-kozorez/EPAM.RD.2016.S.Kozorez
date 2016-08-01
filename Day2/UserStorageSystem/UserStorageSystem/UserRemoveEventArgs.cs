using System;

namespace UserStorageSystem
{
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