using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory
{
    public class DBItem
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Slot { get; set; }

        public DBItem() { }

        public DBItem(string name, int amount, int slot)
        {
            Name = name;
            Amount = amount;
            Slot = slot;
        }

        public Item ToItem()
        {
            return new Item(Inventory.GetModelFromName(Name), Amount, Slot);
        }
    }
}
