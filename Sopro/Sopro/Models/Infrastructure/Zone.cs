using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Zone
    {
        public string id { get; set; }
        [ListMinLength(1)]
        public List<Station> stations { get; set; }
        public char site { get; set; }
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
    }
}
