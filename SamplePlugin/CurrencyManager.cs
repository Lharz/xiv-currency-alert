using CurrencyAlert.Enum;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAlert
{
    internal unsafe class CurrencyManager
    {
        private InventoryManager* Inventory { get; }
        private InventoryContainer* CurrencyContainer { get; }
        public State? State { get; }
        public Configuration Configuration { get; }

        public CurrencyManager(State? state, Configuration configuration)
        {
            State = state;
            Configuration = configuration;

            Inventory = InventoryManager.Instance();
            CurrencyContainer = this.Inventory->GetInventoryContainer(InventoryType.Currency);
        }

        public void Update()
        {

        }
    }
}
