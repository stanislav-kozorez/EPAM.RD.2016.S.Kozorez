using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem.Configuration;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    public static class ConfigurationService
    {
        public static IUserService ConfigureUserService()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("serviceConfig");
            var storagesSection = (StorageConfigSection)ConfigurationManager.GetSection("storage");


            bool usesTcp = servicesSection.UsesTcp;
            int slaveCount = servicesSection.Services.Count - 1;
            if (slaveCount <= 0 || slaveCount > 10)
                throw new ArgumentException("Invalid Slave count");
            int masterCount = servicesSection.Services.Cast<ServiceElement>().Where(x => x.Type == "master").Count();
            if (masterCount != 1)
                throw new ArgumentException("Invalid Master count");
            var storageName = storagesSection.Name;
            if (String.IsNullOrWhiteSpace(storageName))
                throw new ArgumentException("Invalid storage name");
            var userStorage = new XmlUserStorage(storageName);
            var logger = new DefaultLogger();
            var validator = new DefaultValidator();
            var generator = new DefaultGenerator();
            var masterInfo = servicesSection.Services.Cast<ServiceElement>().Single(x => x.Type == "master");
            var master = new UserService(userStorage, logger, validator, generator, true, usesTcp);
            master.Name = masterInfo.Name;
            var slavesInfo = servicesSection.Services.Cast<ServiceElement>().Where(x => x.Type != "master").ToArray();
            var tcpInfos = slavesInfo.Select(x => new TcpInfo() { IpAddress = x.Ip, Port = x.Port }).ToArray();
            master.TcpInfos = tcpInfos;
            var slaves = new UserService[slaveCount];
            for(int i = 0; i < tcpInfos.Length; i++)
            {
                slaves[i] = new UserService(userStorage, logger, validator, generator, usesTcp: usesTcp, slaveInfo: tcpInfos[i]) { Name = slavesInfo[i].Name};
                if (!usesTcp)
                {
                    master.OnUserAdd += slaves[i].OnUserAddHandler;
                    master.OnUserRemove += slaves[i].OnUserRemoveHandler;
                }
            }

            return new UserManagementSystem(master, slaves);
        }
    }
}
