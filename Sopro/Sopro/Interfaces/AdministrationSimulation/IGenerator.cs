using Sopro.Models.Administration;
using Sopro.Models.Simulation;
using System.Collections.Generic;


namespace Sopro.Interfaces.AdministrationSimulation
{
    public interface IGenerator
    {
        List<Booking> generateBookings(Scenario scenario);
    }
}
