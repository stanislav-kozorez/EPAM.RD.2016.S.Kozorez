using System.Configuration;

namespace UserStorageSystem.Configuration
{
    /// <summary>
    ///     Describes custom storage configuration section
    /// </summary>
    public class StorageConfigSection : ConfigurationSection
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
