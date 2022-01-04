using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert.Enum
{
    public enum Currency
    {
        [Slot(1), DefaultThreshold(70000)]
        StormSeals,

        [Slot(4), DefaultThreshold(15000)]
        WolfMarks,

        [Slot(6), DefaultThreshold(1500)]
        Poetics,

        [Slot(8), DefaultThreshold(1000)]
        AlliedSeals
    }
}
