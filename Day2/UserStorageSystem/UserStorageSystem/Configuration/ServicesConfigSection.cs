using System.Configuration;

namespace UserStorageSystem.Configuration
{
    /// <summary>
    ///     Describes custom service configuration section
    /// </summary>
    public class ServicesConfigSection : ConfigurationSection
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
