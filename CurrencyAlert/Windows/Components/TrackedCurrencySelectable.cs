using System.Numerics;
using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using Dalamud.Interface;
using ImGuiNET;
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
        ID = currency.CurrencyInfo().ItemName;
    }
    
    public void DrawLabel()
    {
        currency.DrawIcon();
        ImGui.SameLine();
        currency.DrawName(Colors.White);
    }
    
    public void Draw()
    {
        InfoBox.Instance
            .AddTitle(Strings.CurrentlySelected)
            .AddIcon(currency.CurrencyInfo().IconID, ImGuiHelpers.ScaledVector2(40.0f), 1.0f)
            .SameLine()
            .AddString(currency.CurrencyInfo().ItemName)
            .Draw();
        
        InfoBox.Instance
            .AddTitle(Strings.CurrencyConfiguration, out var innerWidth)
            .AddConfigCheckbox(Strings.Enabled, currency.Enabled)
            .AddInputInt(Strings.Threshold, currency.Threshold, 0, 100000, 0, 0, innerWidth / 2.0f)
            .Draw();
    }
}