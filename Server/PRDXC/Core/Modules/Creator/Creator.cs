using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using PRDXC.Core.Modules.Client;

namespace PRDXC.Core.Modules.Creator
{
    public class Creator : Script
    {
        [RemoteEvent]
        public void SaveCreatorChanges(Player player, string _data)
        {
            if(!player.HasData("PXPlayer")) return;
            var pxPlayer = player.GetData<PXPlayer>("PXPlayer");

            var cc = NAPI.Util.FromJson<CreatorInfo>(_data);
            pxPlayer.Customization = cc.ToServerModel();
            pxPlayer.Load();
            player.TriggerEvent("CreatorFinished");
            pxPlayer.Customization.FinishedCreation = true;
            NAPI.Task.Run(() =>
            {
                player.Health = player.GetData<int>("Health");
                player.Armor = player.GetData<int>("Armor");
                player.Position = player.GetData<Vector3>("Pos");
                player.TriggerEvent("ShowComponent", "Hud", true, NAPI.Util.ToJson(new HudInfo(pxPlayer.Money, pxPlayer.MPClient.Name, pxPlayer.Id)));
                player.Dimension = 0;
            }, 2000);
        }
    }
}
