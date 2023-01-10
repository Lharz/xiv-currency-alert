using KamiLib.Configuration;

namespace CurrencyAlert.DataModels;

public class DisplaySettings
{
    public Setting<bool> ShowIcon = new(true);
    public Setting<bool> ShowName = new(true);
    public Setting<bool> ShowWarningText = new(true);
}