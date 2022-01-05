﻿using CurrencyAlert.Enum;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurrencyAlert
{
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return settingsVisible; }
            set { settingsVisible = value; }
        }
        private bool debugVisible = false;
        public bool DebugVisible
        {
            get { return debugVisible; }
            set { debugVisible = value; }
        }
        public Dictionary<Currency, bool> AlertVisible { get; set; } = new Dictionary<Currency, bool>();

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;

            EnumHelper.Each<Currency>(currency => this.AlertVisible[currency] = false);
        }

        public void Dispose()
        {
            
        }
 
        public void Draw()
        {
            DrawMainWindow();

            if (this.SettingsVisible)
                DrawSettingsWindow();

            if (this.DebugVisible)
                DrawDebugWindow();
        }

        public void DrawMainWindow()
        {
            EnumHelper.Each<Currency>(currency =>
            {
                if (!this.AlertVisible[currency])
                    return;

                ImGui.SetNextWindowSize(new Vector2(375, 10), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSizeConstraints(new Vector2(375, 10), new Vector2(float.MaxValue, float.MaxValue));

                var isVisible = this.AlertVisible[currency];

                if (ImGui.Begin("Currency Alert", ref isVisible,
                    ImGuiWindowFlags.NoScrollbar |
                    ImGuiWindowFlags.NoScrollWithMouse |
                    ImGuiWindowFlags.AlwaysAutoResize |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoFocusOnAppearing
                    ))
                {
                    ImGui.Text($"You need to spend your {currency}");
                }

                ImGui.End();
            });
        }     

        public void DrawSettingsWindow()
        {
            ImGui.SetNextWindowSize(new Vector2(400, 600), ImGuiCond.Always);
            if (ImGui.Begin("Currency Alert Configuration Window", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                EnumHelper.Each<Currency>(currency =>
                {
                    var alertEnabled = this.configuration.AlertEnabled[currency];

                    if (ImGui.Checkbox($"{currency.ToString()} Alert Enabled", ref alertEnabled))
                    {
                        this.configuration.AlertEnabled[currency] = alertEnabled;
                        this.configuration.Save();
                    }

                    var thresholdValue = this.configuration.Threshold[currency];

                    if (ImGui.InputInt($"{currency.ToString()} Threshold Value", ref thresholdValue, 1, 1,
                        this.configuration.AlertEnabled[currency] ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly))
                    {
                        this.configuration.Threshold[currency] = thresholdValue;
                        this.configuration.Save();
                    }
                });
            }

#if DEBUG
            if (ImGui.Button("Open Debug"))
                this.DebugVisible = true;
#endif

            ImGui.End();
        }

        public void DrawDebugWindow()
        {
            ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));

            if (ImGui.Begin("Currency Alert Debug", ref this.debugVisible))
            {
                unsafe
                {
                    InventoryManager* inventoryManager = InventoryManager.Instance();
                    InventoryContainer* currencyContainer = inventoryManager->GetInventoryContainer(InventoryType.Currency);

                    for (var i = 0; i < 100000; ++i)
                    {
                        var item = currencyContainer->GetInventorySlot(i);

                        if (item == null)
                            continue;

                        ImGui.Text($"Index: {i}   Value: {item->Quantity}");
                    }
                }
            }

            ImGui.End();
        }
    }
}
