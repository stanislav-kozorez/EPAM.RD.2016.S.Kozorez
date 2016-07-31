using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem;

namespace UserManagementServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUri = new Uri("http://localhost:8080/");
            using (var host = new ServiceHost(ConfigurationService.ConfigureUserServiceInMultiplyDomains(), serviceUri))
            {
                host.Open();
                Console.WriteLine("Service started. To close service press any key.");
                Console.ReadKey();
            }
        }
    }
}
