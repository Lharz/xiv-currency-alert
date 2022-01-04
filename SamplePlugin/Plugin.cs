﻿using CurrencyAlert.Enum;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;

namespace CurrencyAlert
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Currency Alert";

        private const string commandName = "/currencyalert";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private Configuration Configuration { get; init; }
        private PluginUI PluginUi { get; init; }
        private State State { get; init; }
        private CurrencyManager CurrencyManager { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.State = new State();

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            this.PluginUi = new PluginUI(this.Configuration);

            this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Let you configure alert thresholds for various currencies"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            this.CurrencyManager = new CurrencyManager(this.State, this.Configuration);
            this.PluginInterface.UiBuilder.Draw += this.CurrencyManager.Update;
        }

        private void OnCurrencyThresholdReached(Currency currency)
        {
            this.State.AlertVisible[currency] = true;
        }

        public void Dispose()
        {
            this.PluginUi.Dispose();
            this.CommandManager.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            this.DrawConfigUI();
        }

        private void DrawUI()
        {
            this.PluginUi.Draw(State);
        }

        private void DrawConfigUI()
        {
            this.State.SettingsVisible = true;
        }
    }
}
