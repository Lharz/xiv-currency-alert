using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert.Enum
{
    public enum Currency
    {
        [Slot(1), DefaultThreshold(75000)]
        StormSeal,

        [Slot(2), DefaultThreshold(75000)]
        SerpentSeal,

        [Slot(3), DefaultThreshold(75000)]
        FlameSeal,

        [Slot(4), DefaultThreshold(18000)]
        WolfMark,

        [Slot(6), DefaultThreshold(1500)]
        TomestoneOfPoetics,

        [Slot(8), DefaultThreshold(3500)]
        AlliedSeal,

        [Slot(10), DefaultThreshold(1500)]
        TomestoneOfAstronomy
    }
}
