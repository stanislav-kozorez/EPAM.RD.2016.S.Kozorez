using System;
using System.IO;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;
using UserStorageSystem.Serialization.Xml;

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
                
                var serializer = new XmlSerializer(typeof(StorageInfo));
                result = (StorageInfo)serializer.Deserialize(Name);
            }
            
            return result;
        }

        public void SaveUsers(StorageInfo storageInfo)
        {
            if (storageInfo == null)
                throw new ArgumentNullException(nameof(storageInfo));
            
            var serializer = new XmlSerializer(typeof(StorageInfo));
            serializer.Serialize(storageInfo, Name);
        }
    }
}