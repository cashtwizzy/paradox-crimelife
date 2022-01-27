using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Hud;

namespace PRDXC.Core.Modules.Chat
{
    public class AdminCommands : Script
    {
        [RemoteEvent]
        public void CMD_pos(Player player)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            Resource.Logger.Write($"[{player.Name}] ⤵\nPosition: {player.Position.ToString().Replace(',', '.').Replace(' ', ',')}\nRotation: {player.Heading}", Logger.LogType.Success).GetAwaiter();
        }

        [RemoteEvent]
        public void CMD_revive(Player player, string targetName)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            if(int.TryParse(targetName, out var targetId))
            {
                var target = NAPI.Pools.GetAllPlayers().Find(e =>
                {
                    if(e.HasData("PXPlayer"))
                        return e.GetData<PXPlayer>("PXPlayer").Id == targetId;
                    return false;
                });
                if (target == null)
                {
                    pxPlayer.Notify("Information", $"Spieler nicht gefunden. (ID: {targetId})");
                    return;
                }

                if (!target.HasData("PXPlayer")) return;
                var pxTarget = target.GetData<PXPlayer>("PXPlayer");
                pxTarget.Dead = false;
                pxTarget.MPClient.TriggerEvent("DisableDeathScreen");
                pxPlayer.Notify("Information", $"Du hast {target.Name} wiederbelebt.");
                NAPI.Task.Run(() =>
                {
                    NAPI.Player.SpawnPlayer(pxTarget.MPClient, pxTarget.MPClient.Position);
                    pxTarget.MPClient.StopAnimation();
                }, 2700);
            }
            else
            {
                var target = NAPI.Pools.GetAllPlayers().Find(e => e.Name == targetName);
                if (target == null)
                {
                    pxPlayer.Notify("Information", $"Spieler nicht gefunden. (Name: {targetName})");
                    return;
                }

                if (!target.HasData("PXPlayer")) return;
                var pxTarget = target.GetData<PXPlayer>("PXPlayer");
                pxTarget.Dead = false;
                pxTarget.MPClient.TriggerEvent("DisableDeathScreen");
                pxPlayer.Notify("Information", $"Du hast den Spieler {target.Name} wiederbelebt.");
                NAPI.Task.Run(() =>
                {
                    NAPI.Player.SpawnPlayer(pxTarget.MPClient, pxTarget.MPClient.Position);
                    pxTarget.MPClient.StopAnimation();
                }, 2700);
            }
        }

        [RemoteEvent]
        public void CMD_giveweapon(Player player, string weaponName = "", int ammo = 9999)
        {
            if (weaponName == "" || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            var weapon = NAPI.Util.WeaponNameToModel(weaponName);
            if(weapon == 0) return;
            pxPlayer.Notify("Information", $"Du hast dir die Waffe {weapon} gegeben.");
            pxPlayer.Loadout.Add((uint)weapon);
            player.GiveWeapon(weapon, ammo);
        }

        [RemoteEvent]
        public void CMD_setrank(Player player, string targetName = "", int rank = -1)
        {
            if (targetName == "" || rank == -1 || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Highteam) return;

            if (int.TryParse(targetName, out var targetId))
            {
                var target = NAPI.Pools.GetAllPlayers().Find(e =>
                {
                    if (e.HasData("PXPlayer"))
                        return e.GetData<PXPlayer>("PXPlayer").Id == targetId;
                    return false;
                });

                if (target == null)
                {
                    pxPlayer.Notify("Information", $"Spieler nicht gefunden. (ID: {targetName})");
                    return;
                }

                if (!target.HasData("PXPlayer")) return;
                var pxTarget = target.GetData<PXPlayer>("PXPlayer");
                pxTarget.AdminRank = (AdminRank)rank;
                pxPlayer.Notify("Information", $"Du hast {target.Name} den Adminrang {rank} gesetzt.");
            }
            else
            {
                var target = NAPI.Pools.GetAllPlayers().Find(e => e.Name == targetName);
                if (target == null)
                {
                    pxPlayer.Notify("Information", $"Spieler nicht gefunden. (Name: {targetName})");
                    return;
                }

                if (!target.HasData("PXPlayer")) return;
                var pxTarget = target.GetData<PXPlayer>("PXPlayer");
                pxTarget.AdminRank = (AdminRank)rank;
                pxPlayer.Notify("Information", $"Du hast {target.Name} den Adminrang {rank} gesetzt.");
            }
        }

        [RemoteEvent]
        public void CMD_veh(Player player, string vehicleName = "")
        {
            if (vehicleName == "" || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            var vehHash = NAPI.Util.VehicleNameToModel(vehicleName);
            if (vehHash == 0) return;

            pxPlayer.Notify("Information", $"Du hast das Fahrzeug {vehicleName} gespawned.");
            var veh = NAPI.Vehicle.CreateVehicle(vehHash, player.Position, player.Heading, 1, 1, "PRDXC");
            player.SetIntoVehicle(veh, 0);
        }

        [RemoteEvent]
        public void CMD_announce(Player player, string title = "", string message = "")
        {
            if (title == "" || message == "" || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Highteam) return;

            var players = NAPI.Pools.GetAllPlayers();
            for(var i = 0; i < players.Count; i++)
                players[i].TriggerEvent("DisplayAnnounce", NAPI.Util.ToJson(new AnnounceInfo(title, message)));
        }

        [RemoteEvent]
        public void CMD_aduty(Player player)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            pxPlayer.Aduty = !pxPlayer.Aduty;

            player.TriggerEvent("ADUTY", pxPlayer.Aduty);
            player.SetSharedData("ADUTY", pxPlayer.Aduty);
            player.SetSharedData("ADMINRANK", (pxPlayer.AdminRank > AdminRank.Team) ? $"Admin - {pxPlayer.Id}\nim Dienst" : $"Team - {pxPlayer.Id}\nim Dienst");
            if (pxPlayer.Aduty)
                player.SetSkin((pxPlayer.AdminRank > AdminRank.Team) ? NAPI.Util.GetHashKey("s_m_y_blackops_01") : NAPI.Util.GetHashKey("s_m_y_blackops_02"));
            else
                pxPlayer.Load();
        }

        [RemoteEvent]
        public void SetVisible(Player player, bool state)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            player.SetSharedData("Visible", state);
        }

        [RemoteEvent]
        public void CMD_giveitem(Player player, string item = "", int amount = -1)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            var itemModel = Inventory.Inventory.GetModelFromName(item);

            if (itemModel == null)
            {
                pxPlayer.Notify("Information", "Das angegebene Item wurde nicht gefunden.");
                return;
            }

            pxPlayer.Inventory.AddItem(itemModel, amount);
        }

        [RemoteEvent]
        public void CMD_playanim(Player player, string dict = "", string anim = "", int flag = -1)
        {
            if (dict == "" || anim == "" || flag == -1 || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.AdminRank < AdminRank.Team) return;

            NAPI.Player.PlayPlayerAnimation(player, flag, dict, anim, 1);
        }
    }
}
