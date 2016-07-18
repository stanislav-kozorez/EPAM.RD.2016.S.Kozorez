namespace UserStorageSystem
{
    public class UserManagementSystem
    {
        public UserService Master { get; }
        public UserService[] Slaves { get; }

        public UserManagementSystem(UserService master, UserService[] slaves)
        {
            Master = master;
            Slaves = slaves;
        }
    }
}