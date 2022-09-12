using System;

namespace CurrencyAlert
{
    internal class ItemIDAttribute : Attribute
    {
        public ItemIDAttribute(int v)
        {
            Value = v;
        }

        public int Value { get; }
    }
}