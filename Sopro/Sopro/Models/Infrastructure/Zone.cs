using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Infrastructure
{
    public class Zone
    {
        [ListMinLength(1)]
        public List<Station> stations { get; set; }
        public char site { get; set; }
        [Range(0,int.MaxValue)]
        public int maxPower { get; set; }


        public bool addStation(Station station)
        {
            stations.Add(station);
            if (stations.Contains(station))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteStation(Station station)
        {
            if (stations.Contains(station))
            {
                stations.Remove(station);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool compareTo(Zone zone)
        {
            return zone.site == this.site;
        }
    }
}
