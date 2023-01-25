using KamiLib.Configuration;

namespace CurrencyAlert.DataModels;

public class MoneyOverlaySettings
{
    public Setting<bool> Enabled = new(false);
    
    public Setting<CurrencyName>[] MoneyOverlayCurrencies = 
    {
        new(CurrencyName.Unselected),
        new(CurrencyName.Unselected),
        new(CurrencyName.Unselected),
    };
}