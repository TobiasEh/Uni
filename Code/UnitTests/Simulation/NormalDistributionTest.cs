﻿using NUnit.Framework;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Simulation
{
    /*
    [TestFixture]
    class NormalDistributionTest
    {
        private DateTime start = DateTime.Now.AddDays(4);
        private DateTime end = DateTime.Now.AddDays(7);
        private int bookings = 150;

        [Test]
        public void generateDateTimeValuesNumberOfBookingsTest()
        {
            NormalDistribution distribution = new NormalDistribution();
            List<DateTime> result = distribution.generateDateTimeValues(start, end, bookings);
            Assert.IsTrue(result.Count == bookings);
        }

        [Test]
        public void generateDateTimeValuesStarEndTest()
        {
            NormalDistribution distribution = new NormalDistribution();
            List<DateTime> result = distribution.generateDateTimeValues(start, end, bookings);
            Console.WriteLine("start: " + start);
            Console.WriteLine("end " + end);
            foreach (DateTime dt in result)
            {
                Console.WriteLine(dt);
            }
            Assert.IsTrue(result.All<DateTime>(e => e >= start && e <= end));
        }

        [Test]
        public void generateDateTimeValuesUnique()
        {
            NormalDistribution distribution = new NormalDistribution();
            List<DateTime> result = distribution.generateDateTimeValues(start, end, bookings);
            foreach (DateTime item in result)
            {
                Console.Out.WriteLine(item);
            }
            
            Assert.IsTrue(result.Count() == result.Distinct().Count());
        }

    }
    */
}
