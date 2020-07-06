using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Zone
    {
        private string id { get; set; }
        private List<Station> stations { get; set; }
        private char site { get; set; }
        private int maxPowerZone { get; set; }


        public bool addStation(Station station)
        {
            if (station != null)
            {
                stations.Add(station);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteZone(Station station)
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
