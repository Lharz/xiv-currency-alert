using System.Linq;
using System.Numerics;
using CurrencyAlert.Commands;
using CurrencyAlert.DataModels;
using CurrencyAlert.Localization;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib;
using KamiLib.Configuration;
using KamiLib.GameState;
using KamiLib.Windows;

namespace CurrencyAlert.Windows;

public class OverlaySettings
{
    public Setting<bool> LockPosition = new(false);
    public Setting<float> Opacity = new(1.0f);
    public Setting<bool> MinimalDisplay = new(false);
    public Setting<bool> Show = new(true);
    public Setting<bool> AscendingDescending = new(false);
}

public class OverlayWindow : Window
{
    private static OverlaySettings OverlaySettings => Service.Configuration.OverlaySettings;
    private static DisplaySettings DisplaySettings => Service.Configuration.DisplaySettings;
    
    private Vector2 lastWindowSize = Vector2.Zero;
    
    public OverlayWindow() : base("Currency Alert Overlay")
    {
        KamiCommon.CommandManager.AddCommand(new OverlayCommands());

        Flags |= ImGuiWindowFlags.NoDecoration;
        Flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
        Flags |= ImGuiWindowFlags.NoFocusOnAppearing;
        Flags |= ImGuiWindowFlags.NoNavFocus;
        Flags |= ImGuiWindowFlags.AlwaysAutoResize;
    }

    public override void PreOpenCheck()
    {
        IsOpen = OverlaySettings.Show;
        
        if (Condition.IsBoundByDuty()) IsOpen = false;
        if (Condition.IsInCutsceneOrQuestEvent()) IsOpen = false;
        if (!Service.CurrencyTracker.ActiveWarnings.Any()) IsOpen = false;
        if (!Service.ClientState.IsLoggedIn) IsOpen = false;
    }

    public override void PreDraw()
    {
        var bgColor = ImGui.GetStyle().Colors[(int)ImGuiCol.WindowBg];
        ImGui.PushStyleColor(ImGuiCol.WindowBg, bgColor with {W = OverlaySettings.Opacity.Value});

        var borderColor = ImGui.GetStyle().Colors[(int)ImGuiCol.Border];
        ImGui.PushStyleColor(ImGuiCol.Border, borderColor with {W = OverlaySettings.Opacity.Value});

        if (OverlaySettings.LockPosition)
        {
            Flags |= DrawFlags.LockPosition;
        }
        else
        {
            Flags &= ~DrawFlags.LockPosition;
        }
    }
    
    public override void Draw()
    {
        ResizeWindow();
        
        if (OverlaySettings.MinimalDisplay)
        {
            DrawMinimalDisplay();
        }
        else
        {
            DrawNormalDisplay();
        }
    }

    public override void PostDraw()
    {
        ImGui.PopStyleColor();
        ImGui.PopStyleColor();
    }
    
    private void ResizeWindow()
    {
        if(OverlaySettings.AscendingDescending)
        {
            var size = ImGui.GetWindowSize();

            if(lastWindowSize != Vector2.Zero) 
            {
                var offset = lastWindowSize - size;
                offset.X = 0;

                if (offset != Vector2.Zero)
                {
                    ImGui.SetWindowPos(ImGui.GetWindowPos() + offset);
                }
            }

            lastWindowSize = size;
        }
    }
    
    private static void DrawMinimalDisplay()
    {
        if (DisplaySettings.ShowWarningText)
        {
            ImGui.TextColored(DisplaySettings.TextColor.Value, Strings.OverlayWarningText);
            ImGui.SameLine();
        }

        foreach (var currency in Service.CurrencyTracker.ActiveWarnings)
        {
            if (DisplaySettings.ShowIcon)
            {
                currency.DrawIcon();

                if (DisplaySettings.ShowName)
                {
                    ImGui.SameLine();
                    currency.DrawName(DisplaySettings.TextColor.Value);
                    ImGui.SameLine();
                }
            }
            else if (DisplaySettings.ShowName)
            {
                currency.DrawName(DisplaySettings.TextColor.Value);
                ImGui.SameLine();
            }
        }
    }
    
    private static void DrawNormalDisplay()
    {
        foreach (var currency in Service.CurrencyTracker.ActiveWarnings)
        {
            if (DisplaySettings.ShowWarningText)
            {
                ImGui.TextColored(DisplaySettings.TextColor.Value, Strings.OverlayWarningText);
                ImGui.SameLine();
            }

            if (DisplaySettings.ShowIcon)
            {
                currency.DrawIcon();
                ImGui.SameLine();
            }

            if (DisplaySettings.ShowName)
            {
                currency.DrawName(DisplaySettings.TextColor.Value);
            }
        }
    }
}
