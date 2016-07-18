using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;

namespace UserStorageSystemTests
{
    [TestClass]
    public class ConfigurationServiceTest
    {
        [TestMethod]
        public void ConfigureUserService_ReturnsNotNullUserManagementSystemInstance()
        {
            var userService = ConfigurationService.ConfigureUserService();

            Assert.IsNotNull(userService);
        }
    }
}
