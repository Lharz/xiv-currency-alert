using CurrencyAlert.Localization;
using CurrencyAlert.System.cs;
using CurrencyAlert.Windows;
using Dalamud.Plugin;
using KamiLib;

namespace CurrencyAlert;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "CurrencyAlert";
    
    public Plugin(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
        
        KamiCommon.Initialize(pluginInterface, Name, () => Service.Configuration.Save());
        LocalizationManager.Instance.Initialize();

        Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize(Service.PluginInterface);
        
        KamiCommon.WindowManager.AddConfigurationWindow(new ConfigurationWindow());
        KamiCommon.WindowManager.AddWindow(new OverlayWindow());
        KamiCommon.WindowManager.AddWindow(new MoneyOverlayWindow());

        Service.CurrencyTracker = new CurrencyTracker();
    }

    public void Dispose()
    {
        KamiCommon.Dispose();
        
        Service.CurrencyTracker.Dispose();
        LocalizationManager.Cleanup();
    }
}