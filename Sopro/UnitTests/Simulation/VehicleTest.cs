using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Sopro.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using MailKit;
using System.Linq;
using Sopro.Models.Simulation;

namespace UnitTests.Simulation
{
    [TestFixture]
    class VehicleTest
    {
        static Plug plug = new Plug
        {
            power = 20,
            type = PlugType.CCS
        };

        [Test]
        public void testVehicleCreate()
        {
            Vehicle vehicle = new Vehicle
            {
                capacity = 120,
                model = "newModel",
                plugs = plug,
                socStart = 44,
                socEnd = 99,
            };
            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(vehicle, new ValidationContext(vehicle), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        //socEnd smaler than socStart
        [TestCase(20, "right", 44, 34, "socEnd")]
        //socStart negative
        [TestCase(20, "right", -2, 44, "socStart")]
        //socEnd grater than 100
        [TestCase(20, "right", 25, 102, "socEnd")]
        //capacity negative
        [TestCase(-20, "right", 20, 44, "capacity")]
        //model inlvalid
        [TestCase(20, "", 20, 40, "model")]
        public void testVehicleCreateInvalid(int _capacity, string _model, int _socStart, int _socEnd, string _expected)
        {
            Vehicle vehicle = new Vehicle
            {
                capacity = _capacity,
                model = _model,
                plugs = plug,
                socStart = _socStart,
                socEnd = _socEnd,
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(vehicle, new ValidationContext(vehicle), validationResults, true));
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual(_expected, msg.MemberNames.ElementAt(0));
        }

    }
}
