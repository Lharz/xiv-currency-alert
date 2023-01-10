using System.Numerics;
using ImGuiNET;
using KamiLib.Configuration;

namespace CurrencyAlert.DataModels;

public record TrackedCurrency(CurrencyName Name, Setting<int> Threshold, Setting<bool> Enabled)
{
    private static DisplaySettings DisplaySettings => Service.Configuration.DisplaySettings;
    
    public CurrencyInfo CurrencyInfo => new(Name);

    public void Draw(bool ignoreDisplaySettings = false)
    {
        if (DisplaySettings.ShowIcon || ignoreDisplaySettings)
        {
            if (CurrencyInfo.IconTexture is { } icon)
            {
                ImGui.Image(icon.ImGuiHandle, new Vector2(20.0f));
                ImGui.SameLine();
            }
        }

        if (DisplaySettings.ShowName || ignoreDisplaySettings)
        {
            ImGui.Text(CurrencyInfo.ItemName);
        }
    }
}
