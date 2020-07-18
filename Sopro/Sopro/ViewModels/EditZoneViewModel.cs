using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EditZoneViewModel
    {
        public string name { get; set; }
        public int id { get; set; }
        public Zone zone { get; set; }
    }
}
