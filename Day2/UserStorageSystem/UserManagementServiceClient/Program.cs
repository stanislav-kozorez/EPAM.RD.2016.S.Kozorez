using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserManagementServiceClient.UserManagementService;

namespace UserManagementServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserServiceClient("BasicHttpBinding_IUserService");

            var writeTask = new Task(() => {
                for (int i = 0; i < 100; i++)
                {
                    userService.AddUser(new User()
                    {
                        BirthDate = new DateTime(2000, 6, 20),
                        FirstName = "John" + i,
                        LastName = "Smith" + i,
                        Gender = Gender.Male,
                        Passport = "sf2342323",
                        VisaRecords = new VisaRecord[0]
                    });
                    Thread.Sleep(1000);
                }
            });

            var findTask = new Task(() =>
            {
                for (int i = 0; i < 200; i++)
                {
                    Console.WriteLine("Found {0} users", userService.FindUsersWhoseNameContains("2").Length);
                    Thread.Sleep(500);
                }
            });

            writeTask.Start();
            findTask.Start();
            Task.WaitAll(writeTask, findTask);

            Console.ReadKey();
            userService.Close();
        }
    }
}
