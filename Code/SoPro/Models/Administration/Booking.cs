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

        [Required(ErrorMessage = "Darf nicht leer sein")]
        [Range(1, int.MaxValue)]
        public int capacity { get; set; }

        [Required]
        [BookingPlugsValidation]
        public List<PlugType> plugs { get; set; } = new List<PlugType>() { PlugType.CCS};

        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }

        [Required]
        [BookingSocEndValidation(ErrorMessage ="pipapo")]
        public int socEnd { get; set; }

        [Required]
        public string user { get; set; } = "examplemail.com";

        [Required(ErrorMessage ="Darf nicht leer sein")]
        [BookingStartTimeValidation(ErrorMessage ="Startzeitpunkt kann nicht in der Vergangenheit liegen")]
        public DateTime startTime { get; set; }

        [Required(ErrorMessage = "Darf nicht leer sein")]
        [BookingEndTimeValidation(ErrorMessage ="Endzeitpunkt muss nach start stattfinden")]
        public DateTime endTime { get; set; }

        public Station station { get; set; }

        [Required]
        public bool active { get; set; } = false;

        [Required(ErrorMessage = "Darf nicht leer sein")]
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
