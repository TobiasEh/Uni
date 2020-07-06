using NUnit.Framework;
using sopro2020_abgabe.Models;
using System;
using System.Data;
using Microsoft.AspNetCore.Mvc.Core;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class PlugTest
    {
        [Test]
        public void testPlugCreateValid()
        {
            Plug plug = new Plug
            {
                id = "abc",
                power = 20,
                type = PlugType.CCS
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(plug, new ValidationContext(plug), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void testPlugCreateInvalidPower()
        {
            Plug plug = new Plug
            {
                id = "abc",
                power = -20,
                type = PlugType.CCS
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(plug, new ValidationContext(plug), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("power", msg.MemberNames.ElementAt(0));
        }
    }
}