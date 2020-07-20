using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces.AdministrationController
{
    public interface IBooking
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
        public bool active { get; set; }
        public ILocation location { get; set; }
        public UserType priority { get; set; }

    }
}
