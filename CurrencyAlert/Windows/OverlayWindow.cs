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
}

public class OverlayWindow : Window
{
    private static OverlaySettings OverlaySettings => Service.Configuration.OverlaySettings;
    private static DisplaySettings DisplaySettings => Service.Configuration.DisplaySettings;
    
    public OverlayWindow() : base("Currency Alert Overlay")
    {
        KamiCommon.CommandManager.AddCommand(new OverlayCommands());
        
        Flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
        Flags |= ImGuiWindowFlags.NoFocusOnAppearing;
        Flags |= ImGuiWindowFlags.NoNavFocus;
    }

    public override void PreOpenCheck()
    {
        IsOpen = OverlaySettings.Show;
        
        if (Condition.IsBoundByDuty()) IsOpen = false;
        if (Condition.IsInCutsceneOrQuestEvent()) IsOpen = false;
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
    
    private static void DrawMinimalDisplay()
    {
        if (DisplaySettings.ShowWarningText)
        {
            ImGui.Text(Strings.OverlayWarningText);
            ImGui.SameLine();
        }

        foreach (var currency in Service.CurrencyTracker.ActiveWarnings)
        {
            currency.Draw();
        }
    }
    
    private static void DrawNormalDisplay()
    {
        foreach (var currency in Service.CurrencyTracker.ActiveWarnings)
        {
            if (DisplaySettings.ShowWarningText)
            {
                ImGui.Text(Strings.OverlayWarningText);
                ImGui.SameLine();
            }
            
            currency.Draw();
            ImGui.SameLine();
        }
    }
}
