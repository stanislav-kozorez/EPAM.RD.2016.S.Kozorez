using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;
using UserStorageSystem.Entities;
using System.Collections.Generic;
using System.IO;

namespace UserStorageSystemTests
{
    [TestClass]
    public class XmlUserStorageTest
    {
        List<User> users;
        
        [TestInitialize]
        public void Init()
        {
            users = new List<User>()
            {
                new User()
                {
                    Id = "2",
                    BirthDate = DateTime.Now,
                    FirstName = "John",
                    LastName = "Smith",
                    Gender = Gender.Male,
                    Passport = "sf2342323"
                },
                new User()
                {
                    Id = "3",
                    BirthDate = DateTime.Now,
                    FirstName = "Max",
                    LastName = "Smith",
                    Gender = Gender.Male,
                    Passport = "sf2332323"
                },
                new User()
                {
                    Id = "5",
                    BirthDate = DateTime.Now,
                    FirstName = "Ben",
                    LastName = "Smith",
                    Gender = Gender.Male,
                    Passport = "sf2342323"
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WithEmptyStorageName_ThrowsArgumentException()
        {
            var storage = new XmlUserStorage("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WithStorageNameThatConsistsOfWhiteSpaces_ThrowsArgumentException()
        {
            var storage = new XmlUserStorage("      ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullStorageName_ThrowsArgumentException()
        {
            var storage = new XmlUserStorage(null);
        }

        [TestMethod]
        public void SaveUsers_CreatesNewFileOnDisk()
        {
            var storage = new XmlUserStorage("storage.xml");
            var storageInfo = new StorageInfo()
            {
                Users = users,
                GeneratorCallCount = 3,
                GeneratorTypeName = typeof(DefaultGenerator).FullName,
                LastId = "5"
            };

            storage.SaveUsers(storageInfo);
            Assert.IsTrue(File.Exists("storage.xml"));
        }

        [TestMethod]
        public void LoadUsers_LoadsThreeUsersFromFileStorage()
        {
            var storage = new XmlUserStorage("storage.xml");

            var result = storage.LoadUsers();
            CollectionAssert.AreEqual(users, result.Users);
        }
    }
}
