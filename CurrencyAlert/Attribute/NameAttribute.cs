using System;

namespace CurrencyAlert.Enum
{
    internal class NameAttribute : Attribute
    {
        public NameAttribute(string v)
        {
            Value = v;
        }

        public string Value { get; }
    }
}
