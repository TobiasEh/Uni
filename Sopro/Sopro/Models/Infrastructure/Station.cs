using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Station
    {
        private String id { get; set; }
        private List<Plug> plugs { get; set; }
        private String manufacturer { get; set; }
        private String name { get; set; }
        private int maxPowerStation { get; set; }

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
