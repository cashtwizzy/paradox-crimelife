using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Heavyrifle : ItemModel
    {
        public Heavyrifle() : base("Heavyrifle", "Krasse Waffe junge", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                if (pxPlayer.Loadout.Contains(0xC78D71B4)) return false;
                pxPlayer.Loadout.Add(0xC78D71B4);
                pxPlayer.MPClient.GiveWeapon((WeaponHash)0xC78D71B4, 9999);
                return true;
            }
            catch (Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
            }
            return false;
        }
    }
}
