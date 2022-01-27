using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory
{
    public class InventoryData
    {
        public Inventory Inventory { get; set; }
        public Inventory Trunk { get; set; }
        public int VehId { get; set; }

        public InventoryData(Inventory inv, Inventory trunk = null, int vehid = -1)
        {
            Inventory = inv;
            Trunk = trunk;
            VehId = vehid;
        }

        public string ToJson()
        {
            return NAPI.Util.ToJson(this);
        }
    }
}
