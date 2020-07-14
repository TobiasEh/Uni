using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;

namespace UnitTests.Simulation
{
    [TestFixture]
    class ScenarioTest
    {
        private static Location location = new Location()
        {
            emergency = 3000,
            name = "Ludwigsburg",

        };

        private static int bookingCountPerDay = 100;
        private static int duration = 40;
        private static List<Rushhour> rushhours = new List<Rushhour>();
        private static DateTime start = DateTime.Now.AddDays(3).AddHours(5);
        private static List<Vehicle> vehicles = new List<Vehicle>() {
                    new Vehicle()
                    {
                        capacity = 1000,
                        model = "model",
                        plugs = new Plug()
                        {
                            power = 100,
                            type = PlugType.CCS
                        },
                    socStart = 20,
                    socEnd = 40,
                    },
        };

        public static Vehicle vehicle2 = new Vehicle()
        {
            capacity = 1000,
            model = "model2",
            plugs = new Plug()
            {
                power = 100,
                type = PlugType.CCS
            },
            socStart = 20,
            socEnd = 40,
        };

        public static Rushhour rushhour1 = new Rushhour()
        {
            bookings = 20,
            start = DateTime.Now.AddDays(4),
            end = DateTime.Now.AddDays(5),
            strategy = new NormalDistribution(),
        };

        [Test]
        public void scenarioCreateValidTest()
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = 100,
                duration = 40,
                location = location,
                rushhours = rushhours,
                start = DateTime.Now.AddDays(2).AddHours(4),
                vehicles = vehicles,
            };
            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void scenarioCreateVehiclesInValid()
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = new List<Vehicle>(),
            };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var mes = validationResults[0];
            Assert.AreEqual("vehicles", mes.MemberNames.ElementAt(0));
        }
        //NotNullTests
        [TestCase(null, 40, "bookingCountPerDay")]
        [TestCase(100, null, "duration")]
        [TestCase(0, 40, "bookingCountPerDay")]
        [TestCase(100, -3, "duration")]
        public void scenarioCreateNullInValid(int _bookingCountPerDay, int _duration, string _excepted)
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = _bookingCountPerDay,
                duration = _duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = vehicles,
            };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var mes = validationResults[0];
            Assert.AreEqual(_excepted, mes.MemberNames.ElementAt(0));
        }

        [Test]
        public void scenarioCreateLocationNull()
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = null,
                rushhours = rushhours,
                start = start,
                vehicles = vehicles,
            };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var mes = validationResults[0];
            Assert.AreEqual("location", mes.MemberNames.ElementAt(0));
        }

        [Test]
        public void scenarioRushhoursNull()
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = null,
                start = start,
                vehicles = vehicles,
            };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var mes = validationResults[0];
            Assert.AreEqual("rushhours", mes.MemberNames.ElementAt(0));
        }

        [Test]
        public void scenarioVehicleNull()
        {
            Scenario scenario = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = null,
            };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(scenario, new ValidationContext(scenario), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var mes = validationResults[0];
            Assert.AreEqual("vehicles", mes.MemberNames.ElementAt(0));
        }

        [Test]
        public void scenarioMethodsValid()
        {
            Scenario scenario2 = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = vehicles,
            };

            Assert.IsTrue(scenario2.addVehicle(vehicle2));
            Assert.IsTrue(scenario2.addRushhour(rushhour1));
            Assert.IsTrue(scenario2.deleteVehicle(vehicle2));
            Assert.IsTrue(scenario2.deleteRushhour(rushhour1));
        }

        [Test]
        public void scenarioMethodsVehicleInValid()
        {
            Scenario scenario2 = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = vehicles,
            };
            Assert.IsFalse(scenario2.addVehicle(null));
            scenario2.addVehicle(vehicle2);
            scenario2.deleteVehicle(vehicle2);
            Assert.IsFalse(scenario2.deleteVehicle(vehicle2));
            Assert.IsFalse(scenario2.deleteVehicle(null));
        }

        [Test]
        public void scenarioMethodsRushhourInValid()
        {
            Scenario scenario2 = new Scenario()
            {
                bookingCountPerDay = bookingCountPerDay,
                duration = duration,
                location = location,
                rushhours = rushhours,
                start = start,
                vehicles = vehicles,
            };

            Assert.IsFalse(scenario2.addRushhour(null));
            Assert.IsFalse(scenario2.deleteRushhour(null));
            scenario2.addRushhour(rushhour1);
            Assert.IsFalse(scenario2.addRushhour(rushhour1));
            scenario2.deleteRushhour(rushhour1);
            Assert.IsFalse(scenario2.deleteRushhour(rushhour1));
        }
    }

}
