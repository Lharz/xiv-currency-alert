using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using CurrencyAlert.Windows.Components;
using Dalamud.Interface;
using ImGuiNET;
using KamiLib.Drawing;
using KamiLib.Interfaces;
using KamiLib.Windows;

namespace CurrencyAlert.Windows;

public class ConfigurationWindow : SelectionWindow
{
    public ConfigurationWindow() : base("Currency Alert Configuration Window", 0.25f, 35.0f)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(350, 510),
            MaximumSize = new Vector2(9999,9999)
        };

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
    }

    protected override IEnumerable<ISelectable> GetSelectables()
    {
        return Service.Configuration.TrackedCurrencies
            .Select(tracked => new TrackedCurrencySelectable(tracked) as ISelectable)
            .Prepend(new GeneralSettingsSelectable());
    }
    
    protected override void DrawExtras()
    {
        DrawVersionNumber();
    }

    private static void DrawVersionNumber()
    {
        var assemblyInformation = Assembly.GetExecutingAssembly().FullName!.Split(',');
        var versionString = assemblyInformation[1].Replace('=', ' ');

        var stringSize = ImGui.CalcTextSize(versionString);

        var x = ImGui.GetContentRegionAvail().X / 2 - stringSize.X / 2;
        var y = ImGui.GetWindowHeight() - 30 * ImGuiHelpers.GlobalScale;
            
        ImGui.SetCursorPos(new Vector2(x, y));

        ImGui.TextColored(Colors.Grey, versionString);
    }
}
