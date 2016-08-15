using System.Configuration;

namespace UserStorageSystem.Configuration
{
    /// <summary>
    ///     Describes xml representation of service configuration
    /// </summary>
    /// 
    /// <example>
    ///     <service type="master" name="master_0"></service>
    ///     <service type = "slave" name="slave_1" ip="127.0.0.1" port ="13001"></service>
    /// </example>
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
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

        [ConfigurationProperty("ip", IsRequired = false, DefaultValue = "localhost")]        
        public string Ip
        {
            get
            {
                return (string)this["ip"];
            }

            set
            {
                this["ip"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 13000)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }

            set
            {
                this["port"] = value;
            }
        }
    }
}