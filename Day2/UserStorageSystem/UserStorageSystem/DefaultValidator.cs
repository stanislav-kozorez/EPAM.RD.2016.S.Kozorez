using System;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    [Serializable]
    public class DefaultValidator : MarshalByRefObject, IUserValidator
    {
        public bool UserIsValid(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.BirthDate > DateTime.Now)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Passport))
            {
                return false;
            }

            if (user.Passport.Length != 9)
            {
                return false;
            }

            if (user.VisaRecords == null)
            {
                return false;
            }

            return true;
        }
    }
}