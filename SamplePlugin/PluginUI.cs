using CurrencyAlert.Enum;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Numerics;

namespace CurrencyAlert
{
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            
        }
 
        public void Draw(State state)
        {
            DrawMainWindow(state);

            if (state.SettingsVisible)
                DrawSettingsWindow(state);
        }

        public void DrawMainWindow(State state)
        {
            foreach (var visible in state.AlertVisible)
            {
                if (!visible.Value)
                    continue;

                ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));

                var visibleValue = visible.Value;

                if (ImGui.Begin("Currency Alert", ref visibleValue, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize))
                {
                    ImGui.Text("You need to spend your " + visible.Key);
                }

                ImGui.End();
            }
        }     

        public void DrawSettingsWindow(State state)
        {
            bool visible = state.SettingsVisible;

            ImGui.SetNextWindowSize(new Vector2(400, 600), ImGuiCond.Always);
            if (ImGui.Begin("Currency Alert Configuration Window", ref visible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                foreach (Currency currency in System.Enum.GetValues(typeof(Currency)))
                {
                    ImGui.Text(currency.ToString());

                    var alertEnabled = configuration.AlertEnabled[currency];

                    if (ImGui.Checkbox("Alert Enabled", ref alertEnabled))
                    {
                        configuration.AlertEnabled[currency] = alertEnabled;
                    }

                    var thresholdValue = configuration.Threshold[currency];

                    if (ImGui.InputInt("Threshold Value", ref thresholdValue, 1, 1,
                        configuration.AlertEnabled[currency] ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly))
                    {
                        configuration.Threshold[currency] = thresholdValue;
                    }
                } 
            }

            if (ImGui.Button("Save"))
            {
                this.configuration.Save();
            }

            ImGui.End();
        }
    }
}
