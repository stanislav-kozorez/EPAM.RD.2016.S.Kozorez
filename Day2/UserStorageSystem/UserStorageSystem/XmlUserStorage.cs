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

        public List<User> LoadUsers()
        {
            var result = new List<User>();
            if (File.Exists(filename))
            {
                if (new FileInfo(filename).Extension != ".xml")
                    throw new ArgumentException($"wrong type of file {filename}");

                using (var stream = File.Open(filename, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                    result = (List<User>)serializer.Deserialize(stream);
                }
            }
            
            return result;
        }

        public void SaveUsers(List<User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            using (var stream = File.Open(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                serializer.Serialize(stream, users);
            }
        }
    }
}