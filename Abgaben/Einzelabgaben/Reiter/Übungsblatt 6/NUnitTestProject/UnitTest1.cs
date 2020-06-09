using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using TestProjekt.Models;
using TestProjekt.Validation;

namespace NUnitTestProject
{
    public class Tests
    {
        private List<Booking> bookings = new List<Booking>();

        [Test]
        public void TestAddBooking()
        {
            var range = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var dateValidation = new DateValidation();

            Booking b1 = new Booking { currentCharge = 12, requiredDistance = 100, connectorType = ConnectorType.Type2Plug, start = new DateTime(2020, 12, 12, 12, 0, 0), end = new DateTime(2020, 12, 12, 14, 0, 0) };
            foreach(Booking b in bookings)
            {
                Assert.IsTrue(range.IsValid(b.currentCharge));
                Assert.IsTrue(dateValidation.IsValid(b.start));
                Assert.IsTrue(dateValidation.IsValid(b.end));
            }
        }
    }
}