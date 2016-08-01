using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace UserStorageSystem.Entities
{
    public enum Gender
    {
        Male,
        Female
    }

    [DataContract]
    [Serializable]
    public class User : IEquatable<User>
    {
        public User()
        {
            this.VisaRecords = new List<VisaRecord>();
        }

        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string Passport { get; set; }
        [DataMember]
        public Gender Gender { get; set; }
        [DataMember]
        public List<VisaRecord> VisaRecords { get; set; }

        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            if (string.Compare(this.FirstName, other.FirstName) == 0 &&
                string.Compare(this.LastName, other.LastName) == 0)
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj.GetType() != typeof(User))
            {
                return false;
            }

            User user = (User)obj;

            return this.Equals(user);
        }

        public override int GetHashCode()
        {
            int result = 23;
            result ^= string.IsNullOrEmpty(this.FirstName) ? 0 : this.FirstName.GetHashCode();
            result ^= string.IsNullOrEmpty(this.LastName) ? 0 : this.LastName.GetHashCode();
            return result;
        }
    }
}