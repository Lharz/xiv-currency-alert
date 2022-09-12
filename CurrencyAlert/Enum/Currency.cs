using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert.Enum
{
    public enum Currency
    {
        [Name("Tomestones of Poetics"), ItemID(28), DefaultThreshold(1400), Category("Battle")]
        TomestoneOfPoetics,
        [Name("Tomestones of Aphorism"), ItemID(42), DefaultThreshold(1700), Category("Battle")]
        TomestoneOfAphorism,
        [Name("Tomestones of Astronomy"), ItemID(43), DefaultThreshold(1700), Category("Battle")]
        TomestoneOfAstronomy,

        [Name("Storm Seals"),  ItemID(20), DefaultThreshold(75000), Category("Common")]
        StormSeal,
        [Name("Serpent Seals"), ItemID(21), DefaultThreshold(75000), Category("Common")]
        SerpentSeal,
        [Name("Flame Seals"), ItemID(22), DefaultThreshold(75000), Category("Common")]
        FlameSeal,

        [Name("Wolf Marks"), ItemID(25), DefaultThreshold(18000), Category("Battle")]
        WolfMark,
        [Name("Trophy Crystals"), ItemID(36656), DefaultThreshold(18000), Category("Battle")]
        TrophyCrystal,

        [Name("Allied Seals"), ItemID(27), DefaultThreshold(3500), Category("Battle")]
        AlliedSeal,
        [Name("Centurio Seals"), ItemID(10307), DefaultThreshold(3500), Category("Battle")]
        CenturioSeal,
        [Name("Sack of Nuts"), ItemID(26533), DefaultThreshold(3500), Category("Battle")]
        SackOfNut,
        [Name("Bicolor Gemstone"), ItemID(26807), DefaultThreshold(800), Category("Battle")]
        BicolorGemstone,

        [Name("White Crafters' Scrip"), ItemID(25199), DefaultThreshold(1500), Category("Other")]
        WhiteCraftersScrip,
        [Name("Purple Crafters' Scrip"), ItemID(33913), DefaultThreshold(1500), Category("Other")]
        PurpleCraftersScrip,
        [Name("White Gatherers' Scrip"), ItemID(25200), DefaultThreshold(1500), Category("Other")]
        WhiteGatherersScrip,
        [Name("Purple Gatherers' Scrip"), ItemID(33914), DefaultThreshold(1500), Category("Other")]
        PurpleGatherersScrip,
        [Name("Skybuilders' Scrip"), ItemID(28063), DefaultThreshold(7500), Category("Other")]
        SkybuildersScrip
    }
}
