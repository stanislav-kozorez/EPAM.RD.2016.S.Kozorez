using System;
using System.Collections.Generic;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public class XmlUserStorage : IUserStorage
    {
        public Dictionary<string, User> LoadUsers()
        {
            throw new NotImplementedException();
        }

        public void SaveUsers(Dictionary<string, User> users)
        {
            throw new NotImplementedException();
        }
    }
}