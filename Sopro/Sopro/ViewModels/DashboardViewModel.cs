using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models;

namespace Sopro.ViewModels
{
    public class DashboardViewModel
    {
        private List<IBooking> activeBookings { get; set; }
        private List<IBooking> scheduledBookings { get; set; }
    }
}
