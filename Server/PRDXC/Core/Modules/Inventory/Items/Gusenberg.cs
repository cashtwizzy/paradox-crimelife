using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Gusenberg : ItemModel
    {
        public Gusenberg() : base("Gusenberg", "Tommygun", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                if (pxPlayer.Loadout.Contains(0x61012683)) return false;
                pxPlayer.Loadout.Add(0x61012683);
                pxPlayer.MPClient.GiveWeapon((WeaponHash)0x61012683, 9999);
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
