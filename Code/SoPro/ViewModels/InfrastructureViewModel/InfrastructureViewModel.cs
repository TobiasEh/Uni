using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class InfrastructureViewModel
    {
        public List<ILocation> locations { get; set; }
        public string name { get; set; }
        [Range(0, 1, ErrorMessage = "Nur Zahlen zwischen 0 bis 1")]
        public string emergency { get; set; }
        public string id { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Bitte eine korrekte Zeit eingeben")]
        public DateTime distributionTime { get; set; }
    }
}
