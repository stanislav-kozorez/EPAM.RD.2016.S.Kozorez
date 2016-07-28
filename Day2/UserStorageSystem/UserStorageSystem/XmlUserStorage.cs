using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class XmlUserStorage : MarshalByRefObject, IUserStorage
    {
        public string Name { get; set; }

        public XmlUserStorage(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));
            this.Name = name;
        }

        public StorageInfo LoadUsers()
        {
            StorageInfo result = null;
            if (File.Exists(Name))
            {
                if (new FileInfo(Name).Extension != ".xml")
                    throw new ArgumentException($"wrong type of file {Name}");

                using (var stream = File.Open(Name, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(StorageInfo));
                    result = (StorageInfo)serializer.Deserialize(stream);
                }
            }
            
            return result;
        }

        public void SaveUsers(StorageInfo storageInfo)
        {
            if (storageInfo == null)
                throw new ArgumentNullException(nameof(storageInfo));

            using (var stream = File.Open(Name, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StorageInfo));
                serializer.Serialize(stream, storageInfo);
            }
        }
    }
}