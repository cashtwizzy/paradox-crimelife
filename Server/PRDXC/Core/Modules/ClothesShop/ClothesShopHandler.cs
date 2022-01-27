//Twizzy#9904 https://github.com/cashtwizzy
using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;

namespace PRDXC.Core.Modules.ClothesShop
{
    public class ClothesShopHandler : Script
    {
        [RemoteEvent]
        public void OpenClothesShop(Player player, int shopId)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var shop = ClothesShop.ClothesShops.Find(x => x.Id == shopId);

            if (player.Position.DistanceToSquared(shop?.Position) > 5) return;
            player.TriggerEvent("ShowComponent", "ClothesShop", true, NAPI.Util.ToJson(shop.Items));
        }
        
        [RemoteEvent]
        public void BuyClothing(Player player, int cat, int draw, int tex, int price)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.Money < price) return;

            pxPlayer.SetMoney(pxPlayer.Money - price);
            pxPlayer.Clothes.SetClothing(cat, draw, tex);
        }

        [RemoteEvent]
        public void UpdateClothes(Player player)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            pxPlayer.ApplyClothes();
        }
    }
}
//Twizzy#9904 https://github.com/cashtwizzy