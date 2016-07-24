using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Configuration;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public static class ConfigurationService
    {
        public static IUserService ConfigureUserService()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("services");
            var storagesSection = (StoragesConfigSection)ConfigurationManager.GetSection("storages");

            int slaveCount = servicesSection.Replication.SlaveCount;
            if (slaveCount <= 0 || slaveCount > 10)
                throw new ArgumentException("Invalid Slave count");
            var storageName = storagesSection.Storage.Name;
            if (String.IsNullOrWhiteSpace(storageName))
                throw new ArgumentException("Invalid storage name");
            var userStorage = new XmlUserStorage(storageName);
            var logger = new DefaultLogger();
            var validator = new DefaultValidator();
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
