using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Garage
{
    public class Tuning
    {
        public int Engine { get; set; }
        public int Brakes { get; set; }
        public int Transmission { get; set; }
        public int Suspension { get; set; }
        public bool Turbo { get; set; }

        public Tuning() { }
        public Tuning(int engine, int brakes, int trans, int sus, bool turbo)
        {
            Engine = engine;
            Brakes = brakes;
            Transmission = trans;
            Suspension = sus;
            Turbo = turbo;
        }
    }
}
