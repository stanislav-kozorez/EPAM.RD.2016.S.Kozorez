using System;

namespace Attributes
{
    // Should be applied to .ctors.
    // Matches a .ctor parameter with property. Needs to get a default value from property field, and use this value for calling .ctor.
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class MatchParameterWithPropertyAttribute : Attribute
    {
        private string paramName;
        private string propertyName;

        public string ParameterName { get { return paramName; } }
        public string PropertyName { get { return propertyName; } }

        public MatchParameterWithPropertyAttribute(string paramName, string propertyName)
        {
            this.paramName = paramName;
            this.propertyName = propertyName;
        }
    }
}
