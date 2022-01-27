using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Garage
{
    public class GarageVehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plate { get; set; }
        public string RegDate { get; set; }
        public int Owner { get; set; }
        public float TrunkWeight { get; set; }
        public int MaxTrunkWeight { get; set; }
        public Tuning Tuning { get; set; }

        public GarageVehicle(int id, string name, string plate, string regdate, int owner, float tw, int maxtw, Tuning tuning)
        {
            Id = id;
            Name = name;
            Plate = plate;
            RegDate = regdate;
            Owner = owner;
            TrunkWeight = (float)Math.Round(tw, 2, MidpointRounding.AwayFromZero);
            MaxTrunkWeight = maxtw;
            Tuning = tuning;
        }
    }
}
