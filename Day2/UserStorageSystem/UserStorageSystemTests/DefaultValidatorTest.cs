using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem.Entities;
using UserStorageSystem;

namespace UserStorageSystemTests
{
    /// <summary>
    /// Summary description for DefaultValidatorTest
    /// </summary>
    [TestClass]
    public class DefaultValidatorTest
    {
        [TestMethod]
        public void UserIsValid_UserWithIncorrectBirthDate_ReturnsFalse()
        {
            var user = new User()
            {
                BirthDate = DateTime.Now.AddMonths(3),
                FirstName = "John",
                LastName = "Smith",
                Gender = Gender.Male,
                Passport = "AB3452343"
            };
            var validator = new DefaultValidator();
            Assert.AreEqual(false, validator.UserIsValid(user));
        }

        [TestMethod]
        public void UserIsValid_UserWithEmptyFirstName_ReturnsFalse()
        {
            var user = new User()
            {
                BirthDate = new DateTime(2005, 3, 20),
                FirstName = string.Empty,
                LastName = "Smith",
                Gender = Gender.Male,
                Passport = "AB3452343"
            };
            var validator = new DefaultValidator();
            Assert.AreEqual(false, validator.UserIsValid(user));
        }

        [TestMethod]
        public void UserIsValid_UserWithEmptyLastName_ReturnsFalse()
        {
            var user = new User()
            {
                BirthDate = new DateTime(2005, 3, 20),
                FirstName = "John",
                LastName = string.Empty,
                Gender = Gender.Male,
                Passport = "AB3452343"
            };
            var validator = new DefaultValidator();
            Assert.AreEqual(false, validator.UserIsValid(user));
        }

        [TestMethod]
        public void UserIsValid_UserWithPassportNumberLengthNotEqualToNine_ReturnsFalse()
        {
            var user = new User()
            {
                BirthDate = new DateTime(2005, 3, 20),
                FirstName = string.Empty,
                LastName = "Smith",
                Gender = Gender.Male,
                Passport = "AB34523453"
            };
            var validator = new DefaultValidator();
            Assert.AreEqual(false, validator.UserIsValid(user));
        }

        [TestMethod]
        public void UserIsValid_NullUser_ReturnsFalse()
        {
            User user = null;
            var validator = new DefaultValidator();
            Assert.AreEqual(false, validator.UserIsValid(user));
        }
    }
}
