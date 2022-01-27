using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Schutzweste : ItemModel
    {
        public Schutzweste() : base("Schutzweste", "Eine Kugelsichere Weste die dir Schutz gegen Schusswaffen bietet.", 5, 1) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                pxPlayer.Progress("Schutzweste", "Du verwendest eine Schutzweste...");
                pxPlayer.MPClient.PlayAnimation("anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 1 << 0 | 1 << 5);
                NAPI.Task.Run(() =>
                {
                    if (pxPlayer.MPClient == null || !pxPlayer.MPClient.Exists) return;
                    pxPlayer.MPClient.StopAnimation();
                    pxPlayer.MPClient.Armor = 100;
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
