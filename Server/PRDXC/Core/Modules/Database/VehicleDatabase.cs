using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace PRDXC.Core.Modules.Database
{
    public static class VehicleDatabase
    {
        public static async Task LoadGarages()
        {
            var cmd = new MySqlCommand("SELECT * FROM garage");
            var res = await Resource.Database.QueryResult(cmd);
            for (var i = 0; i < res.Rows.Count; i++)
                new Garage.Garage(
                    (int)res.Rows[i]["id"],
                    (string)res.Rows[i]["name"],
                    NAPI.Util.FromJson<Vector3>(res.Rows[i]["point"]),
                    NAPI.Util.FromJson<List<Vector3>>(res.Rows[i]["spawns"]),
                    NAPI.Util.FromJson<List<float>>(res.Rows[i]["rotations"])
                    );
        }
        public static async Task<DataTable> GetPlayerVehicles(int owner)
        {
            var cmd = new MySqlCommand("SELECT * FROM vehicles WHERE owner=@owner");
            cmd.Parameters.AddWithValue("@owner", owner);
            return await Resource.Database.QueryResult(cmd);
        }
        public static async Task<DataTable> GetVehicleData(int id)
        {
            var cmd = new MySqlCommand("SELECT * FROM vehicles WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            return await Resource.Database.QueryResult(cmd);
        }

        public static async Task CreateVehicle(int id)
        {

        }

        public static async Task UpdateVehicle(int id)
        {

        }

        public static async Task SetVehicleParked(int id, bool parked)
        {
            var cmd = new MySqlCommand("UPDATE vehicles SET parked=@parked WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@parked", parked);
            await Resource.Database.Query(cmd);
        }

        public static async Task SetAllVehiclesParked()
        {
            var cmd = new MySqlCommand("UPDATE vehicles SET parked=1");
            await Resource.Database.Query(cmd);
        }
    }
}
