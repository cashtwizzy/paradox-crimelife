using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Specialcarbine : ItemModel
    {
        public Specialcarbine() : base("Specialcarbine", "Krasse Waffe junge", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                if (pxPlayer.Loadout.Contains(0x969C3D67)) return false;
                pxPlayer.Loadout.Add(0x969C3D67);
                pxPlayer.MPClient.GiveWeapon((WeaponHash)0x969C3D67, 9999);
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
