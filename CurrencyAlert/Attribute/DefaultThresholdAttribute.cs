using System;

namespace CurrencyAlert
{
    internal class DefaultThresholdAttribute : Attribute
    {
        public DefaultThresholdAttribute(int v)
        {
            Value = v;
        }

        public int Value { get; }
    }
}