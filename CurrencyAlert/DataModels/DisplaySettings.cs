using System.Numerics;
using KamiLib.Configuration;
using KamiLib.Drawing;

namespace CurrencyAlert.DataModels;

public class DisplaySettings
{
    public Setting<bool> ShowIcon = new(true);
    public Setting<bool> ShowName = new(true);
    public Setting<bool> ShowWarningText = new(true);
    public Setting<Vector4> TextColor = new(Colors.White);
}