using System;
using UserStorageSystem.Entities;
using UserStorageSystem.Interfaces;

namespace UserStorageSystem
{
    internal class DafaultValidator: IUserValidator
    {
        public bool UserIsValid(User user)
        {
            if (user == null)
                return false;
            if (user.BirthDate > DateTime.Now)
                return false;
            if (String.IsNullOrWhiteSpace(user.FirstName) ||
                String.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Passport))
                return false;
            if (user.VisaRecords == null)
                return false;
            return true;
        }
    }
}