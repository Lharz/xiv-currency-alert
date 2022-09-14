using CurrencyAlert.Entity;
using System.Collections.Generic;

namespace CurrencyAlert.Provider
{
    internal sealed class CurrencyProvider
    {
        private static CurrencyProvider? instance = null;
        public static CurrencyProvider Instance => instance ??= new CurrencyProvider();

        private readonly List<Currency> currencies;

        public CurrencyProvider()
        {
            currencies = new List<Currency>
            {
                new Currency(28, "Tomestones of Poetics", Enum.CurrencyType.TomestoneOfPoetics, Enum.Category.Battle, 1400),
                new Currency(43, "Tomestones of Astronomy", Enum.CurrencyType.TomestoneOfAstronomy, Enum.Category.Battle, 1700),
                new Currency(44, "Tomestones of Causality", Enum.CurrencyType.TomestoneOfCausality, Enum.Category.Battle, 1700),

                new Currency(20, "Storm Seals", Enum.CurrencyType.StormSeal, Enum.Category.Common, 75000),
                new Currency(21, "Serpent Seals", Enum.CurrencyType.SerpentSeal, Enum.Category.Common, 75000),
                new Currency(22, "Flame Seals", Enum.CurrencyType.FlameSeal, Enum.Category.Common, 75000),

                new Currency(25, "Wolf Marks", Enum.CurrencyType.WolfMark, Enum.Category.Battle, 18000),
                new Currency(36656, "Trophy Crystals", Enum.CurrencyType.TrophyCrystal, Enum.Category.Battle, 18000),

                new Currency(27, "Allied Seals", Enum.CurrencyType.AlliedSeal, Enum.Category.Battle, 3500),
                new Currency(10307, "Centurio Seals", Enum.CurrencyType.CenturioSeal, Enum.Category.Battle, 3500),
                new Currency(26533, "Sack of Nuts", Enum.CurrencyType.SackOfNut, Enum.Category.Battle, 3500),
                new Currency(26807, "Bicolor Gemstone", Enum.CurrencyType.BicolorGemstone, Enum.Category.Battle, 800),

                new Currency(25199, "White Crafters' Scrip", Enum.CurrencyType.WhiteCraftersScrip, Enum.Category.Other, 1500),
                new Currency(33913, "Purple Crafters' Scrip", Enum.CurrencyType.PurpleCraftersScrip, Enum.Category.Other, 1500),
                new Currency(25200, "White Gatherers' Scrip", Enum.CurrencyType.WhiteGatherersScrip, Enum.Category.Other, 1500),
                new Currency(33914, "Purple Gatherers' Scrip", Enum.CurrencyType.PurpleGatherersScrip, Enum.Category.Other, 1500),
                new Currency(28063, "Skybuilders' Scrip", Enum.CurrencyType.SkybuildersScrip, Enum.Category.Other, 7500),
            };
        }

        public IEnumerable<Currency> GetAll()
        {
            return currencies;
        }
    }
}
