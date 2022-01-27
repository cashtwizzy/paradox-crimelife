using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.ClothesShop
{
    public class ShopClothing
    {
        public string Name { get; set; }
        public int MaxColor { get; set; }
        public int Category { get; set; }
        public int Drawable { get; set; }
        public int Price { get; set; }

        public ShopClothing(string name, int maxc, int cat, int draw, int price)
        {
            Name = name;
            MaxColor = maxc;
            Category = cat;
            Drawable = draw;
            Price = price;
        }
    }
}
