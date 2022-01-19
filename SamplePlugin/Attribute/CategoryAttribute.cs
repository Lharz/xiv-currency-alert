using System;

namespace CurrencyAlert.Enum
{
    internal class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string v)
        {
            Value = v;
        }

        public string Value { get; }
    }
}
