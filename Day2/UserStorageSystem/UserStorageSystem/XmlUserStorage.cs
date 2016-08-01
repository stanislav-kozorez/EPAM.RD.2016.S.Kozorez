using System;
using System.IO;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;
using UserStorageSystem.Serialization.Xml;

namespace UserStorageSystem
{
    public class XmlUserStorage : MarshalByRefObject, IUserStorage
    {
        public XmlUserStorage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            this.Name = name;
        }

        public string Name { get; set; }

        public StorageInfo LoadUsers()
        {
            StorageInfo result = null;
            if (File.Exists(this.Name))
            {
                if (new FileInfo(this.Name).Extension != ".xml")
                {
                    throw new ArgumentException($"wrong type of file {this.Name}");
                }
                
                var serializer = new XmlSerializer(typeof(StorageInfo));
                result = (StorageInfo)serializer.Deserialize(this.Name);
            }
            
            return result;
        }

        public void SaveUsers(StorageInfo storageInfo)
        {
            if (storageInfo == null)
            {
                throw new ArgumentNullException(nameof(storageInfo));
            }
            
            var serializer = new XmlSerializer(typeof(StorageInfo));
            serializer.Serialize(storageInfo, this.Name);
        }
    }
}