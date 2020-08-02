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
        [Range(1, int.MaxValue, ErrorMessage = "Positive Ganze Zahl eingeben.")]
        public int capacity { get; set; }

        [Required]
        [BookingPlugsValidation(ErrorMessage = "Mindestens 1 Plug ist nötig.")]
        public List<PlugType> plugs { get; set; } = new List<PlugType>() { PlugType.CCS };

        [Required]
        [Range(0, 100, ErrorMessage = "Start SoC zwischen 0 und 100%.")]
        public int socStart { get; set; }

        [Required]
        [BookingSocEndValidation(ErrorMessage = "End SoC sollte größer sein als start SoC und zwischen 0 und 100 sein.")]
        public int socEnd { get; set; }

        [Required]
        public string user { get; set; } = "examplemail.com";

        [BookingStartTimeValidation(ErrorMessage = "Startzeit darf nicht in Vergangenheit liegen.")]
        [Required]
        public DateTime startTime { get; set; }

        [BookingEndTimeValidation("startTime",ErrorMessage = "Endzeit darf nicht in vor Startzeit sein.")]
        [Required]
        public DateTime endTime { get; set; }

        public Station station { get; set; }

        [Required]
        public bool active { get; set; } = false;

        [Required]
        public ILocation location { get; set; } = new Location();
        public UserType priority { get; set; } = UserType.EMPLOYEE;

        /// <summary>
        /// Konstruktor der Klasse der Buchungen.
        /// </summary>
        public Booking()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}
