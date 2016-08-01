using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Configuration
{
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
