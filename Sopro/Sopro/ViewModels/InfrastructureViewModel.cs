using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class InfrastructureViewModel
    {
        public List<ILocation> locations {get; set;}
        public string name { get; set; }
        public string emergency { get; set; }
        public string id { get; set; }
        public DateTime distributionTime { get; set; }
    }
}
