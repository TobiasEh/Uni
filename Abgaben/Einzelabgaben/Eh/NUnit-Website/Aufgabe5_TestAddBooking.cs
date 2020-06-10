
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Website.Models;

namespace NUnitTestProject
{
    [TestFixture]
    class TestBooking
    {
        [Test]
        public void testAddBooking()
        {
            var rangeCharge = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var rangeDistance = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 1000);


            Booking booking = new Booking
            {
                currentCharge = 10,
                requiredDistance = 100,
                plugType = Plug.CCSCombo2Plug,
                start = new DateTime(2020, 9, 7, 13, 0, 0),
                end = new DateTime(2020, 9, 7, 20, 0, 0)
            };

            Assert.IsTrue(rangeCharge.IsValid(booking.currentCharge));
            Assert.IsTrue(rangeDistance.IsValid(booking.requiredDistance));
        }

        [Test]
        public void testAddBookingWrongCharge()
        {
            var rangeCharge = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var rangeDistance = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 1000);


            Booking booking = new Booking
            {
                currentCharge = 110,
                requiredDistance = 100,
                plugType = Plug.CCSCombo2Plug,
                start = new DateTime(2020, 9, 7, 13, 0, 0),
                end = new DateTime(2020, 9, 7, 20, 0, 0)
            };

            Assert.IsFalse(rangeCharge.IsValid(booking.currentCharge));
            Assert.IsTrue(rangeDistance.IsValid(booking.requiredDistance));
        }

        [Test]
        public void testAddBookingWrongDistance()
        {
            var rangeCharge = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var rangeDistance = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 1000);


            Booking booking = new Booking
            {
                currentCharge = 10,
                requiredDistance = 1500,
                plugType = Plug.CCSCombo2Plug,
                start = new DateTime(2020, 9, 7, 13, 0, 0),
                end = new DateTime(2020, 9, 7, 20, 0, 0)
            };

            Assert.IsTrue(rangeCharge.IsValid(booking.currentCharge));
            Assert.IsFalse(rangeDistance.IsValid(booking.requiredDistance));
        }


    }
}