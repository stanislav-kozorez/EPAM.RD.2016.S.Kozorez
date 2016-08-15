using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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


        /// <summary>
        ///     Compares two users by first and last name
        /// </summary>
        /// <param name="other">
        ///     User to compare to.
        /// </param>
        /// <returns>
        ///     True if first name and last name are equal, False otherwise
        /// </returns>
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

        /// <summary>
        ///     Standard Equals method, uses Equals( User other) internally
        /// </summary>
        /// <param name="obj">
        ///     User to compare to.
        /// </param>
        /// <returns>
        ///     True if first name and last name are equal, False otherwise
        /// </returns>
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
        /// <summary>
        ///     Overrided GetHashcode method
        /// </summary>
        /// <returns>
        ///     Hashcode of the User entity
        /// </returns>
        public override int GetHashCode()
        {
            int result = 23;
            result ^= string.IsNullOrEmpty(this.FirstName) ? 0 : this.FirstName.GetHashCode();
            result ^= string.IsNullOrEmpty(this.LastName) ? 0 : this.LastName.GetHashCode();
            return result;
        }
    }
}