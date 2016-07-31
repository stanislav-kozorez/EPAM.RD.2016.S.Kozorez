using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;

namespace UserStorageSystemTests
{
    [TestClass]
    public class ConfigurationServiceTest
    {
        [TestMethod]
        public void ConfigureUserServiceInSingleDomain_ReturnsNotNullUserManagementSystemInstance()
        {
            var userService = ConfigurationService.ConfigureUserServiceInSingleDomain();

            Assert.IsNotNull(userService);
        }
    }
}
