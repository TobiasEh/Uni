using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Infrastructure
{
    public class Plug
    {
        public string id { get; set; }
        [Range(0, int.MaxValue)]
        public int power { get; set; }
        public PlugType type { get; set; }
    }
   
}
