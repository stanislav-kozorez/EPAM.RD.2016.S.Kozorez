using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    public static class InstanceCreator
    {
        public static User[] CreateUsers()
        {
            User[] users;

            Type t = typeof(User);
            var classAttributes = t.GetCustomAttributes(false)
                .Where(x => x is InstantiateUserAttribute)
                .Select(x => (InstantiateUserAttribute) x).ToArray();
            users = new User[classAttributes.Count()];
            var ctorAttribute = t.GetConstructor(new Type[] { typeof(int) }).GetCustomAttributes(false)
                .Where(x => x is MatchParameterWithPropertyAttribute)
                .Select(x => (MatchParameterWithPropertyAttribute) x).First();
            PropertyInfo pinfo = t.GetProperty(ctorAttribute.PropertyName);
            int defValue = (int)(pinfo.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute).Value;
            for (int i = 0; i < classAttributes.Count(); i++)
            {
                var user = (User)Activator.CreateInstance(typeof(User), defValue);
                var classAttribute = classAttributes[i];
                user.FirstName = classAttribute.FirstName;
                user.LastName = classAttribute.LastName;
                user.Id = classAttribute.Id == 0 ? user.Id : classAttribute.Id;
                users[i] = user;
            }
            return users;
            
        }

        public static AdvancedUser CreateAdvancedUser()
        {
            AdvancedUser user;

            Type t = typeof(AdvancedUser);
            var assemblyAttr = t.Assembly.GetCustomAttributes(false)
                .Where(x => x is InstantiateAdvancedUserAttribute)
                .Select(x => (InstantiateAdvancedUserAttribute)x).First();
            
            var ctorAttributes = t.GetConstructor(new Type[] { typeof(int), typeof(int) }).GetCustomAttributes(false)
                .Where(x => x is MatchParameterWithPropertyAttribute)
                .Select(x => (MatchParameterWithPropertyAttribute)x).ToArray();
            var pinfo = t.GetProperty(ctorAttributes[0].PropertyName);
            int id = (int)(pinfo.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute).Value;
            pinfo = t.GetProperty(ctorAttributes[1].PropertyName);
            int externalId = (int)(pinfo.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute).Value;
            
            user = (AdvancedUser)Activator.CreateInstance(typeof(AdvancedUser), id, externalId);
            user.FirstName = assemblyAttr.FirstName;
            user.LastName = assemblyAttr.LastName;
            user.Id = assemblyAttr.Id;
          
            return user;
        }
    }
}
