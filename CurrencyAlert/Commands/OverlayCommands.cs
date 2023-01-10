using System.Collections.Generic;
using CurrencyAlert.Localization;
using KamiLib.ChatCommands;
using KamiLib.Interfaces;

namespace CurrencyAlert.Commands;

public class OverlayCommands : IPluginCommand
{
    public string CommandArgument => "overlay";

    public IEnumerable<ISubCommand> SubCommands { get; } = new List<ISubCommand>
    {
        new SubCommand
        {
            CommandKeyword = "show",
            CommandAction = () =>
            {
                Service.Configuration.OverlaySettings.Show.Value = true;
                Service.Configuration.Save();
            },
            GetHelpText = () => Strings.Commands_ShowOverlayHelp,
        },
        new SubCommand
        {
            CommandKeyword = "hide",
            CommandAction = () =>
            {
                Service.Configuration.OverlaySettings.Show.Value = false;
                Service.Configuration.Save();
            },
            GetHelpText = () => Strings.Commands_HideOverlayHelp,
        },
        new SubCommand
        {
            CommandKeyword = "toggle",
            CommandAction = () =>
            {
                Service.Configuration.OverlaySettings.Show.Value = !Service.Configuration.OverlaySettings.Show.Value;
                Service.Configuration.Save();
            },
            GetHelpText = () => Strings.Commands_ToggleOverlayHelp,
        },
    };
}