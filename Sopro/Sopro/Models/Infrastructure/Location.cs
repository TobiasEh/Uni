using Sopro.CustomValidationAttributes;
using sopro2020_abgabe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Location : ILocation
    {
        public String id { get; set; }
        [ListMinLength(0)]
        public List<Zone> zones { get; set; }
        public string name { get; set; }
        public double emergency { get; set; }

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
