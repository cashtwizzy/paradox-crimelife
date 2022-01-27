using GTANetworkAPI;
using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory.Items
{
    public class Reperaturkasten : ItemModel
    {
        public Reperaturkasten() : base("Reperaturkasten", "Repair die noice Karre", 5, 0.5f) { }

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
