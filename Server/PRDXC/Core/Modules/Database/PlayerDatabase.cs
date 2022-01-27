using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Creator;

namespace PRDXC.Core.Modules.Database
{
    public static class PlayerDatabase
    {
        public static async Task<DataTable> GetPlayerData(string name, ulong social, string serial)
        {
            var cmd = new MySqlCommand("SELECT * FROM accounts WHERE name=@n OR social=@sc OR serial=@se");
            cmd.Parameters.AddWithValue("@n", name);
            cmd.Parameters.AddWithValue("@sc", social);
            cmd.Parameters.AddWithValue("@se", serial);
            return await Resource.Database.QueryResult(cmd);
        }

        public static async Task<bool> PlayerNameTaken(string name)
        {
            var cmd = new MySqlCommand("SELECT * FROM accounts WHERE name=@n");
            cmd.Parameters.AddWithValue("@n", name);
            var res = await Resource.Database.QueryResult(cmd);
            return res.Rows.Count > 0;
        }

        public static async Task CreatePlayerAccount(string name, string pass, ulong social, string serial)
        {
            var cmd = new MySqlCommand("INSERT INTO accounts(name, pass, social, serial, health, armor, position, loadout, dead, custom, admin, money, inventory, clothes) VALUES(@n, @p, @sc, @se, 100, 0, @pos, \"[]\", 0, @cu, 0, 50000, @inv, @clo)");
            cmd.Parameters.AddWithValue("@n", name);
            cmd.Parameters.AddWithValue("@p", pass);
            cmd.Parameters.AddWithValue("@sc", social);
            cmd.Parameters.AddWithValue("@se", serial);
            cmd.Parameters.AddWithValue("@pos", NAPI.Util.ToJson(new Vector3(-1037.6504f, -2737.4753f, 20.16927f)));
            cmd.Parameters.AddWithValue("@cu", NAPI.Util.ToJson(new Customization()));
            cmd.Parameters.AddWithValue("@inv", NAPI.Util.ToJson(new Inventory.Inventory().ToDBInventory()));
            cmd.Parameters.AddWithValue("@clo", NAPI.Util.ToJson(new Clothes()));
            await Resource.Database.Query(cmd);
        }

        public static async Task UpdateSocialAndSerial(ulong social, string serial)
        {
            var cmd = new MySqlCommand("UPDATE accounts SET social=@sc, serial=@sc WHERE id=@id");
            cmd.Parameters.AddWithValue("@sc", social);
            cmd.Parameters.AddWithValue("@se", serial);
            await Resource.Database.Query(cmd);
        }

        public static async Task UpdatePlayerAccount(PXPlayer pxPlayer)
        {
            var cmd = new MySqlCommand("UPDATE accounts SET name=@n, health=@h, armor=@a, position=@pos, loadout=@load, dead=@d, custom=@cu, admin=@admin, money=@money, inventory=@inv, clothes=@clo WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", pxPlayer.Id);
            cmd.Parameters.AddWithValue("@n", pxPlayer.MPClient.Name);
            cmd.Parameters.AddWithValue("@h", pxPlayer.MPClient.Health);
            cmd.Parameters.AddWithValue("@a", pxPlayer.MPClient.Armor);
            cmd.Parameters.AddWithValue("@pos", NAPI.Util.ToJson(pxPlayer.MPClient.Position));
            cmd.Parameters.AddWithValue("@load", NAPI.Util.ToJson(pxPlayer.Loadout));
            cmd.Parameters.AddWithValue("@d", Convert.ToInt16(pxPlayer.Dead));
            cmd.Parameters.AddWithValue("@cu", NAPI.Util.ToJson(pxPlayer.Customization));
            cmd.Parameters.AddWithValue("@admin", (int)pxPlayer.AdminRank);
            cmd.Parameters.AddWithValue("@money", (int)pxPlayer.Money);
            cmd.Parameters.AddWithValue("@inv", NAPI.Util.ToJson(pxPlayer.Inventory.ToDBInventory()));
            cmd.Parameters.AddWithValue("@clo", NAPI.Util.ToJson(pxPlayer.Clothes));
            await Resource.Database.Query(cmd);
        }
    }
}
