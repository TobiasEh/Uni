using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models;
using Sopro.Models.Administration;

namespace Sopro.ViewModels
{
    public class DashboardViewModel
    {
        private List<Booking> scheduledBookings { get; set; }
        private List<Booking> unscheduledBookings { get; set; }
        public DashboardViewModel(List<Booking> _scheduledBookings, List<Booking> _unscheduledBookings)
        {
            scheduledBookings = _scheduledBookings;
            unscheduledBookings = _unscheduledBookings;
        }
    }
}
