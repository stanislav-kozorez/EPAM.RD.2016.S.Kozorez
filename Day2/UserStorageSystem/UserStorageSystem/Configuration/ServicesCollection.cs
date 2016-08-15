using System.Configuration;

namespace UserStorageSystem.Configuration
{
    /// <summary>
    ///     Describes xml representation of collection of services
    /// </summary>
    /// 
    /// <example>
    ///     <services>
    ///         <service type = "master" name="master_0"></service>
    ///         <service type = "slave" name="slave_3" ip="127.0.0.1" port ="13003"></service>
    ///     </services>
    /// </example>
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "service")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public ServiceElement this[int index]
        {
            get
            {
                return (ServiceElement)BaseGet(index);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Name;
        }
    }
}