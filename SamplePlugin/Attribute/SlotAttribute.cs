using System;

namespace CurrencyAlert
{
    internal class SlotAttribute : Attribute
    {
        public SlotAttribute(int v)
        {
            Value = v;
        }

        public int Value { get; }
    }
}