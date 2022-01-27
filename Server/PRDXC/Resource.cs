//Twizzy#9904 https://github.com/cashtwizzy
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Database;
using PRDXC.Core.Modules.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace PRDXC
{
    public class Resource : Script
    {
        public static Logger Logger { get; } = new Logger();
        public static Database Database { get; } = new Database("SERVER=localhost;UID=root;DATABASE=prdxc;");
        private static Timer DeathTimer;
        private static Timer TenMinuteTimer;

        [ServerEvent(Event.ResourceStart)]
        public async Task ResourceStart()
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);

            DeathTimer = new Timer();
            DeathTimer.Interval = 60000;
            DeathTimer.Elapsed += DeathTick;
            DeathTimer.AutoReset = true;
            DeathTimer.Enabled = true;
            TenMinuteTimer = new Timer(120000);
            TenMinuteTimer.Elapsed += TenMinuteTick;
            TenMinuteTimer.AutoReset = true;
            TenMinuteTimer.Enabled = true;
            await VehicleDatabase.LoadGarages();
            await VehicleDatabase.SetAllVehiclesParked();
            await ShopDatabase.LoadShops();
            await ShopDatabase.LoadShopItems();
            await ShopDatabase.LoadClothesShops();
            NAPI.Task.Run(async () =>
            {
                await Logger.DrawLogo();
                await Logger.Write("Resource started! Twizzy#9904 ", LogType.Success);
            }, 250);
        }

        [ServerEvent(Event.ResourceStop)]
        public async Task ResourceStop()
        {
            await Logger.Write("Resource started!", LogType.Success);
        }

        private static void DeathTick(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            var players = NAPI.Pools.GetAllPlayers();
            for(var i = 0; i < players.Count; i++)
            {
                if (!players[i].HasData("PXPlayer")) return;
                var pxPlayer = players[i].GetData<PXPlayer>("PXPlayer");
                if (pxPlayer.Dead)
                {
                    if (pxPlayer.DiedAt.AddMinutes(5) > System.DateTime.Now)
                    {
                        pxPlayer.MPClient.TriggerEvent("StartDeathScreen");
                        pxPlayer.MPClient.PlayAnimation("combat@damage@rb_writhe", "rb_writhe_loop", 1);
                        return;
                    }

                    pxPlayer.MPClient.TriggerEvent("DisableDeathScreen");
                    NAPI.Task.Run(() =>
                    {
                        pxPlayer.Dead = false;
                        NAPI.Player.SpawnPlayer(pxPlayer.MPClient, new Vector3(-1043.023, -2746.636, 21.35941), 0);
                        pxPlayer.MPClient.RemoveAllWeapons();
                        pxPlayer.Loadout = new List<uint>();
                    }, 2700);
                }
            }
        }

        private static void TenMinuteTick(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            var players = NAPI.Pools.GetAllPlayers();
            for (var i = 0; i < players.Count; i++)
            {
                if (!players[i].HasData("PXPlayer")) continue;
                var pxPlayer = players[i].GetData<PXPlayer>("PXPlayer");

                PlayerDatabase.UpdatePlayerAccount(pxPlayer).GetAwaiter();
            }
        }
    }

    [Flags]
    public enum AnimationFlags
    {
        Loop = 1 << 0,
        StopOnLastFrame = 1 << 1,
        OnlyAnimateUpperBody = 1 << 4,
        AllowPlayerControl = 1 << 5,
        Cancellable = 1 << 7
    }
}
//Twizzy#9904 https://github.com/cashtwizzy