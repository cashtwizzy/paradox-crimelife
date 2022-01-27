using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Garage
{
    public class GarageData
    {
        public int GarageId { get; set; }
        public List<GarageVehicle> Vehicles { get; set; }
        public GarageVehicle CurrentVehicle { get; set; }

        public GarageData(int garageId, List<GarageVehicle> vehicles, GarageVehicle currentVehicle)
        {
            GarageId = garageId;
            Vehicles = vehicles;
            CurrentVehicle = currentVehicle;
        }
    }
}
