using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attributes;

namespace Attributes.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CreateUsers_ReturnsThreeUsers()
        {
            var users = InstanceCreator.CreateUsers();

            Assert.IsNotNull(users);
            Assert.AreEqual(3, users.Length);
            foreach (var user in users)
                Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CreateAdvancedUser_ReturnsNotNullUser()
        {
            var user = InstanceCreator.CreateAdvancedUser();

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void UserIsValid_ReturnsTrue()
        {
            var users = InstanceCreator.CreateUsers();

            foreach (var user in users)
            {
                bool actual = InstanceValidator.UserIsValid(user);
                Assert.AreEqual(true, actual);
            }
        }
    }

}
