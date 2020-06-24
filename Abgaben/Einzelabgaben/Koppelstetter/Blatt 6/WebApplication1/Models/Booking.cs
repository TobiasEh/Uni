using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Validations;

namespace WebApplication1.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public double current_Charge { get; set; }
        [Range(1, 1000)]
        [Required]
        public int required_Distance { get; set; }
        [Required]
        [DateTimeVal]
        public DateTime start_Time { get; set; }
        [Required]
        [DateTimeVal]
        public DateTime end_Time { get; set; }
        
        [Required]
        public ConnectorType connectorType { get; set; }
    }
    public enum ConnectorType
    {

        Schuko_Socket,
        Type1_Plug,
        Type2_Plug,
        CHAdeMO_Plug,
        Tesla_Supercharger,
        CCS_Combo_2_Plug
    }
}
