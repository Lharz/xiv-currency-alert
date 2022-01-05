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

        public CurrencyManager()
        {
            Inventory = InventoryManager.Instance();
            CurrencyContainer = this.Inventory->GetInventoryContainer(InventoryType.Currency);
        }

        public void Update(State state, Configuration configuration)
        {
            foreach (Currency currency in System.Enum.GetValues(typeof(Currency)))
            {
                var slot = EnumHelper.GetAttributeOfType<SlotAttribute>(currency).Value;
                uint quantity = this.CurrencyContainer->GetInventorySlot((int) slot)->Quantity;

                if (configuration.AlertEnabled[currency] && quantity >= configuration.Threshold[currency])
                {
                    state.AlertVisible[currency] = true;
                }
                else
                {
                    state.AlertVisible[currency] = false;
                }
            }
        }
    }
}
