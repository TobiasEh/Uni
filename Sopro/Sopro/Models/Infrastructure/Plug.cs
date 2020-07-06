using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Plug
    {
        private String id { get; set; }
        private int power { get; set; }
        private PlugType plugType { get; set; }
    }
    public enum PlugType
    {
        TYPE2,
        CCS
    }
}
