using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Booking
    {
        public double current_Charge { get; set; }
        public int required_Distance { get; set; }
        public DateTime start_Time { get; set; }
        public DateTime end_Time { get; set; }
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
