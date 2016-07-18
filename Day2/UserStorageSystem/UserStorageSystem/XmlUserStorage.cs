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
                throw new ArgumentNullException(nameof(name));
            this.filename = name;
        }

        public Dictionary<string, User> LoadUsers()
        {
            Dictionary<string, User> result;
            if (File.Exists(filename))
            {
                if (new FileInfo(filename).Extension != ".xml")
                    throw new ArgumentException($"wrong type of file {filename}");

                using (var stream = File.Open(filename, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, User>));
                    result = (Dictionary<string, User>)serializer.Deserialize(stream);
                }
            }
            else
            {
                throw new FileNotFoundException($"File {filename} doesn't exist");
            }
            return result;
        }

        public void SaveUsers(Dictionary<string, User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            using (var stream = File.Open(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, User>));
                serializer.Serialize(stream, users);
            }
        }
    }
}