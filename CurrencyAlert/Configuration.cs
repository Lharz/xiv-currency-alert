using CurrencyAlert.Helper;
using CurrencyAlert.Provider;
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

        public bool UiLocked { get; set; } = false;
        public Dictionary<int, bool> AlertEnabled { get; } = new Dictionary<int, bool>();
        public Dictionary<int, int> Threshold { get; } = new Dictionary<int, int>();

        public Configuration()
        {
            foreach (var currency in CurrencyProvider.Instance.GetAll())
            {
                this.AlertEnabled[currency.Id] = true;
                this.Threshold[currency.Id] = currency.DefaultThreshold;
            }
        }

        public void Initialize()
        {
           
        }

        public void Save()
        {
            PluginHelper.PluginInterface.SavePluginConfig(this);
        }
    }
}
