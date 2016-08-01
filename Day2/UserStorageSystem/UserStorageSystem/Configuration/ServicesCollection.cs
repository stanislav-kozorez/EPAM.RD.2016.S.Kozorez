﻿using System;
using System.Configuration;

namespace UserStorageSystem.Configuration
{
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