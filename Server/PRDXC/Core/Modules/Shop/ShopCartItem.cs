using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Shop
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

        public ShopCartItem(int id, string name, int price, int amount)
        {
            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
        }
    }
}
