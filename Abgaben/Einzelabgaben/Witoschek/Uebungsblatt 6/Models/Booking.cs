using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using Blatt03.ViewModel.CustomValidation;

namespace Blatt03.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public double currentCharge { get; set; }
        [Range(0, 1000)]
        [Required]
        public int requiredDistance { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime start { get; set; }
        [DataType(DataType.DateTime)]
        [DateAttribute()]
        [Required]
        public DateTime end { get; set; }
        public ConnectorType connectorType { get; set; }
      
    }

    public enum ConnectorType
    {
        [Description("Schuko-Socket (AC)")]
        SchukoSocket = 0,
        [Description("Type 1-Plug (AC)")]
        Type1Plug,
        [Description("Type 2-Plug (AC)")]
        Type2Plug,
        [Description("CHAdeMO-Plug (DC)")]
        CHAdeMOPlug,
        [Description("Tesla Supercharger (DC)")]
        TeslaSupercharger,
        [Description("CCS Combo 2-Plug (DC)")]
        CCSCombo2Plug
    }

}
