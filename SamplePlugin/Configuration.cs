using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace CurrencyAlert
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public bool PoeticsThresholdEnabled { get; set; } = true;
        public int PoeticsThreshold { get; set; } = 1500;
        public bool StormSealsThresholdEnabled { get; set; } = true;
        public int StormSealsThreshold { get; set; } = 40000;

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

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
