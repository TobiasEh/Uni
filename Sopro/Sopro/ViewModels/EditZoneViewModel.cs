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
        public char site { get; set; }
        public Zone zone { get; set; }
        public Station station { get; set; }
        public int ccs { get; set; }
        public int type2 { get; set; }
        public int ccsPower { get; set; }
        public int type2Power { get; set; }
    }
}
