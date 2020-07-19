
using Sopro.Interfaces.AdministrationController;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class BookingExportInportViewModel
    {
        public string id { get; set; }

        public int capacity { get; set; }

        public List<PlugType> plugs { get; set; }

        public int socStart { get; set; }

        public int socEnd { get; set; }

        public string user { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public Station station { get; set; }

        public bool active { get; set; } = false;

        public Location location { get; set; } 
        public UserType priority { get; set; } 


        public BookingExportInportViewModel()
        {
            id = Guid.NewGuid().ToString();
        }

        public BookingExportInportViewModel(IBooking b)
        {
            id = b.id;
            capacity = b.capacity;
            plugs = b.plugs;
            socStart = b.socStart;
            socEnd = b.socEnd;
            user = b.user;
            startTime = b.startTime;
            endTime = b.endTime;
            station = b.station;
            active = b.active;
            location = (Location) b.location;
            priority = b.priority;
        }

        public IBooking generateBooking()
        {
            IBooking b = (IBooking) new BookingCreateViewModel();
            b.id = id;
            b.capacity = capacity;
            b.plugs = plugs;
            b.socStart = socStart;
            b.socEnd = socEnd;
            b.user = user;
            b.startTime = startTime;
            b.endTime = endTime;
            b.station = station;
            b.active = active;
            b.location = location;
            b.priority = priority;
            return b;
        }
    }
}
