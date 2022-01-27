using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Inventory;

namespace PRDXC.Core.Modules.Shop
{
    public class ShopHandler : Script
    {
        public static List<ShopItem> ShopItems = new List<ShopItem>();

        [RemoteEvent]
        public void OpenShop(Player player, int shopId)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            if (player.Position.DistanceToSquared(Shop.Shops.Find(x => x.Id == shopId)?.Position) > 5) return;
            player.TriggerEvent("ShowComponent", "Shop", true, NAPI.Util.ToJson(ShopItems));
        }

        [RemoteEvent]
        public void BuyItems(Player player, string _data)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var Data = NAPI.Util.FromJson<List<ShopCartItem>>(_data);
            if(Data == null || Data.Count < 1) return;

            foreach (var item in Data)
            {
                var shopItem = ShopItems.Find(x => x.Id == item.Id);
                if (shopItem == null || item.Amount < 1 || item.Price < 1)
                {
                    pxPlayer.Notify("Information", "Es ist ein Fehler aufgetreten.");
                    break;
                }

                var price = shopItem.Price * item.Amount;
                if (pxPlayer.Money < price)
                {
                    pxPlayer.Notify("Information", "Du hast nicht so viel Geld.");
                    break;
                }

                if(!pxPlayer.Inventory.AddItem(Inventory.Inventory.GetModelFromName(item.Name), item.Amount))
                {
                    pxPlayer.Notify("Information", "Du hast nicht so viel Platz.");
                    break;
                }

                pxPlayer.SetMoney(pxPlayer.Money - price);
            }

            pxPlayer.Notify("24/7 Shop", "Vielen Dank für ihren Einkauf!");
        }
    }
}
