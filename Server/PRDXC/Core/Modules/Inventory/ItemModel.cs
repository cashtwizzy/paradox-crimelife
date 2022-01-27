using PRDXC.Core.Modules.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Inventory
{
    public abstract class ItemModel
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public float Weight { get; set; }
        public int MaxAmount { get; set; }

        public ItemModel(string name, string info, int maxAmount, float weight)
        {
            Name = name;
            Info = info;
            MaxAmount = maxAmount;
            Weight = weight;
        }

        public virtual bool Use(PXPlayer pxPlayer) { return false; }
    }
}
