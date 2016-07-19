namespace Monitor
{
    // TODO: Use Monitor (not lock) to protect this structure.
    public class MyClass
    {
        private int _value;
        private object syncObj = new object();

        public int Counter
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public void Increase()
        {
            System.Threading.Monitor.Enter(syncObj);
            try
            {
                _value++;
            }
            finally
            {
                System.Threading.Monitor.Exit(syncObj);
            }
        }

        public void Decrease()
        {
            System.Threading.Monitor.Enter(syncObj);
            try
            {
                _value--;
            }
            finally
            {
                System.Threading.Monitor.Exit(syncObj);
            }
        }
    }
}
