using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory
{
    public class Item
    {
        public ItemModel Model { get; set; }
        public int Amount { get; set; }
        public int Slot { get; set; }
        
        public Item(ItemModel model, int amount, int slot)
        {
            Model = model;
            Amount = amount;
            Slot = slot;
        }

        public void Use(PXPlayer pxPlayer)
        {
            Model.Use(pxPlayer);
        }

        public DBItem ToDBItem()
        {
            return new DBItem(Model.Name, Amount, Slot);
        }
    }
}
