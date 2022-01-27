using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.ClothesShop
{
    public class ClothesShop
    {
        public static List<ClothesShop> ClothesShops = new List<ClothesShop>();
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public List<ShopClothing> Items { get; set; }

        public ClothesShop(int id, string name, Vector3 pos, List<ShopClothing> items)
        {
            Id = id;
            Name = name;
            Position = pos;
            Items = items;

            NAPI.Task.Run(() =>
            {
                NAPI.Blip.CreateBlip(73, Position, 1, 28, $"Kleidungsladen - {Name}", shortRange: true);
                NAPI.Marker.CreateMarker(2, Position, new Vector3(), new Vector3(), 1, new Color(51, 204, 255), true);
                var colshape = NAPI.ColShape.CreateCylinderColShape(Position, 2, 8, 0);
                colshape.SetSharedData("ShopId", Id);
                colshape.SetSharedData("Type", "ClothesShop");
                ClothesShops.Add(this);
            });
        }
    }
}
