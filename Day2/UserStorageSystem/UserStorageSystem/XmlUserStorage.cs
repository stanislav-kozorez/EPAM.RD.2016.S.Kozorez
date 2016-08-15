using System;
using System.IO;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;
using UserStorageSystem.Serialization.Xml;

namespace UserStorageSystem
{
    /// <summary>
    ///     Enables to store User Service state in xml file.
    /// </summary>
    public class XmlUserStorage : MarshalByRefObject, IUserStorage
    {
        /// <summary>
        ///     Initializes object with file name;
        /// </summary>
        /// <param name="name">
        ///     Path to xml file.
        /// </param>
        public XmlUserStorage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            this.Name = name;
        }

        public string Name { get; set; }

        /// <summary>
        ///     Loads User Service state from xml file.
        /// </summary>
        /// <returns>
        ///     service state.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     thrown if file has wrong extention
        /// </exception>
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

        /// <summary>
        ///     Saves User System state to xml file.
        /// </summary>
        /// <param name="storageInfo">
        ///     Contains User Service state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown when storageInfo param is null
        /// </exception>
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