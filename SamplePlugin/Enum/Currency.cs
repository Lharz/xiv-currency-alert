using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert.Enum
{
    public enum Currency
    {
        [Name("Tomestones of Poetics"), Slot(6), DefaultThreshold(1500)]
        TomestoneOfPoetics,
        [Name("Tomestones of Astronomy"), Slot(10), DefaultThreshold(1500)]
        TomestoneOfAstronomy,
        [Name("Storm Seals"),  Slot(1), DefaultThreshold(75000)]
        StormSeal,
        [Name("Serpent Seals"), Slot(2), DefaultThreshold(75000)]
        SerpentSeal,
        [Name("Flame Seals"), Slot(3), DefaultThreshold(75000)]
        FlameSeal,
        [Name("Wolf Marks"), Slot(4), DefaultThreshold(18000)]
        WolfMark,
        [Name("Allied Seals"), Slot(8), DefaultThreshold(3500)]
        AlliedSeal
    }
}
