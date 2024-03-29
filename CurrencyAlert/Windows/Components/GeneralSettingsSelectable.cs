﻿using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using Dalamud.Interface;
using ImGuiNET;
using KamiLib.Drawing;
using KamiLib.Interfaces;

namespace CurrencyAlert.Windows.Components;

public class GeneralSettingsSelectable : ISelectable, IDrawable
{
    private static OverlaySettings OverlaySettings => Service.Configuration.OverlaySettings;
    private static DisplaySettings DisplaySettings => Service.Configuration.DisplaySettings;
    
    public IDrawable Contents => this;
    public string ID => "GeneralSettings";
    
    public void DrawLabel()
    {
        ImGui.Text(Strings.GeneralSettings);
        ImGuiHelpers.ScaledDummy(8.0f);
    }

    public void Draw()
    {
        InfoBox.Instance
            .AddTitle(Strings.OverlaySettings, out var innerWidth)
            .AddConfigCheckbox(Strings.ShowOverlay, OverlaySettings.Show)
            .AddConfigCheckbox(Strings.LockOverlay, OverlaySettings.LockPosition)
            .AddConfigCheckbox(Strings.MinimalOverlay, OverlaySettings.MinimalDisplay)
            .AddConfigCheckbox(Strings.DisplayAscending, OverlaySettings.AscendingDescending)
            .AddDragFloat(Strings.Opacity, OverlaySettings.Opacity, 0.00f, 1.0f, innerWidth / 2.0f)
            .Draw();
        
        InfoBox.Instance
            .AddTitle(Strings.ChatNotifications)
            .AddConfigCheckbox(Strings.ChatNotifications, Service.Configuration.ChatNotification)
            .Draw();
            
        InfoBox.Instance
            .AddTitle(Strings.DisplaySettings)
            .AddConfigCheckbox(Strings.ShowCurrencyIcon, DisplaySettings.ShowIcon)
            .AddConfigCheckbox(Strings.ShowCurrencyName, DisplaySettings.ShowName)
            .AddConfigCheckbox(Strings.ShowWarningText, DisplaySettings.ShowWarningText)
            .AddConfigColor(Strings.TextColor, Strings.Default, DisplaySettings.TextColor, Colors.White)
            .Draw();
    }
}