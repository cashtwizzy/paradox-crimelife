using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Client
{
    public class Clothes
    {
        public int Mask { get; set; }
        public int Mask2 { get; set; }
        public int Top { get; set; }
        public int Top2 { get; set; }
        public int Shirt { get; set; }
        public int Shirt2 { get; set; }
        public int Body { get; set; }
        public int Pants { get; set; }
        public int Pants2 { get; set; }
        public int Shoes { get; set; }
        public int Shoes2 { get; set; }

        public Clothes()
        {
            Mask = 0;
            Mask2 = 0;
            Top = 0;
            Top2 = 0;
            Shirt = 0;
            Shirt2 = 0;
            Body = 0;
            Pants = 0;
            Pants2 = 0;
            Shoes = 0;
            Shoes2 = 0;
        }

        public Clothes(int m1, int m2, int t1, int t2, int s1, int s2, int b, int p1, int p2, int sh1, int sh2)
        {
            Mask = m1;
            Mask2 = m2;
            Top = t1;
            Top2 = t2;
            Shirt = s1;
            Shirt2 = s2;
            Body = b;
            Pants = p1;
            Pants2 = p2;
            Shoes = sh1;
            Shoes2 = sh2;
        }

        public void ApplyClothes(Player player)
        {
            player.SetClothes(1, Mask, Mask2); // Mask
            player.SetClothes(11, Top, Top2); // Top
            player.SetClothes(8, Shirt, Shirt2); // Undershirt
            player.SetClothes(3, Body, 0); // Body
            player.SetClothes(4, Pants, Pants2); // Legs
            player.SetClothes(6, Shoes, Shoes2); // Shoes
        }

        public void SetClothing(int cat, int draw, int tex)
        {
            switch (cat)
            {
                case 1:
                    Mask = draw;
                    Mask2 = tex;
                    break;
                case 11:
                    Top = draw;
                    Top2 = tex;
                    break;
                case 8:
                    Shirt = draw;
                    Shirt2 = tex;
                    break;
                case 3:
                    Body = draw;
                    break;
                case 4:
                    Pants = draw;
                    Pants2 = tex;
                    break;
                case 6:
                    Shoes = draw;
                    Shoes2 = tex;
                    break;
            }
        }
    }
}
