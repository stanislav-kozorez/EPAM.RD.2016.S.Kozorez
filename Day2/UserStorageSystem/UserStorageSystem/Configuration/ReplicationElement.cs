using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class ReplicationElement: ConfigurationElement
    {
        [ConfigurationProperty("slaves", DefaultValue = "3", IsRequired = true)]
        public int SlaveCount
        {
            get
            {
                return (int)this["slaves"];
            }
            set
            {
                this["slaves"] = value;
            }
        }
    }
}