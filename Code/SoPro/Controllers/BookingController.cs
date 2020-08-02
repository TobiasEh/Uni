using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using Sopro.Interfaces;
using Microsoft.AspNetCore.Http;
using Sopro.Models.User;
using Sopro.Models.Administration;
using Sopro.Interfaces.AdministrationController;
using Sopro.Models.Infrastructure;
namespace Sopro.Controllers
{
    /// <summary>
    /// Kontroller Klasse für das Verwalten der Buchungen auf der Gui.
    /// </summary>
    public class BookingController : Controller
    {
        private IMemoryCache cache;
        List<IBooking> bookings;

        /// <summary>
        /// Konstruktor des Kontrollers für die Buchungen.
        /// </summary>
        /// <param name="_cache"> Cache der Anwendung.</param>
        public BookingController(IMemoryCache _cache)
        {
            cache = _cache;
        }


        /// <summary>
        /// Zeigt dem Benutzer eine Übersicht seiner Buchungen an. 
        /// Sollte der Benutzer über die Rolle PLANER verfügen, wird er weitergeleitet auf die Adminsicht. 
        /// Sollte er nicht angemeldet sein, wird er auf die Startseite weitergeleitet.
        /// </summary>
        /// <returns>Eine Übersicht aller Buchungen.</returns>
        public IActionResult Index()
        {
            // Einlesen der session Variable "role", welche die Rolle des Benutzer beschreibt.
            var userID = this.HttpContext.Session.GetString("role");

            // Weiterleiten auf die Startseite sollte der Benutzer nicht eingeloggt sein.
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Weiterleiten auf die admin Seite sollte der Benutzer nicht eingeloggt sein.
            if (userID.Equals(UserType.PLANER.ToString()))
            {
                return RedirectToAction("Index", "Admin");
            }

            List<Booking> unscheduledBookings = new List<Booking>();
            List<Booking> scheduledBookings = new List<Booking>();

            var cacheKey = CacheKeys.BOOKING;
            // Die Buchungen werden aus dem Cache geladen.
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Befüllen der Listen unscheduledBookings und scheduledBookings, mit den Buchungen, welche die Rolle Assistance sehen darf.
            if (userID.Equals(UserType.ASSISTANCE.ToString()))
            {
                foreach (IBooking item in bookings)
                    if (item.priority == UserType.VIP || item.priority == UserType.GUEST)
                    {
                        if (item.station == null)
                        {
                            unscheduledBookings.Add((Booking)item);
                        }
                        else if (item.station != null)
                        {
                            scheduledBookings.Add((Booking)item);
                        }
                    }
                return View(new DashboardViewModel(scheduledBookings, unscheduledBookings));
            }
            else
            {
                // Befüllen der Listen unscheduledBookings und scheduledBookings, mit den Buchungen, welche der Benutzer sehen darf.
                var email = this.HttpContext.Session.GetString("email");
                foreach (IBooking item in bookings)
                    if (item.user == email)
                    {
                        if (item.station == null)
                        {
                            unscheduledBookings.Add((Booking)item);
                        }
                        else if (item.station != null)
                        {
                            scheduledBookings.Add((Booking)item);
                        }
                    }
                return View(new DashboardViewModel(scheduledBookings, unscheduledBookings));
            }
        }

        /// <summary>
        /// Zeigt Benutzer eine Seite an, auf welcher er neue Buchungen erstellen kann.
        /// </summary>
        /// <param name="booking">Eine leere Buchung, oder sollte versucht werden eine Buchung erstellt zu werden, welche nicht den Richtlienen entspricht diese Buchung.</param>
        /// <returns>Eine Seite für das Erstellen der Buchungen.</returns>
        public IActionResult Create(Booking booking)
        {

            // Die Liste der Standorte wird aus dem Cache geladen.
            var cacheKey = CacheKeys.LOCATION;
            List<ILocation> locations = (List<ILocation>)cache.Get(cacheKey);
            if (locations == null)
            {
                locations = new List<ILocation>();
            }

            // Sollte die Buchung noch startzeiten mit 0 initialisiert haben wird die Startzeit und die endzeit auf die aktuelle Uhrzeit gestzt.
            if (booking.startTime == new DateTime() && booking.endTime == new DateTime())
            {
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute + 1, 0);
                booking.startTime = now;
                booking.endTime = now;
            }

            // Erstellen des ViewModels für die Seite. 
            BookingCreateViewModel viewmodel = new BookingCreateViewModel(locations, booking, booking.plugs.Contains(PlugType.CCS), booking.plugs.Contains(PlugType.TYPE2));

            return View("Create", viewmodel);
        }

        /// <summary>
        /// Testet ob die Buchung, welche mithilfe des viewmodel erstellt wurde valide ist.
        /// Wenn nicht wird auf die Create Methode verwiesen.
        /// Ist sie valide, wird auf die Index Methode verwiesen.
        /// </summary>
        /// <param name="viewmodel">Daten mit welchen die Buchung gebaut werden kann.</param>
        /// <returns>Die Methode, welche das weitere Vorgehen beschreibt.</returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Post(string id,BookingCreateViewModel viewmodel)
        {
            IBooking booking = viewmodel.booking;
            booking.id = id;
            // Die Liste der Standorte wird aus dem Cache geladen.
            List<ILocation> locations = (List<ILocation>)cache.Get(CacheKeys.LOCATION);
            if (locations == null)
            {
                locations = new List<ILocation>();
            }

            // Sollte der Benutzer kein ASSISTANCE sein wird seine E-Mail als die E-Mail des Benutzers gestzt.
            
            if (booking.priority == UserType.EMPLOYEE && !this.HttpContext.Session.GetString("role").Equals(UserType.PLANER.ToString()))
            {
                booking.user = this.HttpContext.Session.GetString("email");
            }

            // Filtern des richtigen Standorts aus der Liste der Standorte.
            booking.location = locations.Find(x => x.id.Equals( viewmodel.locationId));

            // Befüllen der Liste an Stecker, welche zum Auto des Benutzers passen.
            List<PlugType> plugs = new List<PlugType>();
            if (viewmodel.ccs)
            {
                plugs.Add(PlugType.CCS);
            }
            if (viewmodel.type2)
            {
                plugs.Add(PlugType.TYPE2);
            }
            booking.plugs = plugs;

            bool test = ModelState.IsValid;
            // Validierung der Buchung, bei Fehlschlag wird an die Create Methode weitergeleited.
            if (!TryValidateModel(booking, nameof(booking)))
            {
                return RedirectToAction("Create", "Booking", booking);
            }

            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Im Edit Fall wird die Buchung, welche editiert wurde entfernt.
            foreach(Booking b in bookings)
            {
                if (b.id.Equals(booking.id))
                {
                    if (!this.HttpContext.Session.GetString("role").Equals(UserType.ASSISTANCE.ToString()))
                    {
                        booking.priority = b.priority;
                        booking.user = b.user;
                    }
                    bookings.Remove(b);
                    
                    break;
                }
            }

            // Sollte der Benutzer eine Adhoc Buchung vornehmen und VIP sein, wird diese erstellt befüllt, verteilt, dem Cache hinzugefügt und es wird zur Index Methode weitergeleited.
            if (booking.startTime.Date == DateTime.Now.Date)
            {
                if (booking.priority != UserType.VIP)
                {
                    return RedirectToAction("Create", "Booking", booking);
                }
                Adhoc adhoc = new Adhoc(false)
                {
                    capacity = booking.capacity,
                    plugs = booking.plugs,
                    socStart = booking.socStart,
                    socEnd = booking.socEnd,
                    user = booking.user,
                    startTime = booking.startTime,
                    endTime = booking.endTime,
                    active = booking.active,
                    location = booking.location,
                    priority = booking.priority
                };
                bookings.Add(adhoc);
                cache.Set(cacheKey, bookings);
                adhoc.triggerBookingDistribution();
                return RedirectToAction("Index", "Booking");
            }

            // Hinzufügen der Buchung zum Cache.
            bookings.Add(booking);
            cache.Set(cacheKey, bookings);

            return RedirectToAction("Index", "Booking");
        }

        /// <summary>
        /// Methode für das Bearbeiten bestehender Buchungen.
        /// Entfernt die Buchung aus dem Cache.
        /// </summary>
        /// <param name="bookingID"> ID der Buchung, welche bearbeitet werden soll.</param>
        /// <returns> Eine Seite für das Bearbeiten der Buchung.</returns>
        public IActionResult Edit(string bookingID)
        {
            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Herausfiltern der Buchung, welche die übergebene Id hat.
            Booking booking = bookings.Find(x => x.id.Equals(bookingID)) as Booking;


            // Die Liste der Standorte wird aus dem Cache geladen.
            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Anlegen des ViewModels für die Seite.
            BookingCreateViewModel viewmodel = new BookingCreateViewModel(locations, booking, false, false);
            if (booking.plugs.Contains(PlugType.CCS))
            {
                viewmodel.ccs = true;
            }
            if (booking.plugs.Contains(PlugType.TYPE2))
            {
                viewmodel.type2 = true;
            }

            return View("Create",viewmodel);
        }
        
        /// <summary>
        /// Methode für das Löschen einer Buchung.
        /// </summary>
        /// <param name="bookingID">Die ID der zu löschenten Buchung.</param>
        /// <returns> Weiterleitung zu der Index Methode.</returns>
        public IActionResult Delete(string bookingID)
        {
            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Die Buchung ermittelt, entfernt und es wird weitergeleitet zur Index Methode.
            if (bookings.RemoveAll(x => x.id.Equals(bookingID)) == 1)
            {
                cache.Set(cacheKey, bookings);
                return RedirectToAction("index");
            }
            //Sollte Keine Buchung gefunden werden. Nicht Möglich.
            return RedirectToAction("index");
        }

        /// <summary>
        /// Methode für das ein Checken und aus Checken. 
        /// Das activ Attribut wird abgeändert sollte der Benutzer sich im Zeitraum befinden.
        /// </summary>
        /// <param name="bookingID"></param>
        /// <returns></returns>
        public IActionResult ToggleCheck(string bookingID)
        {
            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Die Buchung ermittelt.
            Booking booking = null;
            booking = bookings.Find(x => x.id.Equals(bookingID)) as Booking;

            // Sollte die Buchung Aktiv sein, wird sie inaktiv.
            if (booking.active)
            {
                booking.active = false;
                return RedirectToAction("Index");
            }
            // Sollte die Buchung inaktiv sein und die aktuelle Zeit im Ladezeitraum liegen, wird diese aktiviert.
            else if (booking.startTime >= DateTime.Now && booking.endTime <= DateTime.Now)
            {
                booking.active = true;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
