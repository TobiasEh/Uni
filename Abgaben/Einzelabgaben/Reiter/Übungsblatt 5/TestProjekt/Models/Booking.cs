using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace TestProjekt.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public double currentCharge { get; set; }

        [Range(1, 1000)]
        [Required]
        public int requiredDistance { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime start { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime end { get; set; }

        public ConnectorType connectorType { get; set; }
    }

    public enum ConnectorType
    {
        [Description("Schuko-Socket")]
        SchukoSocket = 0,
        [Description("Type 1-Plug")]
        Type1Plug,
        [Description("Type 2-Plug")]
        Type2Plug,
        [Description("CHAdeMO-Plug")]
        CHAdeMOPlug,
        [Description("Tesla Supercharger")]
        TeslaSupercharger,
        [Description("CCS Combo 2-Plug")]
        CCSCombo2Plug
    }
}
