using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private List<User> userListFirst = new List<User>
        {
            new User
            {
                Age = 21,
                Gender = Gender.Man,
                Name = "User1",
                Salary = 21000
            },

            new User
            {
                Age = 30,
                Gender = Gender.Female,
                Name = "Liza",
                Salary = 30000
            },

            new User
            {
                Age = 18,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 19000
            },
            new User
            {
                Age = 32,
                Gender = Gender.Female,
                Name = "Ann",
                Salary = 36200
            },
            new User
            {
                Age = 45,
                Gender = Gender.Man,
                Name = "Alex",
                Salary = 54000
            }
        };

        private List<User> userListSecond = new List<User>
        {
            new User
            {
                Age = 23,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 24000
            },

            new User
            {
                Age = 30,
                Gender = Gender.Female,
                Name = "Liza",
                Salary = 30000
            },

            new User
            {
                Age = 23,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 24000
            },
            new User
            {
                Age = 32,
                Gender = Gender.Female,
                Name = "Kate",
                Salary = 36200
            },
            new User
            {
                Age = 45,
                Gender = Gender.Man,
                Name = "Alex",
                Salary = 54000
            },
            new User
            {
                Age = 28,
                Gender = Gender.Female,
                Name = "Kate",
                Salary = 21000
            }
        };

        [TestMethod]
        public void SortByName()
        {
            var actualDataFirstList = new List<User>();
            var expectedData = userListFirst[4];

            Comparison<User> comparison = (x, y) => String.Compare(x.Name, y.Name);
            actualDataFirstList.AddRange(userListFirst);
            actualDataFirstList.Sort(comparison);
            //ToDo Add code first list

            Assert.IsTrue(actualDataFirstList[0].Equals(expectedData));
        }

        [TestMethod]
        public void SortByNameDescending()
        {
            var actualDataSecondList = new List<User>();
            var expectedData = userListFirst[4];

            //ToDo Add code first list
            Comparison<User> comparison = (x, y) => String.Compare(x.Name, y.Name);
            actualDataSecondList.AddRange(userListSecond);
            actualDataSecondList.Sort(comparison);

            Assert.IsTrue(actualDataSecondList[0].Equals(expectedData));
            
        }

        [TestMethod]
        public void SortByNameAndAge()
        {
            var actualDataSecondList = new List<User>();
            var expectedData = userListSecond[5];

            //ToDo Add code second list
            Comparison<User> comparison = (x, y) =>
            {
                if (String.Compare(x.Name, y.Name) == 1 && x.Age > y.Age)
                    return 1;
                else
                    return -1;
            };
            actualDataSecondList.AddRange(userListSecond);
            actualDataSecondList.Sort(comparison);

            Assert.IsTrue(actualDataSecondList[0].Equals(expectedData));
        }

        [TestMethod]
        public void RemovesDuplicate()
        {
            var actualDataSecondList = new List<User>();
            var expectedData = new List<User> {userListSecond[0], userListSecond[1], userListSecond[3], userListSecond[4],userListSecond[5]};

            //ToDo Add code second list

            actualDataSecondList = userListSecond.Distinct().ToList();

            CollectionAssert.AreEqual(expectedData, actualDataSecondList);
        }

        [TestMethod]
        public void ReturnsDifferenceFromFirstList()
        {
            var actualData = new List<User>();
            var expectedData = new List<User> { userListFirst[0], userListFirst[2], userListFirst[3] };

            //ToDo Add code first list
            actualData = userListFirst.Except(userListSecond).ToList();

            CollectionAssert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void SelectsValuesByNameMax()
        {
            var actualData = new List<User>();
            var expectedData = new List<User> { userListSecond[0], userListSecond[2] };

            //ToDo Add code for second list
            actualData = userListSecond.Where(x => x.Name == "Max").ToList();
            
            CollectionAssert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void ContainOrNotContainName()
        {
            var isContain = false;

            //name max 
            isContain = userListSecond.Where(x => x.Name == "Max").Count() != 0;
            //ToDo Add code for second list

            Assert.IsTrue(isContain);

            // name obama
            //ToDo add code for second list
            isContain = userListSecond.Where(x => x.Name == "Obama").Count() != 0;
            Assert.IsFalse(isContain);
        }

        [TestMethod]
        public void AllListWithName()
        {
            var isAll = true;

            //name max 
            //ToDo Add code for second list
            isAll = userListSecond.All(x => x.Name == "Max");

            Assert.IsFalse(isAll);
        }

        [TestMethod]
        public void ReturnsOnlyElementByNameMax()
        {
            var actualData = new User();
            
            try
            {
                //ToDo Add code for second list
                //name Max
                actualData = userListSecond.Single(x => x.Name == "Max");
                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Sequence contains more than one matching element", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void ReturnsOnlyElementByNameNotOnList()
        {
            var actualData = new User();

            try
            {
                //ToDo Add code for second list
                //name Ldfsdfsfd
                actualData = userListSecond.Single(x => x.Name == "Lsdfsdf");

                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Sequence contains no matching element", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void ReturnsOnlyElementOrDefaultByNameNotOnList()
        {
            var actualData = new User();

            //ToDo Add code for second list

            //name Ldfsdfsfd
            actualData = userListSecond.SingleOrDefault(x => x.Name == "Lsdfsdf");


            Assert.IsTrue(actualData == null);
        }


        [TestMethod]
        public void ReturnsTheFirstElementByNameNotOnList()
        {
            var actualData = new User();

            try
            {
                //ToDo Add code for second list
                //name Ldfsdfsfd
                actualData = userListSecond.Single(x => x.Name == "Ldfsdfsfd");
                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Sequence contains no matching element", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void ReturnsTheFirstElementOrDefaultByNameNotOnList()
        {
            var actualData = new User();

            //ToDo Add code for second list
            //name Ldfsdfsfd
            actualData = userListSecond.SingleOrDefault(x => x.Name == "Lsdfdsfd");

            Assert.IsTrue(actualData == null);
        }

        [TestMethod]
        public void GetMaxSalaryFromFirst()
        {
            var expectedData = 54000;
            var actualData = new User();

            Comparison<User> comparison = (x, y) =>
            {
                if ( x.Salary > y.Salary)
                    return 1;
                else
                    return -1;
            };
            //ToDo Add code for first list
            userListFirst.Sort(comparison);
            actualData = userListFirst.OrderBy(x => x.Salary).Last();

            Assert.IsTrue(expectedData == actualData.Salary);
        }

        [TestMethod]
        public void GetCountUserWithNameMaxFromSecond()
        {
            var expectedData = 2;
            var actualData = 0;

            //ToDo Add code for second list
            actualData = userListSecond.Count(x => x.Name == "Max");

            Assert.IsTrue(expectedData == actualData);
        }

        [TestMethod]
        public void Join()
        {
            var NameInfo = new[]
            {
                new {name = "Max", Info = "info about Max"},
                new {name = "Alan", Info = "About Alan"},
                new {name = "Alex", Info = "About Alex"}
            }.ToList();

            var expectedData = 3;
            var actualData = -1;

            //ToDo Add code for second list
            
            
            Assert.IsTrue(expectedData == actualData);
        }
    }
}
