namespace UserStorageSystem
{
    public class UserRemoveEventArgs
    {
        private string id;

        public string Id { get { return id; } }

        public UserRemoveEventArgs(string id)
        {
            this.id = id;
        }
    }
}