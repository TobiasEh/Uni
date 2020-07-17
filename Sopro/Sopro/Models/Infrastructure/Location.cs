using Sopro.CustomValidationAttributes;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    public class Location : ILocation
    {
        public string id { get; set; }
        [ListMinLength(0)]
        public List<Zone> zones { get; set; }
        [Required]
        public string name { get; set; }
        [Range(0, int.MaxValue)]
        public double emergency { get; set; }
        public Schedule schedule { get; set; }
        public Distributor distributor { get; set; }
        public DateTime normalizedDistributionTime { get; set; }

        public Location()
        {
            schedule = new Schedule();
            distributor = new Distributor(schedule, this);
           
        }

        public bool addZone(Zone zone)
        {
            if(zone != null)
            {
                zones.Add(zone);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteZone(Zone zone)
        {
            if (zones.Contains(zone))
            {
                zones.Remove(zone);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
