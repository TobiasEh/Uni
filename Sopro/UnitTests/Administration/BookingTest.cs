using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTests
{
    [TestFixture]
    public class BookingTest
    {
        [Test]
        public void BookingCreateValid()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 10,
                socEnd = 100,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);

        }

        [Test]
        public void BookingCreateInvalidCapacity()
        {
            Booking b = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 10,
                socEnd = 100,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
            };
        }
    }
}