using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageSystem;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = ConfigurationService.ConfigureUserService();
        }
    }
}
