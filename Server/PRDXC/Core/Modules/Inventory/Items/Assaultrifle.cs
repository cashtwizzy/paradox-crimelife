using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Assaultrifle : ItemModel
    {
        public Assaultrifle() : base("Assaultrifle", "Russische AK47", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                if (pxPlayer.Loadout.Contains(0xBFEFFF6D)) return false;
                pxPlayer.Loadout.Add(0xBFEFFF6D);
                pxPlayer.MPClient.GiveWeapon((WeaponHash)0xBFEFFF6D, 9999);
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
