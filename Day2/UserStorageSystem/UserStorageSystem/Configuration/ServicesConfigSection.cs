using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class ServicesConfigSection: ConfigurationSection
    {
        [ConfigurationProperty("usesTcp", DefaultValue = true, IsRequired = true)]
        public bool UsesTcp
        {
            get { return (bool)this["usesTcp"]; }
        }

        [ConfigurationProperty("services")]
        public ServicesCollection Services
        {
            get
            {
                return (ServicesCollection)this["services"];
            }
        }
    }
}
