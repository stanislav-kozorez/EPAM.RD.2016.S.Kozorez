namespace UserStorageSystem
{
    public class UserRemoveEventArgs
    {
        private int index;

        public int Index { get { return index; } }

        public UserRemoveEventArgs(int index)
        {
            this.index = index;
        }
    }
}