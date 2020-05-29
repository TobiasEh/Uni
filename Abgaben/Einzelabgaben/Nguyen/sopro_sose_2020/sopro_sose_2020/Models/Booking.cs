using Foolproof;
using sopro_sose_2020.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro_sose_2020.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public int cur_charge { get; set; }

        [Range(0, 1000)]
        [Required]
        public int needed_distance { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        [DateValidation]
        public DateTime startTime { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        [GreaterThan("startTime")]
        public DateTime endTime { get; set; }
        [Required]
        public ConnectorType connectorType { get; set; }

    }
    public enum ConnectorType
    {
        type_a,
        type_b,
        type_c
    }
}
