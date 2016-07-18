using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem
{
    public static class ConfigurationService
    {
        public static UserManagementSystem ConfigureUserService()
        {
            //Get configuration from app.config
            //create masters and slaves

            

            int slaveCount = 3; //later we will get this from config
            if (slaveCount <= 0 || slaveCount > 10)
                throw new ArgumentException("Invalid Slave count");
            var storageName = "Storage.xml"; //later we will get this from config
            if (String.IsNullOrWhiteSpace(storageName))
                throw new ArgumentException("Invalid storage name");
            var userStorage = new XmlUserStorage(storageName);
            var logger = new DefaultLogger();
            var validator = new DafaultValidator();
            var generator = new DefaultGenerator();
            var master = new UserService(userStorage, true, logger, validator, generator);
            var slaves = new UserService[slaveCount];
            for(int i = 0; i < slaveCount; i++)
            {
                slaves[i] = new UserService(userStorage, false, logger, validator, generator);
                master.OnUserAdd += slaves[i].OnUserAddHandler;
                master.OnUserRemove += slaves[i].OnUserRemoveHandler;
            }

            return new UserManagementSystem(master, slaves);
        }
    }
}
