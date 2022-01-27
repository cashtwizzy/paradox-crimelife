using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Shop
{
    public class Shop
    {
        public static List<Shop> Shops = new List<Shop>();
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        public Shop(int id, string name, Vector3 pos)
        {
            Id = id;
            Name = name;
            Position = pos;

            NAPI.Task.Run(() =>
            {
                NAPI.Blip.CreateBlip(52, Position, 1, 25, $"24/7 - {Name}", shortRange: true);
                NAPI.Marker.CreateMarker(2, Position, new Vector3(), new Vector3(), 1, new Color(51, 204, 255), true);
                var colshape = NAPI.ColShape.CreateCylinderColShape(Position, 2, 8, 0);
                colshape.SetSharedData("ShopId", Id);
                colshape.SetSharedData("Type", "Shop");
                Shops.Add(this);
            });
        }
    }
}
