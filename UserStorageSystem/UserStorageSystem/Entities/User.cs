using System;

namespace UserStorageSystem.Entities
{
    public class User: IEquatable<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Passport { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }

        public bool Equals(User other)
        {
            //compare by first and last name
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}