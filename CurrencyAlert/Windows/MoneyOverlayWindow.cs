using System;
using System.Collections.Generic;
using System.Numerics;
using CurrencyAlert.DataModels;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using KamiLib.Atk;
using KamiLib.Drawing;

namespace CurrencyAlert.Windows;

public unsafe class MoneyOverlayWindow : Window
{
    private static MoneyOverlaySettings Settings => Service.Configuration.MoneyOverlaySettings;
    private readonly Dictionary<CurrencyName, CurrencyInfo> currencyInfoCache = new();
    public MoneyOverlayWindow() : base("###CurrencyAlertMoneyOverlayWindow")
    {
        IsOpen = true;

        Flags |= ImGuiWindowFlags.NoDecoration;
        Flags |= ImGuiWindowFlags.NoBackground;
        Flags |= ImGuiWindowFlags.NoFocusOnAppearing;
        Flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
        Flags |= ImGuiWindowFlags.NoNavFocus;
        Flags |= ImGuiWindowFlags.NoInputs;
        Flags |= ImGuiWindowFlags.NoNavInputs;
        Flags |= ImGuiWindowFlags.NoMouseInputs;
    }

    public override void PreDraw()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8.0f));
        
        var baseNode = new BaseNode("_Money");
        var rootNode = baseNode.GetRootNode();
        var height = rootNode->Height;
        
        Position = new Vector2(rootNode->X, rootNode->Y - height * 3);
        Size = new Vector2(rootNode->Width, rootNode->Height * 3);
    }

    public override void Draw()
    {
        if (!Settings.Enabled) return;
        
        foreach (var currency in Settings.MoneyOverlayCurrencies)
        {
            if (currency.Value != CurrencyName.Unselected)
            {
                if (currencyInfoCache.TryGetValue(currency.Value, out var currencyInfo))
                {
                    DrawCurrency(currencyInfo);
                }
                else
                {
                    var newCurrencyInfo = new CurrencyInfo(currency.Value);
                    currencyInfoCache.Add(currency.Value, newCurrencyInfo);
                    DrawCurrency(newCurrencyInfo);
                }
            }
            else
            {
                ImGuiHelpers.ScaledDummy(28.0f);
            }
        }
    }

    public override void PostDraw()
    {
        ImGui.PopStyleVar();
        ImGui.PopStyleVar();
    }

    private void DrawCurrency(CurrencyInfo currency)
    {
        var icon = currency.IconTexture;
        if (icon is null) return;

        var iconSize = new Vector2(36.0f);
        
        var region = ImGui.GetContentRegionAvail();
        var iconStartX = region.X - iconSize.X;
        
        ImGui.SetCursorPos(new Vector2(iconStartX, ImGui.GetCursorPos().Y));
        ImGui.Image(icon.ImGuiHandle, iconSize);

        var text = currency.GetCurrentQuantity().ToString("N0");
        var textSize = ImGui.CalcTextSize(text);

        ImGui.SameLine();
        TextOutlined(new Vector2(iconStartX - textSize.X, ImGui.GetCursorPos().Y + iconSize.Y / 2.0f - textSize.Y / 2.0f), text);
    }
    
    public static void TextOutlined(Vector2 startingPosition, string text)
    {
        startingPosition = startingPosition.Ceil();

        var outlineThickness = (int)MathF.Ceiling(1);
        
        for (var x = -outlineThickness; x <= outlineThickness; ++x)
        {
            for (var y = -outlineThickness; y <= outlineThickness; ++y)
            {
                if (x == 0 && y == 0) continue;
                
                ImGui.SetCursorPos(startingPosition + new Vector2(x, y));
                ImGui.TextColored(Colors.Black ,text);
            }
        }

        ImGui.SetCursorPos(startingPosition);
        ImGui.TextColored(Colors.White ,text);
    }
}

public static class VectorExtensions
{
    public static Vector2 Ceil(this Vector2 data)
    {
        return new Vector2(MathF.Ceiling(data.X), MathF.Ceiling(data.Y));
    }
}