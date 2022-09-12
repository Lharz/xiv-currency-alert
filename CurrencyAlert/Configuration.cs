using CurrencyAlert.Enum;
using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace CurrencyAlert
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 5;

        public Dictionary<Currency, bool> AlertEnabled { get; set; } = new Dictionary<Currency, bool>();
        public Dictionary<Currency, int> Threshold { get; set; } = new Dictionary<Currency , int>();

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public Configuration()
        {
            EnumHelper.Each<Currency>(currency =>
            {
                this.AlertEnabled[currency] = true;
                var defaultValue = EnumHelper.GetAttributeOfType<DefaultThresholdAttribute>(currency);
                this.Threshold[currency] = defaultValue.Value;
            });
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}
