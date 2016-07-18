using System;

namespace UserStorageSystem.Entities
{
    [Serializable()]
    public class User: IEquatable<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Passport { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }
        public User() { }

        public bool Equals(User other)
        {
            if (other == null)
                return false;
            if (String.Compare(this.FirstName, other.FirstName) == 0 &&
                String.Compare(this.LastName, other.LastName) == 0)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (obj.GetType() != typeof(User))
                return false;
            User user = (User)obj;
            return Equals(user);
        }

        public override int GetHashCode()
        {
            int result = 23;
            result ^= String.IsNullOrEmpty(FirstName) ? 0 : FirstName.GetHashCode();
            result ^= String.IsNullOrEmpty(LastName) ? 0 : LastName.GetHashCode();
            return result;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}