using Blatt03.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UITests
{
    public class Tests
    {
        public List<Booking> bookings = new List<Booking>();

        [Test]
        public void TestCreateBooking()
        {
            bookings.Clear();
            var range = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var dataVal = new DataAttribute();

            bookings.Add(new Booking { currentCharge = 20, requiredDistance = 100, connectorType = ConnectorType.Type2Plug, start = new DateTime(2020, 10, 10, 10, 0, 0), end = new DateTime(2020, 10, 10, 12, 0, 0) });
            bookings.Add(new Booking { currentCharge = 90, requiredDistance = 1000, connectorType = ConnectorType.TeslaSupercharger, start = new DateTime(2020, 12, 5, 11, 0, 0), end = new DateTime(2020, 12, 5, 13, 45, 0) });
            foreach (Booking b in bookings)
            {
                var resultR = range.IsValid(b.chargeStatus);
                Assert.IsTrue(resultR);
                var resultDataV1 = dataVal.IsValid(b.startTime);
                Assert.IsTrue(resultDataV1);
                var resultDataV2 = dataVal.IsValid(b.endTime);
                Assert.IsTrue(resultDataV2);
                var dateNon = new DateNonNegativeAttribute(b.startTime);
                var resultDateNon = dateNon.IsValid(b.endTime);
                Assert.IsTrue(resultDateNon);
            };
        }

        [Test]
        public void BadTestCreateBooking()
        {
            bookings.Clear();
            var range = new System.ComponentModel.DataAnnotations.RangeAttribute(0, 100);
            var dataVal = new DataValidationAttribute();

            bookings.Add(new Booking { chargeStatus = 120, distance = 0, connectorType = ConnectorType.Type2_Plug, startTime = new DateTime(2020, 3, 3, 10, 0, 0), endTime = new DateTime(2020, 3, 2, 12, 0, 0) });
            bookings.Add(new Booking { chargeStatus = -1, distance = 1020, connectorType = ConnectorType.Tesla_Supercharger, startTime = new DateTime(2020, 1, 2, 11, 0, 0), endTime = new DateTime(2020, 1, 2, 10, 45, 0) });
            foreach (Booking item in bookings)
            {
                var resultR = range.IsValid(item.chargeStatus);
                Assert.IsFalse(resultR, range.ErrorMessage);
                var resultDataV1 = dataVal.IsValid(item.startTime);
                Assert.IsFalse(resultDataV1, dataVal.ErrorMessage);
                var resultDataV2 = dataVal.IsValid(item.endTime);
                Assert.IsFalse(resultDataV2, dataVal.ErrorMessage);
                var dateNon = new DateNonNegativeAttribute(item.startTime);
                var resultDateNon = dateNon.IsValid(item.endTime);
                Assert.IsFalse(resultDateNon, dateNon.ErrorMessage);
            };
        }
    }
}