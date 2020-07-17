using NUnit.Framework;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.SimulationTest
{
    [TestFixture]
    class SimulatorTest
    {
        private ILocation location = new Location()
        {
            name = "Berlin",
            emergency = 30.2,
        };

        private Vehicle vehicle = new Vehicle()
        {
            capacity = 120,
            model = "Porsche",
            socStart = 24,
            socEnd = 50,
            plugs = new List<PlugType>()
            {
                PlugType.CCS
            }
        };

        private Vehicle vehicle2 = new Vehicle()
        {
            capacity = 140,
            model = "Porsche",
            socStart = 60,
            socEnd = 100,
            plugs = new List<PlugType>()
            {
                PlugType.CCS
            }
        };

        [Test]
        public void testSimulator()
        {
            Simulator sim = new Simulator()
            {

            };
        }
    }
}
