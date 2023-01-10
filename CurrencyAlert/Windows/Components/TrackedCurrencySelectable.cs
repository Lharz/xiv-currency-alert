using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using KamiLib.Drawing;
using KamiLib.Interfaces;

namespace CurrencyAlert.Windows.Components;

public class TrackedCurrencySelectable : ISelectable, IDrawable
{
    public IDrawable Contents => this;
    public string ID { get; }

    private readonly TrackedCurrency currency;

    public TrackedCurrencySelectable(TrackedCurrency trackedCurrency)
    {
        currency = trackedCurrency;
        ID = currency.CurrencyInfo.ItemName;
    }
    
    public void DrawLabel()
    {
        currency.Draw(true);
    }
    
    public void Draw()
    {
        InfoBox.Instance
            .AddTitle(Strings.CurrencyConfiguration, out var innerWidth)
            .AddConfigCheckbox(Strings.Enabled, currency.Enabled)
            .AddInputInt(Strings.Threshold, currency.Threshold, 0, 100000, 0, 0, innerWidth / 2.0f)
            .Draw();
    }
}