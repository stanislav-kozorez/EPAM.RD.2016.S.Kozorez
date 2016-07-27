using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class XmlUserStorage : IUserStorage
    {
        private readonly string filename;

        public XmlUserStorage(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));
            this.filename = name;
        }

        public StorageInfo LoadUsers()
        {
            StorageInfo result = null;
            if (File.Exists(filename))
            {
                if (new FileInfo(filename).Extension != ".xml")
                    throw new ArgumentException($"wrong type of file {filename}");

                using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
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

            using (var stream = File.Open(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StorageInfo));
                serializer.Serialize(stream, storageInfo);
            }
        }
    }
}