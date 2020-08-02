using Sopro.Interfaces;
using Sopro.Models.Administration;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
namespace Sopro.ViewModels.ExportImportViewModel
{
    public class ExecutedScenarioExportImportViewModel
    {
        public string id { get; set; }
        public int duration { get; set; }
        public int bookingCountPerDay { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<RushhourExportImportViewModel> rushhours { get; set; } = new List<RushhourExportImportViewModel>();
        public DateTime start { get; set; }
        public LocationExportImportViewModel location { get; set; }

        public List<double> locationWorkload { get; set; }
        public List<List<double>> stationWorkload { get; set; }
        public int fulfilledRequests { private get; set; } = 0;
        public List<BookingExportImportViewModel> bookings { get; set; }
        public List<BookingExportImportViewModel> generatedBookings { get; set; }

        public ExecutedScenarioExportImportViewModel() { }

        public ExecutedScenarioExportImportViewModel(ExecutedScenario s)
        {
            id = s.id;
            duration = s.duration;
            bookingCountPerDay = s.bookingCountPerDay;
            vehicles = s.vehicles;
            foreach (Rushhour r in s.rushhours)
            {
                rushhours.Add(new RushhourExportImportViewModel(r));
            }
            start = s.start;
            location = new LocationExportImportViewModel(s.location);
            locationWorkload = s.locationWorkload;
            stationWorkload = s.stationWorkload;
            fulfilledRequests = s.fulfilledRequests;
            bookings = new List<BookingExportImportViewModel>();

            foreach (Booking b in s.bookings){
                b.location = null;
                bookings.Add(new BookingExportImportViewModel(b));
            }

            generatedBookings = new List<BookingExportImportViewModel>();

            foreach (Booking b in s.generatedBookings)
            {
                b.location = null;
                generatedBookings.Add(new BookingExportImportViewModel(b));
            }
        }

        public ExecutedScenario generateScenario()
        {
            List<Booking>  _generatedBookings = new List<Booking>();
            ILocation l = location.generateLocation();
            foreach (BookingExportImportViewModel b in generatedBookings)
            {
                Booking booking = (Booking)b.generateBooking();
                booking.location = l;
                _generatedBookings.Add(booking);
            }
            ExecutedScenario s = new ExecutedScenario(_generatedBookings);
            s.id = id;
            s.duration = duration;
            s.bookingCountPerDay = bookingCountPerDay;
            s.vehicles = vehicles;
            s.rushhours = new List<Rushhour>();
            foreach (RushhourExportImportViewModel r in rushhours)
            {
                s.rushhours.Add(r.generateRushhour());
            }
            s.start = start;
            s.location = l;

            s.locationWorkload = locationWorkload;
            s.stationWorkload = stationWorkload;
            s.fulfilledRequests = fulfilledRequests;
            List<Booking>  _bookings = new List<Booking>();

            foreach (BookingExportImportViewModel b in bookings)
            {
                Booking booking = (Booking)b.generateBooking();
                booking.location = l;
                _bookings.Add((Booking)b.generateBooking());
            }
            s.bookings = _bookings;
            return s;
        }
    }
}
