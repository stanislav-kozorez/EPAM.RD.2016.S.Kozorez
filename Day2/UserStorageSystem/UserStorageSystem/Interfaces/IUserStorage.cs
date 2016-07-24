using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    public interface IUserStorage
    {
        void SaveUsers(List<User> users);
        List<User> LoadUsers();     
    }
}
