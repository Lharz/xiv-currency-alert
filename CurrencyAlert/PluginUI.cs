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
                    var name = EnumHelper.GetAttributeOfType<NameAttribute>(currency).Value;
                    ImGui.Text($"You need to spend your {name}");
                }

                ImGui.End();
            });
        }     

        public void DrawSettingsWindow()
        {
            ImGui.SetNextWindowSize(new Vector2(700, 500), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Currency Alert Configuration Window", ref this.settingsVisible))
            {
                if (ImGui.BeginTabBar("AlertsConfiguration_Tabs"))
                {
                    EnumHelper.Each<Currency>(currency =>
                    {
                        var name = EnumHelper.GetAttributeOfType<NameAttribute>(currency).Value;
                        var category = EnumHelper.GetAttributeOfType<CategoryAttribute>(currency).Value;
                        var alertEnabled = this.configuration.AlertEnabled[currency];

                        if (ImGui.BeginTabItem(category))
                        {
                            if (ImGui.Checkbox($"{name} Alert Enabled", ref alertEnabled))
                            {
                                this.configuration.AlertEnabled[currency] = alertEnabled;
                                this.configuration.Save();
                            }

                            var thresholdValue = this.configuration.Threshold[currency];

                            if (ImGui.InputInt($"{name} Threshold Value", ref thresholdValue, 1, 1,
                                this.configuration.AlertEnabled[currency] ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly))
                            {
                                this.configuration.Threshold[currency] = thresholdValue;
                                this.configuration.Save();
                            }

                            ImGui.EndTabItem();
                        }
                    });
                }
            }

            ImGui.End();
        }
    }
}
