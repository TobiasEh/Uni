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
        public DateTime startTime { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime endTime { get; set; }

        public ConnectorType connectorType { get; set; }

    }
    public enum ConnectorType
    {
        type_a,
        type_b,
        type_c
    }
}
