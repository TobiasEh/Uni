using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class VehicleExportImportViewModel
    {
        public string id { get; set; }
        public string model { get; set; }
        public int capacity { get; set; }
        public int socStart { get; set; }
        public int socEnd { get; set; }
        public List<PlugType> plugs { get; set; }

        public VehicleExportImportViewModel() { }

        public VehicleExportImportViewModel(IVehicle v) 
        {
            id = v.id;
            model = v.model;
            capacity = v.capacity;
            socStart = v.socStart;
            socEnd = v.socEnd;
            plugs = v.plugs;
        }

        public IVehicle generateVehicle()
        {
            IVehicle v = new Vehicle();
            v.id = id;
            v.model = model;
            v.capacity = capacity;
            v.socStart = socStart;
            v.socEnd = socEnd;
            v.plugs = plugs;

            return v;
        }
    }

}
