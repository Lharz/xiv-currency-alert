using System.Numerics;
using ImGuiNET;
using KamiLib.Configuration;

namespace CurrencyAlert.DataModels;

public record TrackedCurrency(CurrencyName Name, Setting<int> Threshold, Setting<bool> Enabled)
{
    private static DisplaySettings DisplaySettings => Service.Configuration.DisplaySettings;

    public CurrencyInfo CurrencyInfo()
    {
        return new CurrencyInfo(Name);
    }

    public void DrawIcon()
    {
        if (CurrencyInfo().IconTexture is { } icon)
        {
            ImGui.Image(icon.ImGuiHandle, new Vector2(20.0f));
        }
    }

    public void DrawName(Vector4 color)
    {
        ImGui.TextColored(color, CurrencyInfo().ItemName);
    }
}


