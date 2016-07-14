using System.Linq;
using System.Reflection;

namespace Attributes
{
    public static class InstanceValidator
    {
        public static bool UserIsValid(User user)
        {
            var type = typeof(User);

            var pInfo = type.GetProperties();
            foreach (var p in pInfo)
            {
                if (p.PropertyType == typeof(string))
                {
                    StringValidatorAttribute attr = p.GetCustomAttributes(false)
                        .Where(x => x is StringValidatorAttribute).Select(x => (StringValidatorAttribute)x).FirstOrDefault();
                    if (attr != null && p.GetValue(user).ToString().Length > attr.Length)
                        return false;
                }
            }

            var fInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var f in fInfo)
            {
                if (f.FieldType == typeof(int))
                {
                    IntValidatorAttribute attr = f.GetCustomAttributes(false)
                        .Where(x => x is IntValidatorAttribute)
                        .Select(x => (IntValidatorAttribute)x).FirstOrDefault();
                    int value = (int)f.GetValue(user);
                    if ((attr != null) && (value < attr.From || value > attr.To))
                        return false;
                }
            }

            return true;
        }

        public static bool AdvancedUserIsValid(AdvancedUser user)
        {
            return UserIsValid(user);
        }
    }
}
