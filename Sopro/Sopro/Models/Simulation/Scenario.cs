using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Simulation
{
    public class Scenario
    {
        [Required]
        [Range(1,100)]
        public int duration { get; set; }
        [Required]
        [Range(1, 100)]
        public int bookingCount { get; set; }
        [Required]
        public List<Vehicle> vehicles { get; set; }
        [Required]
        public List<Rushhour> rushours { get; set; }

        public bool addVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            return true;
        }
        
    }
}
