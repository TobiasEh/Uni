using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Models
{
    public class Plug
    {
        [Range(0, int.MaxValue)]
        public int power { get; set; }
        public PlugType type { get; set; }
    }
   
}
