using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Configuration
{
    public class StoragesConfigSection: ConfigurationSection
    {
        [ConfigurationProperty("storage")]
        public StorageElement Storage
        {
            get
            {
                return (StorageElement)this["storage"];
            }
            set
            {
                this["storage"] = value;
            }
        }
    }
}
