using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRDXC.Core.Modules.Logger
{
    public class Logger
    {
        public Logger() { }

        public async Task Write(string msg, LogType type)
        {
            await Task.Run(() =>
            {
                Console.ForegroundColor = (ConsoleColor)type;
                Console.Write("[+] ");
                Console.ResetColor();
                Console.WriteLine(msg);
            });
        }

        public async Task DrawLogo()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("  mddddddddddddddhhdhhhhddhdddmNN        ");
                Console.WriteLine("  Ndhyyyyyyyyyyyyyyyyyyyyyyyyyyyyhm      ");
                Console.WriteLine("    NdhyyyyyyyyyyyyyyyyyyyyyyyyyyyyhmN   ");
                Console.WriteLine("      NmmmmmmmmmmmmmmmmmmmmmdhhyyyyhhhN  ");
                Console.WriteLine("                               mhhhhhhhN ");
                Console.WriteLine("       Nmdddddddddhhddddddddm   mhhhhhhdN");
                Console.WriteLine("       mhhhhhhhhhhhhhhhhhhhhhdN  dhhhhhdN");
                Console.WriteLine("      NhhhhhhhhhhhhhhhhhhhhhhdN  dhhhhhdN");
                Console.WriteLine("      dhhhhhdddmmmmmmmmmmddmN   Ndhhhhhm ");
                Console.WriteLine("     mhhhhhhd                 NmdhhhhhdN ");
                Console.WriteLine("    NdhhhhhdN  NddddddddddddmddhhhhhhdN  ");
                Console.WriteLine("    NdhhhhhddNNdddddddddddddddddddddm    ");
                Console.WriteLine("      mddddddddddddddddddddddddddmN      ");
                Console.WriteLine("       NmdddddddddddddddddddmmNN         ");
                Console.WriteLine("         Nddddddddddm                    ");
                Console.WriteLine("           NmdddddddN                    ");
                Console.WriteLine("              mddddN                     ");
                Console.WriteLine("               NmmN                      ");
                Console.WriteLine("                                         ");
                Console.WriteLine("            PARADOX CRIMELIFE            ");
                Console.WriteLine("              IO & COPONTE               ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
                Console.WriteLine("                                         ");
            });
        }
    }
}
