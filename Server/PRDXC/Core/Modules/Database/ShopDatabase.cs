using GTANetworkAPI;
using MySqlConnector;
using PRDXC.Core.Modules.ClothesShop;
using PRDXC.Core.Modules.Shop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRDXC.Core.Modules.Database
{
    public static class ShopDatabase
    {
        public static async Task LoadShops()
        {
            var cmd = new MySqlCommand("SELECT * FROM shops");
            var res = await Resource.Database.QueryResult(cmd);
            for (var i = 0; i < res.Rows.Count; i++)
                new Shop.Shop(
                    (int)res.Rows[i]["id"],
                    (string)res.Rows[i]["name"],
                    NAPI.Util.FromJson<Vector3>(res.Rows[i]["pos"])
                    );
        }

        public static async Task LoadShopItems()
        {
            var cmd = new MySqlCommand("SELECT * FROM shopitems");
            var res = await Resource.Database.QueryResult(cmd);
            for (var i = 0; i < res.Rows.Count; i++)
                new ShopItem(
                    (int)res.Rows[i]["id"],
                    (string)res.Rows[i]["name"],
                    (int)res.Rows[i]["price"]
                    );
        }

        public static async Task LoadClothesShops()
        {
            var cmd = new MySqlCommand("SELECT * FROM clothesshops");
            var res = await Resource.Database.QueryResult(cmd);
            for (var i = 0; i < res.Rows.Count; i++)
                new ClothesShop.ClothesShop(
                    (int)res.Rows[i]["id"],
                    (string)res.Rows[i]["name"],
                    NAPI.Util.FromJson<Vector3>(res.Rows[i]["pos"]),
                    NAPI.Util.FromJson<List<ShopClothing>>(res.Rows[i]["items"])
                    );
        }
    }
}
