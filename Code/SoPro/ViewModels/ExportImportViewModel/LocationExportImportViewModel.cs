using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class LocationExportImportViewModel
    {
        public string id { get; set; }
        public List<Zone> zones { get; set; }
        public string name { get; set; }
        public double emergency { get; set; }
        public DateTime normalizedDistributionTime { get; set; }

        public LocationExportImportViewModel() { }

        public LocationExportImportViewModel(ILocation l) 
        {
            id = l.id;
            zones = l.zones;
            name = l.name;
            emergency = l.emergency;
            normalizedDistributionTime = l.normalizedDistributionTime;
        }

        public ILocation generateLocation()
        {
            ILocation l= new Location();
            l.id = id;
            l.zones = zones;
            l.name = name;
            l.emergency = emergency;
            l.normalizedDistributionTime = normalizedDistributionTime;

            return l;
        }
    }
}
