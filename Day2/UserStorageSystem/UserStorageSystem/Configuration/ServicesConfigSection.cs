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
        [ConfigurationProperty("replication")]
        public ReplicationElement Replication
        {
            get
            {
                return (ReplicationElement)this["replication"];
            }
            set
            {
                this["replication"] = value;
            }
        }
    }
}
