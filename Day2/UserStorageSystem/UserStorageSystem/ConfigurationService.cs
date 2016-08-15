using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UserStorageSystem.Configuration;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    /// <summary>
    ///     Used for configuration of User Management System
    /// </summary>
    public static class ConfigurationService
    {
        /// <summary>
        ///     Configures User Management System in single domain.
        /// </summary>
        /// <returns>
        ///     UserManagementSystem instance, configured in single domain 
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     thrown when App.config file contains wrong settings.
        /// </exception>
        public static UserManagementSystem ConfigureUserServiceInSingleDomain()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("serviceConfig");
            var storagesSection = (StorageConfigSection)ConfigurationManager.GetSection("storage");

            bool usesTcp = servicesSection.UsesTcp;
            int slaveCount = servicesSection.Services.Count - 1;
            if (slaveCount <= 0 || slaveCount > 10)
            {
                throw new ArgumentException("Invalid Slave count");
            }

            int masterCount = servicesSection.Services.Cast<ServiceElement>().Where(x => x.Type == "master").Count();
            if (masterCount != 1)
            {
                throw new ArgumentException("Invalid Master count");
            }

            var storageName = storagesSection.Name;
            if (string.IsNullOrWhiteSpace(storageName))
            {
                throw new ArgumentException("Invalid storage name");
            }

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
            for (int i = 0; i < tcpInfos.Length; i++)
            {
                slaves[i] = new UserService(userStorage, logger, validator, generator, usesTcp: usesTcp, slaveInfo: tcpInfos[i]) { Name = slavesInfo[i].Name };
                if (!usesTcp)
                {
                    master.OnUserAdd += slaves[i].OnUserAddHandler;
                    master.OnUserRemove += slaves[i].OnUserRemoveHandler;
                }
            }

            return new UserManagementSystem(master, slaves);
        }

        /// <summary>
        ///     Configures User Management System in multiply domains.
        /// </summary>
        /// <returns>
        ///     UserManagementSystem instance, configured in multiply domains 
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     thrown when App.config file contains wrong settings.
        /// </exception>
        public static UserManagementSystem ConfigureUserServiceInMultiplyDomains()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("serviceConfig");
            var storagesSection = (StorageConfigSection)ConfigurationManager.GetSection("storage");

            bool usesTcp = servicesSection.UsesTcp;
            int slaveCount = servicesSection.Services.Count - 1;
            if (slaveCount <= 0 || slaveCount > 10)
            {
                throw new ArgumentException("Invalid Slave count");
            }

            int masterCount = servicesSection.Services.Cast<ServiceElement>().Where(x => x.Type == "master").Count();
            if (masterCount != 1)
            {
                throw new ArgumentException("Invalid Master count");
            }

            var storageName = storagesSection.Name;
            if (string.IsNullOrWhiteSpace(storageName))
            {
                throw new ArgumentException("Invalid storage name");
            }

            UserService master = null;
            var servicesInfo = servicesSection.Services.Cast<ServiceElement>().ToArray();
            var tcpInfos = servicesInfo.Where(x => x.Type == "slave").Select(x => new TcpInfo() { IpAddress = x.Ip, Port = x.Port }).ToArray();
            var slaves = new UserService[slaveCount];
            var currentSlaveIndex = 0;
            for (int i = 0; i < servicesInfo.Length; i++)
            {
                var domain = AppDomain.CreateDomain("Domain_" + servicesInfo[i].Name);
                var userStorage = (IUserStorage)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(XmlUserStorage).FullName,
                    true, BindingFlags.CreateInstance, null, new object[] { storageName }, CultureInfo.CurrentCulture, null);
                var logger = (ILogger)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(DefaultLogger).FullName);
                var validator = (IUserValidator)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(DefaultValidator).FullName);
                var generator = (IIdGenerator)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(DefaultGenerator).FullName);
                
                if (servicesInfo[i].Type == "master")
                {
                    master = (UserService)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(UserService).FullName, true, BindingFlags.CreateInstance, null,
                                  new object[] { userStorage, logger, validator, generator, true, usesTcp, null }, CultureInfo.CurrentCulture, null);
                    master.Name = servicesInfo[i].Name;
                    master.TcpInfos = tcpInfos;
                }
                else
                {
                    slaves[currentSlaveIndex] = (UserService)domain.CreateInstanceAndUnwrap("UserStorageSystem", typeof(UserService).FullName,
                    true, BindingFlags.CreateInstance, null,
                    new object[] { userStorage, logger, validator, generator, false, usesTcp, tcpInfos[currentSlaveIndex] }, CultureInfo.CurrentCulture, null);
                    slaves[currentSlaveIndex++].Name = servicesInfo[i].Name;
                }
            }

            if (!usesTcp)
            {
                for (int i = 0; i < slaveCount; i++)
                {
                    master.OnUserAdd += slaves[i].OnUserAddHandler;
                    master.OnUserRemove += slaves[i].OnUserRemoveHandler;
                }
            }

            return new UserManagementSystem(master, slaves);
        }
    }
}
