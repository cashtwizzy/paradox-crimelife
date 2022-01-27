using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory
{
    public class DBInventory
    {
        public List<DBItem> Items { get; set; }
        public int MaxKG { get; set; }

        public DBInventory() { }

        public DBInventory(List<DBItem> items, int maxkg)
        {
            Items = items;
            MaxKG = maxkg;
        }

        public DBInventory(List<Item> items, int maxkg)
        {
            Items = GetDBItemsFromItems(items);
            MaxKG = maxkg;
        }

        private List<DBItem> GetDBItemsFromItems(List<Item> items)
        {
            List<DBItem> result = new List<DBItem>();
            for(var i = 0; i < items.Count; i++)
                result.Add(items[i].ToDBItem());
            return result;
        }

        public Inventory ToInventory()
        {
            return new Inventory(GetItemsFromDBItems(Items), MaxKG);
        }

        private List<Item> GetItemsFromDBItems(List<DBItem> items)
        {
            var result = new List<Item>();
            for (var i = 0; i < items.Count; i++)
                result.Add(items[i].ToItem());
            return result;
        }
    }
}
