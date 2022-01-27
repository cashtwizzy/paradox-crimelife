using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Medikit : ItemModel
    {
        public Medikit() : base("Medikit", "Ein Verbandskasten mit allen nötigen dingen um dich zu verarzten.", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                pxPlayer.Progress("Medikit", "Du verwendest ein Medikit...");
                pxPlayer.MPClient.PlayAnimation("anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 1 << 0 | 1 << 5);
                NAPI.Task.Run(() =>
                {
                    if (pxPlayer.MPClient == null || !pxPlayer.MPClient.Exists) return;
                    pxPlayer.MPClient.StopAnimation();
                    pxPlayer.MPClient.Health = 100;
                }, 5000);
            }
            catch (Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
            }
            return false;
        }
    }
}
