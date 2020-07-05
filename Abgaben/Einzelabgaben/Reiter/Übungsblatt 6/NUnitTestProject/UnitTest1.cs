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
        [Test]
        public void TestBooking()
        {
            DateValidation dv = new DateValidation();

            TestDataSet good = new TestDataSet
            {
                charge = 12,
                dist = 120,
                start = "2020-06-17 12:00:00",
                end = "2020-06-17 14:00:00",
                connect = "Type1Plug"
            };
            
            Assert.IsTrue(good.charge >= 0 && good.charge <=100);
            Assert.IsTrue(good.dist > 0 && good.dist <= 1000);
            Assert.IsTrue(dv.IsValid(good.start));
            Assert.IsTrue(dv.IsValid(good.end));
            DateTime start = DateTime.Parse(good.start);
            DateTime end = DateTime.Parse(good.end);
            Assert.IsTrue(start < end);

            TestDataSet wrongInput = new TestDataSet
            {
                charge = 1200,
                dist = 120,
                start = "2020-06-17 12:00:00",
                end = "2020-06-17 14:00:00",
                connect = "Type1Plug"
            };

            Assert.IsFalse(wrongInput.charge >= 0 && wrongInput.charge <= 100);
            Assert.IsTrue(wrongInput.dist > 0 && wrongInput.dist <= 1000);
            Assert.IsTrue(dv.IsValid(wrongInput.start));
            Assert.IsTrue(dv.IsValid(wrongInput.end));
            start = DateTime.Parse(wrongInput.start);
            end = DateTime.Parse(wrongInput.end);
            Assert.IsTrue(start < end);

            TestDataSet startBeforeEnd = new TestDataSet
            {
                charge = 12,
                dist = 120,
                start = "2020-06-17 14:00:00",
                end = "2020-06-17 12:00:00",
                connect = "Type1Plug"
            };

            Assert.IsTrue(startBeforeEnd.charge >= 0 && startBeforeEnd.charge <= 100);
            Assert.IsTrue(startBeforeEnd.dist > 0 && startBeforeEnd.dist <= 1000);
            Assert.IsTrue(dv.IsValid(startBeforeEnd.start));
            Assert.IsTrue(dv.IsValid(startBeforeEnd.end));
            start = DateTime.Parse(startBeforeEnd.start);
            end = DateTime.Parse(startBeforeEnd.end);
            Assert.IsFalse(start < end);

            TestDataSet nullInput = new TestDataSet
            {
                charge = 12,
                dist = 120,
                start = null,
                end = "2020-06-17 12:00:00",
                connect = "Type1Plug"
            };

            Assert.IsTrue(nullInput.charge >= 0 && nullInput.charge <= 100);
            Assert.IsTrue(nullInput.dist > 0 && nullInput.dist <= 1000);
            Assert.IsFalse(dv.IsValid(nullInput.start));
            Assert.IsTrue(dv.IsValid(nullInput.end));
        }
    }
}