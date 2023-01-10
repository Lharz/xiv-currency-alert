using System;
using System.Linq;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiScene;
using KamiLib.Caching;
using Lumina.Excel.GeneratedSheets;

namespace CurrencyAlert.DataModels;

public class CurrencyInfo : IDisposable
{
    public uint ItemID { get; }
    public string ItemName { get; } = string.Empty;
    public uint IconID { get; }
    public TextureWrap? IconTexture { get; }
    
    public CurrencyInfo(CurrencyName currency)
    {
        ItemID = GetItemIdForCurrency(currency);

        // Gets the item from Lumina, then checks if it is null
        if (LuminaCache<Item>.Instance.GetRow(ItemID) is { } currencyItem)
        {
            ItemName = currencyItem.Name.ToDalamudString().TextValue;
            IconID = currencyItem.Icon;

            if (IconCache.Instance.GetIcon(IconID) is { } iconTexture)
            {
                IconTexture = iconTexture;
            }
        }
    }

    public void Dispose()
    {
        IconTexture?.Dispose();
    }
    
    public unsafe int GetCurrentQuantity() => InventoryManager.Instance()->GetInventoryItemCount(ItemID);

    private static uint GetItemIdForCurrency(CurrencyName currency)
    {
        return currency switch
        {
            CurrencyName.StormSeal => 20,
            CurrencyName.SerpentSeals => 21,
            CurrencyName.FlameSeals => 22,
            CurrencyName.WolfMarks => 25,
            CurrencyName.TrophyCrystals => 36656,
            CurrencyName.AlliedSeals => 27,
            CurrencyName.CenturioSeals => 10307,
            CurrencyName.SackOfNuts => 26533,
            CurrencyName.BicolorGemstones => 26807,
            CurrencyName.Poetics => 28,
            CurrencyName.NonLimitedTomestone => GetNonLimitedTomestoneId(),
            CurrencyName.LimitedTomestone => GetLimitedTomestoneId(),
            CurrencyName.WhiteCrafterScripts => 25199,
            CurrencyName.WhiteGatherersScripts => 25200,
            CurrencyName.PurpleCrafterScripts => 33913,
            CurrencyName.PurpleGatherersScripts => 33914,
            CurrencyName.SkybuildersScripts => 28063,
            _ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null)
        };
    }

    // This will always return the ItemID of whatever tomestone is currently weekly limited
    private static uint GetLimitedTomestoneId()
    {
        return LuminaCache<TomestonesItem>.Instance
            .Where(tomestone => tomestone.Tomestones.Row is 3)
            .First()
            .Item.Row;
    }

    // This will always return the ItemID of whatever tomestone is not limited
    private static uint GetNonLimitedTomestoneId()
    {
        return LuminaCache<TomestonesItem>.Instance
            .Where(tomestone => tomestone.Tomestones.Row is 2)
            .First()
            .Item.Row;
    }
}