using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EditZoneViewModel
    {
        
        public string name { get; set; }
       
        public string id { get; set; }
        
        public char site { get; set; }
       
        public Zone zone { get; set; }
       
        public Station station { get; set; }
        [Required]
        [AtleastOnePlugStation("ccs","type2", ErrorMessage = "Mindestens ein Plug ist erforderlich.")]
        public int ccs { get; set; }
        [Required]
        [AtleastOnePlugStation("ccs","type2", ErrorMessage = "Mindestens ein Plug ist erforderlich.")]
        public int type2 { get; set; }
        [Required]
        [PowerNotZero("ccs", ErrorMessage = "Die Leistung sollte größer 0 sein.")]
        public int ccsPower { get; set; }
        [Required]
        [PowerNotZero("type2", ErrorMessage = "Die Leistung sollte größer 0 sein.")]
        public int type2Power { get; set; }
    }
}
