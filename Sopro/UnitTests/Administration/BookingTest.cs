﻿using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class BookingTest
    {
        static Plug p1 = new Plug
        {
            id = "abc",
            power = 20,
            type = PlugType.CCS
        };

        static Plug p2 = new Plug
        {
            id = "abcd",
            power = 40,
            type = PlugType.TYPE2
        };

        static Station s1 = new Station
        {
            id = "abc",
            plugs = new List<Plug> { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        static Zone z1 = new Zone
        {
            stations = new List<Station> { s1 },
            id = "abc",
            site = 'A',
            maxPower = 1000
        };

        static Location lTest = new Location
        {
            id = "abc",
            name = "TestLocation",
            emergency = 1,
            zones = { z1 }
        };

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
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);

        }

        [Test]
        public void BookingCreateInvalidCapacity()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = -20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 10,
                socEnd = 100,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("capacity", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidPlugs()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { },
                socStart = 10,
                socEnd = 100,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("plugs", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidSocStart()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = -5,
                socEnd = 100,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("socStart", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidSocEnd()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 10,
                socEnd = 9,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("socEnd", msg.MemberNames.ElementAt(0));

            Booking booking2 = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 10,
                socEnd = 1001,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            validationResults = new List<ValidationResult>();
            actual = Validator.TryValidateObject(booking2, new ValidationContext(booking2), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            msg = validationResults[0];
            Assert.AreEqual("socEnd", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidUser()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 5,
                socEnd = 20,
                user = "",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("user", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidStartTime()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 5,
                socEnd = 20,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(-1),
                endTime = DateTime.Now.AddDays(1).AddHours(3),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("startTime", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidEndTime()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 5,
                socEnd = 20,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(-1),
                location = lTest
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("endTime", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void BookingCreateInvalidLocation()
        {
            Booking booking = new Booking
            {
                id = "abc",
                capacity = 20,
                plugs = new List<PlugType>() { PlugType.CCS },
                socStart = 5,
                socEnd = 20,
                user = "example@email.de",
                startTime = DateTime.Now.AddDays(1).AddHours(1),
                endTime = DateTime.Now.AddDays(1).AddHours(-1),
                location = null
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(booking, new ValidationContext(booking), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("location", msg.MemberNames.ElementAt(0));
        }
    }
}