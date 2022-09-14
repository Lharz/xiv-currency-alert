using CurrencyAlert.Helper;
using CurrencyAlert.Provider;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace CurrencyAlert
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Currency Alert";

        private const string commandName = "/currencyalert";

        private Configuration Configuration { get; init; }
        private PluginUI PluginUI { get; init; }

        [PluginService] public static ChatGui Chat { get; private set; } = null!;

        private bool LoggedIn => PluginHelper.ClientState.LocalPlayer != null && PluginHelper.ClientState.LocalContentId != 0;

        public Plugin(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginHelper>();

            try
            {
                this.Configuration = PluginHelper.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            }
            catch (Exception e)
            {
                this.Configuration = new Configuration();
                this.Configuration.Save();

                PluginHelper.Chat.Print("Your CurrencyAlert configuration has been reset because of compatibility issues. Please check the plugin configuration window.");
            }

            this.Configuration.Initialize();

            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            this.PluginUI = new PluginUI(this.Configuration);

            PluginHelper.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Lets you configure alert thresholds for various currencies"
            });

            PluginHelper.PluginInterface.UiBuilder.Draw += DrawUI;
            PluginHelper.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.PluginUI.Dispose();
            PluginHelper.CommandManager.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            this.DrawConfigUI();
        }

        private void DrawUI()
        {
            if (!this.LoggedIn)
                return;

            this.UpdateAlerts();

            this.PluginUI.Draw();
        }

        private void UpdateAlerts()
        {
            // TODO: move this logic elsewhere
            // TODO: do this only every X seconds
            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();

                foreach (var currency in CurrencyProvider.Instance.GetAll())
                {
                    int quantity = inventoryManager->GetInventoryItemCount((uint)currency.Id);

                    if (this.Configuration.AlertEnabled[currency.Id] && quantity >= this.Configuration.Threshold[currency.Id])
                    {
                        this.PluginUI.AlertVisible[currency.Id] = true;
                    }
                    else
                    {
                        this.PluginUI.AlertVisible[currency.Id] = false;
                    }
                }
            }
        }

        private void DrawConfigUI()
        {
            this.PluginUI.SettingsVisible = true;
        }
    }
}
