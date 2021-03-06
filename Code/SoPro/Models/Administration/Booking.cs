﻿using Sopro.ValidationAttributes;
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
        public List<PlugType> plugs { get; set; } = new List<PlugType>() { PlugType.CCS};

        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }

        [Required]
        [BookingSocEndValidation]
        public int socEnd { get; set; }

        [Required]
        public string user { get; set; } = "examplemail.com";

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
        public ILocation location { get; set; } = new Location();
        public UserType priority { get; set; } = UserType.EMPLOYEE;

        /// <summary>
        /// Konstruktor der Klasse der Buchungen.
        /// </summary>
        public Booking()
        {
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Erzeugt eine Tiefe Kopie einer Buchung.
        /// </summary>
        /// <returns>Tiefe Kopie einer Buchung.</returns>
        public Booking deepCopy()
        {
            Booking copy = new Booking()
            {
                id = id,
                capacity = capacity,
                plugs = plugs,
                socStart = socStart,
                socEnd = socEnd,
                user = user,
                startTime = startTime,
                endTime = endTime,
                active = active,
                location = location,
                station = station,
                priority = priority
            };

            return copy;
        }
    }
}
