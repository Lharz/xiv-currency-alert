using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using CurrencyAlert.DataModels;
using CurrencyAlert.Windows;
using KamiLib.Configuration;

namespace CurrencyAlert;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 6;

    public OverlaySettings OverlaySettings = new();
    public DisplaySettings DisplaySettings = new();
    public MoneyOverlaySettings MoneyOverlaySettings = new();
    
    public Setting<bool> ChatNotification = new(false);

    public TrackedCurrency[] TrackedCurrencies = {
        // Grand Company Seals
        new(CurrencyName.StormSeal, new Setting<int>(75_000), new Setting<bool>(true)),
        new(CurrencyName.SerpentSeals, new Setting<int>(75_000), new Setting<bool>(true)),
        new(CurrencyName.FlameSeals, new Setting<int>(75_000), new Setting<bool>(true)),
        
        // PvP Currencies
        new(CurrencyName.WolfMarks, new Setting<int>(18_000), new Setting<bool>(true)),
        new(CurrencyName.TrophyCrystals, new Setting<int>(18_000), new Setting<bool>(true)),
        
        // Hunts
        new(CurrencyName.AlliedSeals, new Setting<int>(3_500), new Setting<bool>(true)),
        new(CurrencyName.CenturioSeals, new Setting<int>(3_500), new Setting<bool>(true)),
        new(CurrencyName.SackOfNuts, new Setting<int>(3_500), new Setting<bool>(true)),
        
        // FATEs
        new(CurrencyName.BicolorGemstones, new Setting<int>(800), new Setting<bool>(true)),
        
        // Tomestones
        new(CurrencyName.Poetics, new Setting<int>(1_400), new Setting<bool>(true)),
        new(CurrencyName.NonLimitedTomestone, new Setting<int>(1_700), new Setting<bool>(true)),
        new(CurrencyName.LimitedTomestone, new Setting<int>(1_700), new Setting<bool>(true)),
        
        // Crafting & Gathering
        new(CurrencyName.WhiteCrafterScripts, new Setting<int>(3_500), new Setting<bool>(true)),
        new(CurrencyName.WhiteGatherersScripts, new Setting<int>(3_500), new Setting<bool>(true)),
        
        new(CurrencyName.PurpleCrafterScripts, new Setting<int>(3_500), new Setting<bool>(true)),
        new(CurrencyName.PurpleGatherersScripts, new Setting<int>(3_500), new Setting<bool>(true)),
        
        // Ishguard Restoration
        new(CurrencyName.SkybuildersScripts, new Setting<int>(7_500), new Setting<bool>(true)),
    };

    [NonSerialized]
    private DalamudPluginInterface? pluginInterface;
    public void Initialize(DalamudPluginInterface inputPluginInterface) => pluginInterface = inputPluginInterface;
    public void Save() => pluginInterface!.SavePluginConfig(this);
}