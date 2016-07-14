using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem
{
    public static class ConfigurationService
    {
        public static UserManagementSystem ConfigureUserService()
        {
            //Get configuration from app.config
            //create masters and slaves
            return new UserManagementSystem();
        }
    }
}
