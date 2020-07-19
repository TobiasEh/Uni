using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using Sopro.Interfaces;
using Sopro.Interfaces.AdministrationController;

namespace Sopro.Models.Administration
{
    public class Booking : IBooking
    {
        public string id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int capacity { get; set; }

        [Required]
        [BookingPlugsValidation]
        public List<PlugType> plugs { get; set; }

        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }

        [Required]
        [BookingSocEndValidation]
        public int socEnd { get; set; }

        [Required]
        public string user { get; set; }

        [Required]
        [BookingStartTimeValidation]
        public DateTime startTime { get; set; }

        [Required]
        [BookingEndTimeValidation]
        public DateTime endTime { get; set; }
        public Station station { get; set; }

        [Required]
        public bool active { get; set; } = false;

        [Required]
        public ILocation location { get; set; }
        public UserType priority { get; set; }

        public Booking()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}
