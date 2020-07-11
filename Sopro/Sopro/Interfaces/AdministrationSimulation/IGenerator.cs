using Sopro.Models.Administration;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.AdministrationSimulation
{
    public class IGenerator
    {
        List<Booking> generateBookings(Scenario scenario);
    }
}
