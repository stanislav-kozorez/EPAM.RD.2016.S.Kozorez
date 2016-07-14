using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Attributes
{
    public static class InstanceValidator
    {
        public static bool UserIsValid(User user)
        {
            bool result = false;

            var type = typeof(User);

            var pinfo = type.GetProperties();

            

            return result;
        }

        public static bool AdvancedUserIsValid(AdvancedUser user)
        {
            return true;
        }
    }
}
