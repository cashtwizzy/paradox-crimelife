using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Garage;

namespace PRDXC.Core.Modules.Inventory
{
    public class InventoryHandler : Script
    {
        [RemoteEvent]
        public void GetInventoryData(Player player)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            PXVehicle nearbyVehicle = null;

            var vehicles = NAPI.Pools.GetAllVehicles();
            for (var i = 0; i < vehicles.Count; i++)
                if (vehicles[i].Position.DistanceTo(player.Position) < 5)
                {
                    if (!vehicles[i].HasData("PXVehicle")) continue;
                    var foundVehicle = vehicles[i].GetData<PXVehicle>("PXVehicle");
                    if(!foundVehicle.Locked && !foundVehicle.TrunkLocked)
                    {
                        nearbyVehicle = foundVehicle;
                        break;
                    }
                }

            player.TriggerEvent("ShowComponent", "Inventory", true, new InventoryData(pxPlayer.Inventory, nearbyVehicle == null ? null : nearbyVehicle.Trunk, nearbyVehicle == null ? -1 : nearbyVehicle.MPVehicle.Id).ToJson()); ;
        }

        [RemoteEvent]
        public void UseInventoryItem(Player player, int slot)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            pxPlayer.Inventory.UseItem(pxPlayer, slot);
        }

        [RemoteEvent]
        public void ThrowInventoryItem(Player player, int slot, int amount)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            if(!pxPlayer.Inventory.RemoveItem(slot, amount))
                pxPlayer.Notify("Information", "Ein Fehler ist aufgetreten!");
        }

        [RemoteEvent]
        public void MoveAllToSlot(Player player, int oldSlot, int newSlot)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            if(!pxPlayer.Inventory.MoveAllToSlot(oldSlot, newSlot))
                pxPlayer.Notify("Information", "Ein Fehler ist aufgetreten!");
        }

        [RemoteEvent]
        public void MoveAmountToSlot(Player player, int oldSlot, int newSlot, int amount)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            if (!pxPlayer.Inventory.MoveAmountToSlot(oldSlot, newSlot, amount))
                pxPlayer.Notify("Information", "Ein Fehler ist aufgetreten!");
        }

        [RemoteEvent]
        public void MoveAmountToSlotTrunk(Player player, int vehId, int oldSlot, int newSlot, int amount)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var mpVehicle = NAPI.Pools.GetAllVehicles().Find(x => x.Id == vehId);
            if (!mpVehicle.HasData("PXVehicle")) return;
            var pxVehicle = mpVehicle.GetData<PXVehicle>("PXVehicle");

            if (!pxVehicle.Trunk.MoveAmountToSlot(oldSlot, newSlot, amount))
                pxPlayer.Notify("Information", "Ein Fehler ist aufgetreten!");
        }

        [RemoteEvent]
        public void MoveAllToSlotTrunk(Player player, int vehId, int oldSlot, int newSlot)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var mpVehicle = NAPI.Pools.GetAllVehicles().Find(x => x.Id == vehId);
            if (!mpVehicle.HasData("PXVehicle")) return;
            var pxVehicle = mpVehicle.GetData<PXVehicle>("PXVehicle");

            if (!pxVehicle.Trunk.MoveAllToSlot(oldSlot, newSlot))
                pxPlayer.Notify("Information", "Ein Fehler ist aufgetreten!");
        }

        [RemoteEvent]
        public void MoveItemToContainer(Player player, int vehId, int oldSlot, int newSlot, bool toTrunk)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var mpVehicle = NAPI.Pools.GetAllVehicles().Find(x => x.Id == vehId);
            if (!mpVehicle.HasData("PXVehicle")) return;
            var pxVehicle = mpVehicle.GetData<PXVehicle>("PXVehicle");
            
            if (toTrunk)
            {
                var oldItem = pxPlayer.Inventory.Items.Find(x => x.Slot == oldSlot);
                if (pxVehicle.Trunk.Items.Find(x => x.Slot == newSlot) != null || oldItem == null)
                    return;

                player.PlayAnimation("mp_common", "givetake2_a", (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody));
                pxPlayer.Inventory.Items.Remove(oldItem);
                oldItem.Slot = newSlot;
                pxVehicle.Trunk.Items.Add(oldItem);
            }
            else
            {
                var oldItem = pxVehicle.Trunk.Items.Find(x => x.Slot == oldSlot);
                if (pxPlayer.Inventory.Items.Find(x => x.Slot == newSlot) != null || oldItem == null)
                    return;

                player.PlayAnimation("mp_common", "givetake2_a", (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody));
                pxVehicle.Trunk.Items.Remove(oldItem);
                oldItem.Slot = newSlot;
                pxPlayer.Inventory.Items.Add(oldItem);
            }

            NAPI.Task.Run(() => { player.StopAnimation(); }, 2250);
        }
    }
}
