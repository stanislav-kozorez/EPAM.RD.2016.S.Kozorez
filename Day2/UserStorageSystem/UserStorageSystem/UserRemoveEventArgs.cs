using System;

namespace UserStorageSystem
{
    [Serializable]
    public class UserRemoveEventArgs
    {
        private int index;

        public int Index { get { return index; } set { index = value; } }

        public UserRemoveEventArgs(int index)
        {
            this.index = index;
        }
    }
}