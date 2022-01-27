//Twizzy#9904 https://github.com/cashtwizzy
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Hud
{
    public class NotificationInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int Time { get; set; }

        public NotificationInfo(string title, string msg, int time)
        {
            Title = title;
            Message = msg;
            Time = time;
        }
    }
}
//Twizzy#9904 https://github.com/cashtwizzy