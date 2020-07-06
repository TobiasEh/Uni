using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Plug
    {
        public string id { get; set; }
        public int power { get; set; }
        public PlugType plugType { get; set; }
    }
    public enum PlugType
    {
        TYPE2,
        CCS
    }
}
