using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IntValidatorAttribute : Attribute
    {
        private int from;
        private int to;

        public int From { get { return from; } }
        public int To { get { return to; } }

        public IntValidatorAttribute(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
