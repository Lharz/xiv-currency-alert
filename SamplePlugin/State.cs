using CurrencyAlert.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert
{
    internal class State
    {
        public bool SettingsVisible { get; set; }
        public Dictionary<Currency, bool> AlertVisible { get; set; } = new Dictionary<Currency, bool>();

        public State()
        {
            foreach (Currency currency in System.Enum.GetValues(typeof(Currency)))
            {
                this.AlertVisible[currency] = false;
            }
        }
    }
}
