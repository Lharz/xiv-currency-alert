using CurrencyAlert.Provider;
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
        public Dictionary<int, bool> AlertVisible { get; set; } = new Dictionary<int, bool>();

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;

            foreach (var currency in CurrencyProvider.Instance.GetAll())
            {
                this.AlertVisible[currency.Id] = false;
            }
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
            foreach (var currency in CurrencyProvider.Instance.GetAll())
            {
                if (!this.AlertVisible[currency.Id])
                    continue;

                ImGui.SetNextWindowSize(new Vector2(375, 10), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSizeConstraints(new Vector2(375, 10), new Vector2(float.MaxValue, float.MaxValue));

                var isVisible = this.AlertVisible[currency.Id];

                var guiOptions = ImGuiWindowFlags.NoScrollbar |
                    ImGuiWindowFlags.NoScrollWithMouse |
                    ImGuiWindowFlags.AlwaysAutoResize |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoFocusOnAppearing;

                if (configuration.UiLocked)
                    guiOptions |= ImGuiWindowFlags.NoMove;

                if (ImGui.Begin("Currency Alert", ref isVisible, guiOptions))
                {
                    ImGui.Text($"You need to spend your");

                    if (currency.Image != null)
                    {
                        ImGui.SameLine();
                        ImGui.Image(currency.Image.ImGuiHandle, new Vector2(22, 22));
                    }

                    ImGui.SameLine();

                    ImGui.Text($"{currency.Name}");

                    ImGui.End();
                }
            }
        }     

        public void DrawSettingsWindow()
        {
            ImGui.SetNextWindowSize(new Vector2(700, 500), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Currency Alert Configuration Window", ref this.settingsVisible))
            {
                var uiLocked = this.configuration.UiLocked;

                if (ImGui.Checkbox("Lock alert window", ref uiLocked))
                {
                    this.configuration.UiLocked = uiLocked;
                    this.configuration.Save();
                }

                if (ImGui.BeginTabBar("AlertsConfiguration_Tabs"))
                {
                    foreach (var currency in CurrencyProvider.Instance.GetAll())
                    {
                        var name = currency.Name;
                        var category = currency.Category;
                        var alertEnabled = this.configuration.AlertEnabled[currency.Id];

                        if (ImGui.BeginTabItem(category.ToString()))
                        {
                            if (currency.Image != null)
                            {
                                ImGui.Image(currency.Image.ImGuiHandle, new Vector2(22, 22));
                                ImGui.SameLine();
                            }

                            ImGui.Text($"{currency.Name}");

                            if (ImGui.Checkbox($"Enabled##{name}", ref alertEnabled))
                            {
                                this.configuration.AlertEnabled[currency.Id] = alertEnabled;
                                this.configuration.Save();
                            }

                            ImGui.SameLine();

                            var thresholdValue = this.configuration.Threshold[currency.Id];

                            if (ImGui.InputInt($"Threshold##{name}", ref thresholdValue, 1, 1,
                                this.configuration.AlertEnabled[currency.Id] ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly))
                            {
                                this.configuration.Threshold[currency.Id] = thresholdValue;
                                this.configuration.Save();
                            }

                            ImGui.Separator();

                            ImGui.EndTabItem();
                        }
                    }

                    ImGui.EndTabBar();
                }

                ImGui.End();
            }
        }
    }
}
