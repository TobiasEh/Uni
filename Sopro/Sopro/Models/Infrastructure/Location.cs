using Sopro.CustomValidationAttributes;
using sopro2020_abgabe.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Infrastructure
{
    public class Location : ILocation
    {
        public String id { get; set; }
        [ListMinLength(0)]
        public List<Zone> zones { get; set; }
        [Required]
        public string name { get; set; }
        [Range(0, int.MaxValue)]
        public double emergency { get; set; }
        public Schedule schedule { get; set; }
        public Distributer distributer { get; set; }

        public Location()
        {
            schedule = new Schedule();
            distributer = new Distributer(schedule,this);
           
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
