using System;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Infrastructure
{
    public class Plug
    {
        [Range(0, int.MaxValue)]
        public int power { get; set; }
        public PlugType type { get; set; }
    }
   
}
