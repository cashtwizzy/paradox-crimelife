//Twizzy#9904 https://github.com/cashtwizzy
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Database;
using GTANetworkAPI;
using PRDXC.Core.Modules.Creator;
using PRDXC.Core.Modules.Hud;

namespace PRDXC.Core.Modules.Login
{
    public class Login : Script
    {
        [RemoteEvent]
        public void PlayerLogin(Player player, string name, string pass)
        {
            try
            {
                var dbData = PlayerDatabase.GetPlayerData(name, player.SocialClubId, player.Serial).Result;
                if (dbData.Rows.Count > 0)
                {
                    var row = dbData.Rows[0];
                    if (Convert.ToUInt64(row["social"]) != 0 && (string)row["serial"] != "")
                    {
                        if (player.SocialClubId != Convert.ToUInt64(row["social"]) || player.Serial != (string)row["serial"])
                        {
                            player.Kick("Bitte melde dich im Support.");
                            return;
                        }
                    }
                    else
                    {
                        PlayerDatabase.UpdateSocialAndSerial(player.SocialClubId, player.Serial).GetAwaiter();
                    }

                    if (name != (string)row["name"] || pass != (string)row["pass"])
                    {
                        player.TriggerEvent("LoginDisplayError", "Die angegebenen Daten sind nicht korrekt.");
                        return;
                    }

                    var pxPlayer = new PXPlayer(
                        Convert.ToInt32(row["id"]),
                        Convert.ToInt32(row["money"]),
                        NAPI.Util.FromJson<List<uint>>(row["loadout"]),
                        NAPI.Util.FromJson<Inventory.DBInventory>(row["inventory"]).ToInventory(),
                        NAPI.Util.FromJson<Customization>(row["custom"]),
                        player,
                        (AdminRank)Convert.ToInt32(row["admin"]),
                        Convert.ToBoolean(row["dead"]),
                        NAPI.Util.FromJson<Clothes>(row["clothes"])
                        );

                    player.Name = name;
                    player.SetData("PXPlayer", pxPlayer);
                    player.TriggerEvent("LoginFinished", name);
                    player.TriggerEvent("SetHudData", NAPI.Util.ToJson(new HudInfo(pxPlayer.Money, name, pxPlayer.Id)));

                    NAPI.Task.Run(() =>
                    {
                        if (!pxPlayer.Customization.FinishedCreation)
                        {
                            player.Position = new Vector3(402.8664, -996.4108, -99.00027);
                            player.Heading = -185;
                            player.TriggerEvent("ShowComponent", "Creator", true, NAPI.Util.ToJson(new CreatorInfo()));
                            player.SetData("Health", Convert.ToInt32(row["health"]));
                            player.SetData("Armor", Convert.ToInt32(row["armor"]));
                            player.SetData("Pos", NAPI.Util.FromJson<Vector3>((string)row["position"]));
                            return;
                        }

                        player.Health = Convert.ToInt32(row["health"]);
                        player.Armor = Convert.ToInt32(row["armor"]);
                        player.Position = NAPI.Util.FromJson<Vector3>((string)row["position"]);
                        pxPlayer.Load();
                        player.Dimension = 0;
                        if (pxPlayer.AdminRank >= AdminRank.Team)
                            pxPlayer.Notify("Administration", "Deine Rechte wurden erfolgreich geladen.");
                            
                        if (pxPlayer.Dead)
                        {
                            pxPlayer.DiedAt = DateTime.Now;
                            pxPlayer.MPClient.TriggerEvent("StartDeathScreen");
                            NAPI.Player.PlayPlayerAnimation(pxPlayer.MPClient, (int)(1 << 0 | 1 << 5), "combat@damage@rb_writhe", "rb_writhe_loop");
                        }
                    }, 1800);
                }
                else
                {
                    if (PlayerDatabase.PlayerNameTaken(name).Result)
                    {
                        player.TriggerEvent("LoginDisplayError", "Dieser Name ist bereits vergeben.");
                        return;
                    }

                    PlayerDatabase.CreatePlayerAccount(name, pass, player.SocialClubId, player.Serial).GetAwaiter();
                    player.TriggerEvent("RegisterFinished");
                }
            }
            catch (Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
            }
        }
    }
}
//Twizzy#9904 https://github.com/cashtwizzy