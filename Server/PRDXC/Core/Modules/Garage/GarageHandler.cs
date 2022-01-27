using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Database;
using PRDXC.Core.Modules.Inventory;

namespace PRDXC.Core.Modules.Garage
{
    public class GarageHandler : Script
    {
        [RemoteEvent]
        public void OpenGarage(Player player, int garageId)
        {
            try
            {
                if (!player.HasData("PXPlayer")) return;
                var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

                if (player.Position.DistanceToSquared(Garage.Garages.Find(x => x.Id == garageId)?.GaragePoint) > 5) return;
                var garageVehicles = new List<GarageVehicle>();
                var _dbvehicles = VehicleDatabase.GetPlayerVehicles(pxPlayer.Id).Result;
                for (var i = 0; i < _dbvehicles.Rows.Count; i++)
                    if (Convert.ToBoolean(_dbvehicles.Rows[i]["parked"]) == true)
                    {
                        var trunk = NAPI.Util.FromJson<DBInventory>(_dbvehicles.Rows[i]["trunk"]).ToInventory();
                        garageVehicles.Add(new GarageVehicle(
                            (int)_dbvehicles.Rows[i]["id"],
                            (string)_dbvehicles.Rows[i]["name"],
                            (string)_dbvehicles.Rows[i]["plate"],
                            (string)_dbvehicles.Rows[i]["regdate"],
                            (int)_dbvehicles.Rows[i]["owner"],
                            trunk.GetInventoryWeight(),
                            trunk.MaxKG,
                            NAPI.Util.FromJson<Tuning>(_dbvehicles.Rows[i]["tuning"])
                            ));
                    }

                PXVehicle nearbyVehicle = null;
                var vehicles = NAPI.Pools.GetAllVehicles();
                for (var i = 0; i < vehicles.Count; i++)
                    if (vehicles[i].Position.DistanceTo(player.Position) < 35)
                    {
                        if (!vehicles[i].HasData("PXVehicle") || vehicles[i].GetData<PXVehicle>("PXVehicle").Owner != pxPlayer.Id) continue;
                        if (nearbyVehicle == null)
                        {
                            nearbyVehicle = vehicles[i].GetData<PXVehicle>("PXVehicle");
                        }
                        else
                        {
                            if (vehicles[i].Position.DistanceToSquared(player.Position) < nearbyVehicle.MPVehicle.Position.DistanceTo(player.Position))
                                nearbyVehicle = vehicles[i].GetData<PXVehicle>("PXVehicle");
                        }
                    }
                var clientData = new GarageData(garageId, garageVehicles, nearbyVehicle?.ToGarageVehicle());
                player.TriggerEvent("ShowComponent", "Garage", true, NAPI.Util.ToJson(clientData));
            }
            catch (Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
            }
        }

        [RemoteEvent]
        public void ParkVehicle(Player player, int id)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");
            var pxVehicle = PXVehicle.Vehicles.Find(x => x.Id == id);
            if(pxVehicle == null || pxVehicle.Owner != pxPlayer.Id) return;

            VehicleDatabase.SetVehicleParked(id, true).GetAwaiter();
            PXVehicle.Vehicles.Remove(pxVehicle);
            pxVehicle.MPVehicle.Delete();
        }

        [RemoteEvent]
        public void TakeVehicle(Player player, int garageId, int id)
        {
            if (!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var vehData = VehicleDatabase.GetVehicleData(id).Result;
            var row = vehData.Rows[0];
            var pxVehicle = new PXVehicle(
                (int)row["id"],
                (string)row["name"],
                (string)row["plate"],
                (string)row["regdate"],
                (int)row["owner"],
                NAPI.Util.FromJson<Inventory.Inventory>(row["trunk"]),
                NAPI.Util.FromJson<Tuning>(row["tuning"]),
                (int)row["color1"],
                (int)row["color2"],
                (int)row["lights"]
                );
            var garage = Garage.Garages.Find(x => x.Id == garageId);
            var freeSpawnPoint = GetFreeGarageSpawn(garage.SpawnPoints);
            if (freeSpawnPoint == null)
            {
                pxPlayer.Notify("Information", "Es ist kein Ausparkpunkt frei.");
                return;
            }
            var veh = NAPI.Vehicle.CreateVehicle(
                NAPI.Util.GetHashKey(pxVehicle.Name),
                freeSpawnPoint,
                garage.SpawnPointRotation[garage.SpawnPoints.IndexOf(freeSpawnPoint)],
                pxVehicle.PrimaryColor,
                pxVehicle.SecondaryColor,
                pxVehicle.Plate,
                255,
                true,
                false,
                0);

            pxVehicle.ApplyVehicleTo(veh);
            VehicleDatabase.SetVehicleParked(pxVehicle.Id, false).GetAwaiter();
        }

        private Vector3 GetFreeGarageSpawn(List<Vector3> spawns)
        {
            Vector3 freespawn = null;
            foreach(var spawn in spawns)
            {
                bool free = true;
                foreach(var veh in NAPI.Pools.GetAllVehicles())
                {
                    if (veh.Position.DistanceTo(spawn) < 3)
                    {
                        free = false;
                        break;
                    }
                }

                if (free)
                {
                    freespawn = spawn;
                    break;
                }
            }

            return freespawn;
        }

        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void PlayerEnterVehicleAttempt(Player player, Vehicle veh, sbyte seat)
        {
            if (!veh.HasData("PXVehicle") || !player.HasData("PXPlayer")) return;
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            if (pxVehicle.Locked)
                player.StopAnimation();
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void PlayerEnterVehicle(Player player, Vehicle veh, sbyte seat)
        {
            if (!veh.HasData("PXVehicle") || !player.HasData("PXPlayer")) return;
            var pxVehicle = veh.GetData<PXVehicle>("PXVehicle");

            pxVehicle.MPVehicle.EngineStatus = pxVehicle.Engine;
        }
    }
}
