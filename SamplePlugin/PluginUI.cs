using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Numerics;

namespace CurrencyAlert
{
    public enum CurrencySlot
    {
        StormSeals = 1,
        WolfMarks = 4,
        Poetics = 6,
        AlliedSeals = 8
    };

    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        private bool poeticsAlertVisible = false;
        public bool PoeticsAlertVisible 
        {
            get { return this.poeticsAlertVisible; }
            set { this.poeticsAlertVisible = value; }
        }

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            DrawMainWindow();
            DrawSettingsWindow();
        }

        public void DrawMainWindow()
        {
            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();
                InventoryContainer* currencyContainer = inventoryManager->GetInventoryContainer(InventoryType.Currency);

                // Poetics: 6
                // Wolf Marks: 4
                // Allied Seal: 8
                // Company Seals: 1,2,3

                uint poetics = currencyContainer->GetInventorySlot((int) CurrencySlot.Poetics)->Quantity;

                bool poeticsThresholdEnabled = this.configuration.PoeticsThresholdEnabled;
                uint poeticsThreshold = (uint) this.configuration.PoeticsThreshold;

                PoeticsAlertVisible = poeticsThresholdEnabled && poetics >= poeticsThreshold;

                if (!PoeticsAlertVisible)
                {
                    return;
                }

                ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));

                if (ImGui.Begin("DEPENSE TES POETICS MERDE", ref this.poeticsAlertVisible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize))
                {
                    ImGui.Text("DEPENSE TES POETICS MEEEEERDE");
                }

                ImGui.End();
            }
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(330, 120), ImGuiCond.Always);
            if (ImGui.Begin("Currency Alert Configuration Window", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                var poeticsThresholdEnabledConfig = this.configuration.PoeticsThresholdEnabled;

                if (ImGui.Checkbox("Poetics Threshold Enabled", ref poeticsThresholdEnabledConfig))
                {
                    this.configuration.PoeticsThresholdEnabled = poeticsThresholdEnabledConfig;
                    this.configuration.Save();
                }

                var poeticsThresholdConfig = this.configuration.PoeticsThreshold;

                if (ImGui.InputInt("Poetics Threshold", ref poeticsThresholdConfig, 1, 1, this.configuration.PoeticsThresholdEnabled ? ImGuiInputTextFlags.None : ImGuiInputTextFlags.ReadOnly))
                {
                    this.configuration.PoeticsThreshold = poeticsThresholdConfig;
                    this.configuration.Save();
                }
            }
            ImGui.End();
        }
    }
}
