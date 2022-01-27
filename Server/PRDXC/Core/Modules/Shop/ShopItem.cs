using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Shop
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public ShopItem(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
            ShopHandler.ShopItems.Add(this);
        }
    }
}
