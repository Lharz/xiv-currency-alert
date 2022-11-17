using CurrencyAlert.Enum;
using CurrencyAlert.Provider;
using ImGuiNET;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Vector2 lastMainWindowPos = new Vector2 { X = 10, Y = 10 };

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

        int count = 0;

        void recordPos()
        {
            lastMainWindowPos = ImGui.GetWindowPos();

            if (this.configuration.StackDirection == StackDirection.Up)
            {
                lastMainWindowPos.Y += ImGui.GetWindowHeight();
            }
            else if (this.configuration.StackDirection == StackDirection.Left)
            {
                lastMainWindowPos.X += ImGui.GetWindowWidth();
            }
        }

        public void DrawMainWindow()
        {
            var guiOptions = 
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse |
                ImGuiWindowFlags.AlwaysAutoResize |
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoFocusOnAppearing;

            if (configuration.UiLocked)
                guiOptions |= ImGuiWindowFlags.NoMove;

            var visibleCurrency = CurrencyProvider.Instance.GetAll().Where(x => AlertVisible[x.Id]);

            if (ImGui.Begin("Currency Alert", guiOptions))
            {
                if (count == 0)
                {
                    recordPos();
                    count++;
                }
                else if (ImGui.IsMouseDragging(ImGuiMouseButton.Left))
                {
                    recordPos();
                }
                else
                {
                    Vector2 newPos = new Vector2 { X = lastMainWindowPos.X, Y = lastMainWindowPos.Y };

                    if (this.configuration.StackDirection == StackDirection.Up)
                    {
                        newPos.Y -= ImGui.GetWindowHeight();
                    }
                    else if (this.configuration.StackDirection == StackDirection.Left)
                    {
                        newPos.X -= ImGui.GetWindowWidth();
                    }

                    ImGui.SetNextWindowPos(newPos);
                }

                bool first = true;
                foreach (var currency in visibleCurrency)
                {
                    ImGui.Begin("Currency Alert", guiOptions);

                    if (first)
                    {
                        ImGui.Text($"You need to spend your");
                        if (this.configuration.StackDirection == StackDirection.Left || this.configuration.StackDirection == StackDirection.Right)
                        {
                            first = false;
                        }
                    }

                    if (currency.Image != null)
                    {
                        ImGui.SameLine();
                        ImGui.Image(currency.Image.ImGuiHandle, new Vector2(22, 22));
                    }

                    if (!this.configuration.MinimalDisplay)
                    {
                        ImGui.SameLine();
                        ImGui.Text($"{currency.Name}");
                    }

                    if (this.configuration.StackDirection == StackDirection.Left || this.configuration.StackDirection == StackDirection.Right)
                    {
                        ImGui.SameLine();
                    }

                    ImGui.End();
                }
            }

            ImGui.End();
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

                var minimalDisplay = this.configuration.MinimalDisplay;
                if (ImGui.Checkbox("Minimal Display", ref minimalDisplay))
                {
                    this.configuration.MinimalDisplay = minimalDisplay;
                    this.configuration.Save();
                }

                int stackDirection = (int) this.configuration.StackDirection;
                if (ImGui.Combo("Stack Direction", ref stackDirection, new string[] { "Up", "Down", "Left", "Right" }, 4))
                {
                    this.configuration.StackDirection = (StackDirection) stackDirection;
                    this.configuration.Save();
                };

                if (ImGui.Button("Reset Position"))
                {
                    lastMainWindowPos = new Vector2 { X = 100, Y = 100 };
                }

                ImGui.NewLine();

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
