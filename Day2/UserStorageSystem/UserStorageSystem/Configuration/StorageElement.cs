using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class StorageElement: ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "storage.xml", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}