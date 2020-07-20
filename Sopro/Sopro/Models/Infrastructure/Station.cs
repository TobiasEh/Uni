using Sopro.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    public class Station
    {
        [ListMinLength(1)]
        public List<Plug> plugs { get; set; } = new List<Plug>() { new Plug() { power = 0, type = PlugType.CCS} };
        public string manufacturer { get; set; }
        [Range(0, int.MaxValue)]
        public int maxPower { get; set; }
        [Range(0, int.MaxValue)]
        public int maxParallelUseable { get; set; }
        public int id { get; set; }

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
