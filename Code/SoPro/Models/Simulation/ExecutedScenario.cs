using Sopro.Interfaces;
using Sopro.Interfaces.HistorySimulation;
using Sopro.Interfaces.Simulation;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Klasse ist für die Verwaltung ausgeführter Szenarien zuständig.
    /// </summary>
    public class ExecutedScenario : Scenario, IRunnable, IEvaluatable
    {
        public List<double> locationWorkload { get; set; }
        public List<List<double>> stationWorkload { get; set; }
        public int fulfilledRequests { get; set; } = 0;
        public List<Booking> bookings { get; set; }
        public readonly List<Booking> generatedBookings;


        /// <summary>
        /// Konstruktor eines auszuführenden Szenarios.
        /// </summary>
        /// <param name="scenario">
        /// Das Szenario das als Grundlage zur
        /// Erstellung des auszuführenden Szenarios dient.
        /// </param>
        public ExecutedScenario(Scenario scenario)
        {
            bookings = new List<Booking>();
            stationWorkload = new List<List<double>>();
            locationWorkload = new List<double>();

            if (scenario != null)
            {
                duration = scenario.duration;
                bookingCountPerDay = scenario.bookingCountPerDay;
                vehicles = scenario.vehicles;
                rushhours = scenario.rushhours;
                start = scenario.start;
                location = scenario.location.deepCopy();
            }
            generatedBookings = Generator.generateBookings(this);
        }

        public ExecutedScenario(List<Booking> _generatedBookings)
        {
            generatedBookings = _generatedBookings;
            bookings = new List<Booking>();
            stationWorkload = new List<List<double>>();
            locationWorkload = new List<double>();
        }

        /// <summary>
        /// Gibt die Auslastung des Standortes wärend der gesamten Simulation zurück.
        /// </summary>
        /// <returns>Liste der Auslastungen des Standortes pro Tick.</returns>
        public List<double> getLocationWorkload()
        {
            return locationWorkload.ToList();
        }

        /// <summary>
        /// Gibt die Auslastung der Stationen eines Statndortes wärend der gesamten Simulation zurück.
        /// </summary>
        /// <returns>Liste von Listen der Auslastungen jeder Staion por Tick.</returns>
        public List<List<double>> getStationWorkload()
        {
            return stationWorkload;
        }

        /// <summary>
        /// Gibt die Anzahl der erfüllten Anfragen zurück.
        /// </summary>
        /// <returns>Die Anzahl der erfüllten Anfragen.</returns>
        public int getFulfilledRequests()
        {
            return fulfilledRequests;
        }

        /// <summary>
        /// Fügt eine neue Auslastung zu der Liste der Auslastungen des Standortes hinzu.
        /// Fügt eine Liste zu der Liste der Listen der Auslastungen der Stationen hinzu.
        /// </summary>
        /// <param name="location">Neue Auslastugn des Standortes.</param>
        /// <param name="station">Neue Liste an Auslastungen der Station.</param>
        /// <returns>Wahrheitswert, ob das updaten der Listen erfolgreich war.</returns>
        public bool updateWorkload(double location, List<double> station)
        {
            int count = locationWorkload.Count();
            locationWorkload.Add(location);

            if (count == locationWorkload.Count())
                return false;

            count = stationWorkload.Count();
            stationWorkload.Add(station);

            if (count == stationWorkload.Count())
                return false;
            return true;
        }

        /// <summary>
        /// Gibt die Liste der Buchungen zurück.
        /// </summary>
        /// <returns>Liste der Buchungen.</returns>
        public List<Booking> getBookings()
        {
            return bookings;
        }

        /// <summary>
        /// Leert die Liste der Auslastungen des Standortes und die Liste der Listen der Auslastungen der Stationen.
        /// Setzt die Erfüllten Buchungen auf 0.
        /// Leer die Liste Buchungen und fügt ihr die Liste der generierten Buchungen hinzu.
        /// </summary>
        public void clear()
        {
            locationWorkload = new List<double>();
            stationWorkload = new List<List<double>>();
            fulfilledRequests = 0;
            bookings = new List<Booking>();
            bookings.AddRange(generatedBookings);
        }

        public ExecutedScenario deepCopy(ILocation l)
        {
            List<Booking> _generatedBookings = new List<Booking>();
            foreach(Booking b in generatedBookings)
            {
                Booking booking = b.deepCopy();
                booking.location = l;
                _generatedBookings.Add(booking);
            }

            ExecutedScenario copy = new ExecutedScenario(generatedBookings);
            copy.id = id;
            copy.duration = duration;
            copy.bookingCountPerDay = bookingCountPerDay;
            copy.vehicles = vehicles;
            copy.rushhours = rushhours;
            copy.start = start;
            copy.location = l;

            copy.locationWorkload = locationWorkload;
            copy.stationWorkload = stationWorkload;
            copy.fulfilledRequests = fulfilledRequests;

            List<Booking> _bookings = new List<Booking>();
            foreach(Booking b in bookings)
            {
                Booking booking = b.deepCopy();
                booking.location = l;
                _bookings.Add(booking);
            }
            copy.bookings = _bookings;
            return copy;
        }
    }
}
