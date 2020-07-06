using sopro2020_abgabe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Location : ILocation
    {
        private String id { get; set; }
        private List<Zone> zones { get; set; }
        private String name { get; set; }
        private double emergencyReserve { get; set; }

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
