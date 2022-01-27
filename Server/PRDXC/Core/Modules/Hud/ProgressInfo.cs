using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Hud
{
    public class ProgressInfo
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public int Time { get; set; }

        public ProgressInfo(string title, string msg, int time, string image = "")
        {
            Title = title;
            Desc = msg;
            Image = image;
            Time = time;
        }
    }
}
