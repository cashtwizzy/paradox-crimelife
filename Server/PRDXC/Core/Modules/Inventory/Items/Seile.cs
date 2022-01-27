using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Seile : ItemModel
    {
        public Seile() : base("Seile", "Fessel die brees", 5, 0.5f) { }

        public override bool Use(PXPlayer pxPlayer)
        {
            try
            {
                // Finish when XMenue is ready
            }
            catch (Exception ex)
            {
                Resource.Logger.Write(ex.Message, Logger.LogType.Error).GetAwaiter();
            }
            return false;
        }
    }
}
