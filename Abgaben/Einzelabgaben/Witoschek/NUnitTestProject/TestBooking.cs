using Blatt03.Models;
using Blatt03.ViewModel.CustomValidation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace NUnitTestProject
{
    [TestFixture]
    class TestBooking
    {
        [Test]
        public void testAddBooking()
        {
            var range = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var dateValidation = new DateAttribute();

            // Alle Attribute richtig
            Booking b1 = new Booking { currentCharge = 30, 
                requiredDistance = 150, 
                connectorType = ConnectorType.CCSCombo2Plug, 
                start = new DateTime(2020, 7, 8, 8, 0, 0), 
                end = new DateTime(2020, 7, 8, 10, 0, 0) };

            Assert.IsTrue(range.IsValid(b1.currentCharge));
            Assert.IsTrue(b1.requiredDistance > 0 && b1.requiredDistance <= 1000);
            Assert.IsTrue(dateValidation.IsValid(b1.start));
            Assert.IsTrue(dateValidation.IsValid(b1.end));
            Assert.IsTrue(b1.end > b1.start);

            // currentCharge ungültig
            Booking b2 = new Booking
            {
                currentCharge = -30,
                requiredDistance = 150,
                connectorType = ConnectorType.CCSCombo2Plug,
                start = new DateTime(2020, 7, 8, 8, 0, 0),
                end = new DateTime(2020, 7, 8, 10, 0, 0)
            };

            Assert.IsFalse(range.IsValid(b2.currentCharge));
            Assert.IsTrue(b2.requiredDistance > 0 && b2.requiredDistance <= 1000);
            Assert.IsTrue(dateValidation.IsValid(b2.start));
            Assert.IsTrue(dateValidation.IsValid(b2.end));
            Assert.IsTrue(b2.end > b2.start);

            // requiredDistance ungültig
            Booking b3 = new Booking
            {
                currentCharge = 30,
                requiredDistance = -150,
                connectorType = ConnectorType.CCSCombo2Plug,
                start = new DateTime(2020, 7, 8, 8, 0, 0),
                end = new DateTime(2020, 7, 8, 10, 0, 0)
            };

            Assert.IsTrue(range.IsValid(b3.currentCharge));
            Assert.IsFalse(b3.requiredDistance > 0 && b3.requiredDistance <= 1000);
            Assert.IsTrue(dateValidation.IsValid(b3.start));
            Assert.IsTrue(dateValidation.IsValid(b3.end));
            Assert.IsTrue(b3.end > b3.start);

            // start ungültig
            Booking b4 = new Booking
            {
                currentCharge = 30,
                requiredDistance = 150,
                connectorType = ConnectorType.CCSCombo2Plug,
                start = new DateTime(2020, 5, 5, 8, 0, 0),
                end = new DateTime(2020, 7, 8, 10, 0, 0)
            };

            Assert.IsTrue(range.IsValid(b4.currentCharge));
            Assert.IsTrue(b4.requiredDistance > 0 && b4.requiredDistance <= 1000);
            Assert.IsFalse(dateValidation.IsValid(b4.start));
            Assert.IsTrue(dateValidation.IsValid(b4.end));
            Assert.IsTrue(b4.end > b4.start);

            // end ungültig
            Booking b5 = new Booking
            {
                currentCharge = 30,
                requiredDistance = 150,
                connectorType = ConnectorType.CCSCombo2Plug,
                start = new DateTime(2020, 7, 8, 8, 0, 0),
                end = new DateTime(2020, 7, 7, 10, 0, 0)
            };

            Assert.IsTrue(range.IsValid(b5.currentCharge));
            Assert.IsTrue(b5.requiredDistance > 0 && b3.requiredDistance <= 1000);
            Assert.IsTrue(dateValidation.IsValid(b5.start));
            Assert.IsTrue(dateValidation.IsValid(b5.end));
            Assert.IsFalse (b5.end > b5.start);

        }
    }
}
