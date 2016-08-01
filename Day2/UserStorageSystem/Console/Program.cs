using System;
using System.Threading;
using System.Threading.Tasks;
using UserStorageSystem;
using UserStorageSystem.Entities;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var userService = ConfigurationService.ConfigureUserServiceInSingleDomain();
            var userService = ConfigurationService.ConfigureUserServiceInMultiplyDomains();
            
            var writeTask = new Task(() => 
            {
                for (int i = 0; i < 100; i++)
                {
                    userService.AddUser(new User()
                    {
                        BirthDate = new DateTime(2000, 6, 20),
                        FirstName = "John" + i,
                        LastName = "Smith" + i,
                        Gender = Gender.Male,
                        Passport = "sf2342323"
                    });
                    Thread.Sleep(1000);
                }
            });

            var findTask = new Task(() =>
            {
                for (int i = 0; i < 200; i++)
                {
                    Console.WriteLine("Found {0} users", userService.FindUser(x => x.FirstName.Contains("2")).Length);
                    Thread.Sleep(500);
                }
            });

            writeTask.Start();
            findTask.Start();
            Task.WaitAll(writeTask, findTask);

            Console.ReadKey();
        }
    }
}
