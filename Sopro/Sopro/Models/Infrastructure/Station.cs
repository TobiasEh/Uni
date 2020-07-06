using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Station
    {
        public string id { get; set; }
        [ListMinLength(1)]
        public List<Plug> plugs { get; set; }
        public string manufacturer { get; set; }
        public int maxPower { get; set; }
        public int maxParallelUsable { get; set; }

        public bool addPlug(Plug plug)
        {
            plugs.Add(plug);
            if (plugs.Contains(plug))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deletePlug(Plug plug)
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
