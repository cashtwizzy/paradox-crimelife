using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Database;
using PRDXC.Core.Modules.Hud;

namespace PRDXC.Core.Modules.Events
{
    public class PlayerEvents : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(Player player)
        {
            player.Dimension = (ushort)(10000 + player.Id);
            player.Position = new Vector3(3094.4954, 5939.8125, 159.07666);
            player.SetSharedData("ADUTY", false);
            player.SetSharedData("ADMINRANK", "undefined");
            player.SetSharedData("Visible", true);
            player.TriggerEvent("ShowComponent", "Login", true);
            Resource.Logger.Write($"Player {player.Name} Connected", Logger.LogType.Success).GetAwaiter();
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            if(!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            PlayerDatabase.UpdatePlayerAccount(pxPlayer).GetAwaiter();
        }

        [ServerEvent(Event.PlayerDeath)]
        public void PlayerDeath(Player player, Player killer, uint reason)
        {
            try
            {
                if (!player.HasData("PXPlayer")) return;
                var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
                pxPlayer.Dead = true;
                pxPlayer.DiedAt = DateTime.Now;
                NAPI.Player.SpawnPlayer(player, player.Position);
                player.TriggerEvent("StartDeathScreen");
                pxPlayer.MPClient.PlayAnimation("combat@damage@rb_writhe", "rb_writhe_loop", 1 << 0 | 1 << 5);
                if (pxPlayer.Cuffed)
                {
                    pxPlayer.Cuffed = false;
                    pxPlayer.MPClient.TriggerEvent("SetCuffState", false);
                }
                if (killer == null || !killer.Exists || player == killer || !killer.HasData("PXPlayer")) return;
                var pxKiller = killer.GetData<PXPlayer>("PXPlayer");
                pxPlayer.Notify("Information", $"Du wurdest von {killer.Name}({pxKiller.Id}) getötet.");
                pxKiller.Notify("Information", $"Du hast {player.Name} getötet.");
            }
            catch(Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
                Resource.Logger.Write(ex.StackTrace, Logger.LogType.Error).GetAwaiter();
            }
        }
    }
}
