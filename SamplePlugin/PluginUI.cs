using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Numerics;

namespace SamplePlugin
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        //public unsafe InventoryManager* InventoryManager { get; }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
            //this.InventoryManager = InventoryManager.Instance();
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

                uint poetics = currencyContainer->GetInventorySlot(6)->Quantity;
                uint wolfMarks = currencyContainer->GetInventorySlot(4)->Quantity;
                uint stormSeals = currencyContainer->GetInventorySlot(1)->Quantity;
                uint serpentSeals = currencyContainer->GetInventorySlot(2)->Quantity;
                uint flameSeals = currencyContainer->GetInventorySlot(3)->Quantity;

                uint poeticsThreshold = (uint) this.configuration.PoeticsThreshold;

                Visible = false;

                if (poetics >= poeticsThreshold)
                {
                    Visible = true;
                }

                if (!Visible)
                {
                    return;
                }

                ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));

                if (ImGui.Begin("My Amazing Window", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
                {
                    if (ImGui.Button("Show Settings"))
                    {
                        SettingsVisible = true;
                    }

                    if (poetics >= poeticsThreshold)
                    {
                        ImGui.Text("DEPENSE TES POETICS MERDE");
                    }
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

            ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
            if (ImGui.Begin("A Wonderful Configuration Window", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                // can't ref a property, so use a local copy
                var configValue = this.configuration.PoeticsThreshold;
                if (ImGui.InputInt("Poetics Threshold", ref configValue))
                {
                    this.configuration.PoeticsThreshold = configValue;
                    // can save immediately on change, if you don't want to provide a "Save and Close" button
                    this.configuration.Save();
                }
            }
            ImGui.End();
        }
    }
}
