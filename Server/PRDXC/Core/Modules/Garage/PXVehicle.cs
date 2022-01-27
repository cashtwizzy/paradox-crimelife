using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Garage
{
    public class PXVehicle
    {
        public static List<PXVehicle> Vehicles = new List<PXVehicle>();
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plate { get; set; }
        public string RegDate { get; set; }
        public int Owner { get; set; }
        public Inventory.Inventory Trunk { get; set; }
        public Tuning Tuning { get; set; }
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public int Headlights { get; set; }
        public bool Locked { get; set; }
        public bool TrunkLocked { get; set; } 
        public bool Engine { get; set; }
        public Vehicle MPVehicle { get; set; }

        public PXVehicle(int id, string name, string plate, string regdate, int owner, Inventory.Inventory trunk, Tuning tuning, int color1, int color2, int lights)
        {
            Id = id;
            Name = name;
            Plate = plate;
            RegDate = regdate;
            Owner = owner;
            Trunk = trunk;
            Tuning = tuning;
            PrimaryColor = color1;
            SecondaryColor = color2;
            Headlights = lights;
            MPVehicle = null;
            Locked = true;
            TrunkLocked = true;
            Engine = false;
            Vehicles.Add(this);
        }

        public GarageVehicle ToGarageVehicle()
        {
            return new GarageVehicle(Id, Name, Plate, RegDate, Owner, Trunk.GetInventoryWeight(), Trunk.MaxKG, Tuning);
        }

        public void ApplyVehicleTo(Vehicle veh)
        {
            MPVehicle = veh;
            veh.SetData("PXVehicle", this);
            MPVehicle.SetSharedData("LockState", Locked);
            MPVehicle.SetSharedData("EngineState", Engine);
            MPVehicle.EngineStatus = false;
            MPVehicle.Locked = true;
            SetTuning();
        }

        public void SetTuning()
        {
            MPVehicle.PrimaryColor = PrimaryColor;
            MPVehicle.SecondaryColor = SecondaryColor;
            MPVehicle.Neons = false;
        }
    }
}
