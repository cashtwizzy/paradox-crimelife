using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Carbinerifle_MK2 : ItemModel
    {
        public Carbinerifle_MK2() : base("Carbinerifle_MK2", "Karabiner MK2", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                if (pxPlayer.Loadout.Contains(0xFAD1F1C9)) return false;
                pxPlayer.Loadout.Add(0xFAD1F1C9);
                pxPlayer.MPClient.GiveWeapon((WeaponHash)0xFAD1F1C9, 9999);
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
