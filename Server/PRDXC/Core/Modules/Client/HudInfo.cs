using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Client
{
    public class HudInfo
    {
        public int money { get; set; }
        public string name { get; set; }
        public int id { get; set; }

        public HudInfo(int money, string name, int id)
        {
            this.money = money;
            this.name = name;
            this.id = id;
        }
    }
}
