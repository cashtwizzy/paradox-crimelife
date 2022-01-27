using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Garage;

namespace PRDXC.Core.Modules.XMenu
{
    public class XMenu : Script
    {
        [RemoteEvent]
        public void XMenu_LockVehicle(Player player, Vehicle veh = null)
        {
            if (veh == null || !player.HasData("PXPlayer") || !veh.HasData("PXVehicle")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            if (pxVehicle.Owner != pxPlayer.Id) return;

            pxVehicle.Locked = !pxVehicle.Locked;
            pxVehicle.MPVehicle.Locked = pxVehicle.Locked;
            if(pxVehicle.Locked) pxVehicle.TrunkLocked = true;
            pxVehicle.MPVehicle.SetSharedData("LockState", pxVehicle.Locked);
            pxPlayer.Notify("Information", $"Du hast dein Fahrzeug {(pxVehicle.Locked ? "abgeschlossen" : "aufgeschlossen")}.");
        }

        [RemoteEvent]
        public void XMenu_LockTrunk(Player player, Vehicle veh = null)
        {
            if (veh == null || !player.HasData("PXPlayer") || !veh.HasData("PXVehicle")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            if (pxVehicle.Owner != pxPlayer.Id) return;

            pxVehicle.TrunkLocked = !pxVehicle.TrunkLocked;
            pxPlayer.Notify("Information", $"Du hast den Kofferraum {(pxVehicle.TrunkLocked ? "abgeschlossen" : "aufgeschlossen")}.");
        }

        [RemoteEvent]
        public void XMenu_ToggleEngine(Player player, Vehicle veh = null)
        {
            if (veh == null || !player.HasData("PXPlayer") || !veh.HasData("PXVehicle")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            if (pxVehicle.Owner != pxPlayer.Id) return;

            pxVehicle.Engine = !pxVehicle.Engine;
            pxVehicle.MPVehicle.EngineStatus = pxVehicle.Engine;
            pxVehicle.MPVehicle.SetSharedData("EngineState", pxVehicle.Engine);
            pxPlayer.Notify("Information", $"Motor {(pxVehicle.Engine ? "eingeschaltet" : "ausgeschaltet")}.");
        }

        [RemoteEvent]
        public void XMenu_RepairVehicle(Player player, Vehicle veh = null)
        {
            if(veh == null || !player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var repkit = Inventory.Inventory.GetModelFromName("Reperaturkasten");

            if (!pxPlayer.Inventory.HasItem(repkit))
            {
                pxPlayer.Notify("Information", "Du benötigst einen Reperaturkasten dafür.");
                return;
            }

            pxPlayer.Inventory.RemoveItem(repkit, 1);
            pxPlayer.Progress("Reperatur", "Du reparierst ein Fahrzeug.", 5000, "XMenu/repair.png");
            player.PlayAnimation("mini@repair", "fixing_a_player", 33);
            NAPI.Task.Run(() =>
            {
                player.StopAnimation();
                veh.Repair();
            }, 4950);
        }

        [RemoteEvent]
        public void XMenu_GetVehicleInfo(Player player, Vehicle veh = null)
        {
            if (veh == null || !player.HasData("PXPlayer") || !veh.HasData("PXVehicle")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            
            pxPlayer.Notify("Information", $"Name: {pxVehicle.Name}, ID: {pxVehicle.Id}.");
        }

        [RemoteEvent]
        public void XMenu_GetPlayerInfo(Player player, Player target = null)
        {
            if (target == null || !player.HasData("PXPlayer") || !target.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxTarget = target.GetData<PXPlayer>("PXPlayer");


            pxPlayer.Notify("Information", $"ID: {pxTarget.Id}.");
        }

        [RemoteEvent]
        public void XMenu_CuffPlayer(Player player, Player target = null)
        {
            if (target == null || !player.HasData("PXPlayer") || !target.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxTarget = target.GetData<PXPlayer>("PXPlayer");
            if (pxPlayer.Cuffed) return;
            if (!pxTarget.Cuffed)
            {
                if (!pxPlayer.Inventory.HasItem(Inventory.Inventory.GetModelFromName("Seile")))
                {
                    pxPlayer.Notify("Information", "Du benötigst 1x Seil dafür.");
                    return;
                }

                pxPlayer.Inventory.RemoveItem(Inventory.Inventory.GetModelFromName("Seile"), 1);

                pxTarget.MPClient.TriggerEvent("SetCuffState", true);
                pxTarget.Cuffed = true;
                NAPI.Player.PlayPlayerAnimation(pxPlayer.MPClient, 1, "mp_arrest_paired", "cop_p2_back_left");
                NAPI.Player.PlayPlayerAnimation(pxTarget.MPClient, 1, "mp_arrest_paired", "crook_p2_back_left");
                NAPI.Task.Run(() =>
                {
                    pxPlayer.MPClient.StopAnimation();
                    NAPI.Player.PlayPlayerAnimation(pxTarget.MPClient, 2, "mp_arrest_paired", "crook_p2_back_left");
                }, 5000);
            }
            else
            {
                pxPlayer.MPClient.PlayAnimation("mp_arresting", "a_uncuff", 1);
                pxTarget.MPClient.PlayAnimation("mp_arresting", "b_uncuff", 1);
                NAPI.Task.Run(() =>
                {
                    pxTarget.MPClient.StopAnimation();
                    pxPlayer.MPClient.StopAnimation();
                    pxTarget.MPClient.TriggerEvent("SetCuffState", false);
                    pxTarget.Cuffed = false;
                }, 4750);
            }
        }

        [RemoteEvent]
        public void GivePlayerMoney(Player player, int amount, Player target = null)
        {
            if (target == null || !player.HasData("PXPlayer") || !target.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxTarget = target.GetData<PXPlayer>("PXPlayer");

            if (pxPlayer.Money < amount) return;

            pxPlayer.SetMoney(pxPlayer.Money - amount);
            pxTarget.SetMoney(pxTarget.Money + amount);
            pxPlayer.Notify("Information", "Du hast jemandem Geld zugesteckt.");
            pxTarget.Notify("Information", "Jemand hat dir Geld zugesteckt.");
        }
    }
}
