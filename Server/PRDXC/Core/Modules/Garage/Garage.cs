using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace PRDXC.Core.Modules.Garage
{
    public class Garage
    {
        public static List<Garage> Garages = new List<Garage>();
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 GaragePoint { get; set; }
        public List<Vector3> SpawnPoints { get; set; }
        public List<float> SpawnPointRotation { get; set; }

        public Garage(int id, string name, Vector3 garage, List<Vector3> spawnPoints, List<float> rotations)
        {
            Id = id;
            Name = name;
            GaragePoint = garage;
            SpawnPoints = spawnPoints;
            SpawnPointRotation = rotations;

            NAPI.Task.Run(() =>
            {
                NAPI.Blip.CreateBlip(357, GaragePoint, 1, 4, $"Garage - {Name}", shortRange: true);
                NAPI.Marker.CreateMarker(36, GaragePoint, new Vector3(), new Vector3(), 1, new Color(51, 204, 255), true);
                var colshape = NAPI.ColShape.CreateCylinderColShape(GaragePoint, 2, 8, 0);
                colshape.SetSharedData("GarageId", Id);
                colshape.SetSharedData("Type", "Garage");
                Garages.Add(this);
            });
        }
    }
}
