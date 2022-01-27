using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Hud
{
    public class AnnounceInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public AnnounceInfo(string title, string msg)
        {
            Title = title;
            Message = msg;
        }
    }
}
