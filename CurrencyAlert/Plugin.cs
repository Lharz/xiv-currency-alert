using CurrencyAlert.Enum;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game;
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
        private PluginUI PluginUI { get; init; }

        [PluginService] public static ChatGui Chat { get; private set; } = null!;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            this.PluginUI = new PluginUI(this.Configuration);

            this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Lets you configure alert thresholds for various currencies"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.PluginUI.Dispose();
            this.CommandManager.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            this.DrawConfigUI();
        }

        private void DrawUI()
        {
            // TODO: move this logic elsewhere
            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();

                EnumHelper.Each<Currency>(currency =>
                {
                    var itemID = EnumHelper.GetAttributeOfType<ItemIDAttribute>(currency).Value;
                    int quantity = inventoryManager->GetInventoryItemCount((uint)itemID);

                    if (this.Configuration.AlertEnabled[currency] && quantity >= this.Configuration.Threshold[currency])
                    {
                        this.PluginUI.AlertVisible[currency] = true;
                    }
                    else
                    {
                        this.PluginUI.AlertVisible[currency] = false;
                    }
                });
            }

            this.PluginUI.Draw();
        }

        private void DrawConfigUI()
        {
            this.PluginUI.SettingsVisible = true;
        }
    }
}
