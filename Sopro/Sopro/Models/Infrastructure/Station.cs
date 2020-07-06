using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Station
    {
        public string id { get; set; }
        public List<Plug> plugs { get; set; }
        public string manufacturer { get; set; }
        public string name { get; set; }
        public int maxPowerStation { get; set; }

        public bool addStation(Plug plug)
        {
            if (plug != null)
            {
                plugs.Add(plug);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteZone(Plug plug)
        {
            if (plugs.Contains(plug))
            {
                plugs.Remove(plug);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
