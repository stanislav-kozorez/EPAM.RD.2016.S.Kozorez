using System;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InstantiateUserAttribute : Attribute
    {
        private int id = 0;
        private string firstName;
        private string lastName;

        public int Id { get { return id; } }
        public string FirstName { get { return firstName; } }
        public string LastName { get { return lastName; } }

        public InstantiateUserAttribute(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public InstantiateUserAttribute(int id, string firstName, string lastName)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}
