
using Sopro.Interfaces.AdministrationController;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class BookingExportImportViewModel
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

        public LocationExportImportViewModel location { get; set; } 
        public UserType priority { get; set; } 


        public BookingExportImportViewModel()
        {
            id = Guid.NewGuid().ToString();
        }

        public BookingExportImportViewModel(IBooking b)
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
            if(b.location != null)
            {
                location = new LocationExportImportViewModel( b.location);
            }
            
            priority = b.priority;
        }

        public IBooking generateBooking()
        {
            IBooking b = (IBooking) new Booking();
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
            if(location != null) { 
                b.location = location.generateLocation();
            }
            b.priority = priority;
            return b;
        }
    }
}
