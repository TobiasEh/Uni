using Sopro.Models.Administration;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Simulation
{
    public static class Generator
    {
        public static double probabilityVIP { get; set; } = 0.05;
        public static double probabilityGUEST { get; set; } = 0.05;
        public static double probabilityADHOC { get; set; } = 0.10;

        private static Random gen = new Random();
        private static int startHour = 6;
        private static int endHour = 20;
        private static int[] lowestPlugPowers;
        private static TimeSpan length = new TimeSpan(0,5,0);
        private static double multiplier = 5;
        private static double multipierMinProbability = 3;

        /// <summary>
        /// Generiert Die Liste der Buchungen für ein Szenario.
        /// Jeder Tag wird dafür in gleich weit entfernte Zeitpunkte geteilt. Für jeden wir ermittelt ob er in einer Stoßzeit liegen.
        /// Falls ja, so wird der aktuelle zeitpunkt auf das Ende der Stoßzeit gesetzt und die von der Stoßzeit gegebenen Buchungen werden erzeugt.
        /// Falls nein wird mit einer immer gleichen Wahrscheinlichkeit eine Buchung für diesen Zeitpunkt erstellt.
        /// Für die Details der Buchungen wird aus der Liste an Autos aus dem Szenario gewählt.
        /// </summary>
        /// <param name="scenario"> Szenario für welches die Buchungen erstellt werden sollen.</param>
        /// <returns> Die Liste der generierten Buchungen.</returns>
        public static List<Booking> generateBookings(Scenario scenario)
        {
            // Berechnet die niedrigste Leistung der verschiedenen Stecker Sorten.
            lowestPlugPowers = lowestPowerPerPlugType(scenario);

            // Berechnet die Wahrscheinlichkeit mit der eine Buchung erstellt werden soll. Die Wahrscheinlichkeit ist 1-(1/xt)^n.
            // n = bookingCountPerDay. 
            // x = wie oft man im Schnitt durch den Tag laufen will bis alle Buchungen verteilt sind.
            // t = Die Anzahl der Zeitabschnitte, in den man den Tag unterteilen kann.
            double maxProbability = 1 - Math.Pow( 1 - ( 1 / ( ( (endHour-startHour) * 60 /length.TotalMinutes) * multiplier)), scenario.bookingCountPerDay);

            // Berechne der wahrscheinlichkeit ohne Stoßzeit.
            double probability = maxProbability / multipierMinProbability;

            List<Booking> generatedBookingsTotal = new List<Booking>();

            DateTime currently = scenario.start;

            // Für jeden Tick. Also jeden Tag.
            for(int i = 0; i < scenario.duration; i++)
            {
                List<Booking> generatedBookings = new List<Booking>();
                // Wird so lange wiederholt bis die Buchungen für einen Tag erreicht wurden.
                while (generatedBookings.Count < scenario.bookingCountPerDay)
                {
                    // Zurücksetzen des Tages, wenn das Ende erreicht wurde. 
                    if(currently.Hour >= endHour)
                    {
                        currently = currently.AddHours(-14);
                    }
                    // Sollte eine Stoßzeit im Szenario liegen, so wird für diese generiert.
                    List<DateTime> generatedStartTimes = null;
                    foreach(Rushhour r in scenario.rushhours)
                    {
                        if (currently > r.start && currently < r.end)
                        {
                            generatedStartTimes = new List<DateTime>();
                            generatedStartTimes = r.strategy.generateDateTimeValues(r.start, r.end, maxProbability, probability, length, scenario.bookingCountPerDay - generatedBookings.Count, r.spread);
                            foreach (DateTime startTime in generatedStartTimes)
                            {
                                generatedBookings.Add(generateBooking(startTime, scenario));
                            }
                            currently = r.end;
                            break;
                        }
                    }
                    
                    // Sollte keine Stoßzeit gefunden worden sein.
                    if(generatedStartTimes == null)
                    {
                        if(gen.NextDouble() <  probability)
                        {
                            generatedBookings.Add(generateBooking(currently, scenario));
                            bool test = generatedBookings.Count < scenario.bookingCountPerDay;
                        }
                        currently = currently.Add(length);
                    }
                }
                // Tages Buchungen hinzufügen und den nächsten Tag weiterzählen.
                generatedBookingsTotal.AddRange(generatedBookings);
                currently = currently.AddDays(1);
            }

            return generatedBookingsTotal;        }

        /// <summary>
        /// Berechnet die niedrigste Leistung der verschiedenen Stecker Sorten.
        /// </summary>
        /// <param name="scenario">Szenario welches mit Buchungen gefüllt werden muss.</param>
        /// <returns>Liste der Leistungen geordnet wie das Enmum PlugType</returns>
        private static int[] lowestPowerPerPlugType(Scenario scenario)
        {
            int[] lowestPlugPowers = new int[Enum.GetNames(typeof(PlugType)).Length];
            Array.Fill(lowestPlugPowers, 999);
            foreach (Zone z in scenario.location.zones)
            {
                foreach (Station s in z.stations)
                {
                    foreach (Plug p in s.plugs)
                    {
                        lowestPlugPowers[(int)p.type] = p.power < lowestPlugPowers[(int)p.type] ? p.power : lowestPlugPowers[(int)p.type];
                    }
                }
            }
            return lowestPlugPowers;
        }

        /// <summary>
        /// Generiert Buchungen mit den übergebenen Variablen.
        /// </summary>
        /// <param name="startTime">Zeitpunkt zu dem der Ladevorgang starten kann.</param>
        /// <param name="scenario">Szenario welches mit Buchungen gefüllt werden muss.></param>
        /// <returns>Eine Generierte Buchung</returns>
        private static Booking generateBooking(DateTime startTime, Scenario scenario)
        {
            int random = gen.Next(scenario.vehicles.Count);

            // Zufälliges Auto füllt die Buchung.
            List<PlugType> plugs = scenario.vehicles[random].plugs;
            int capacity = scenario.vehicles[random].capacity;
            int socEnd = scenario.vehicles[random].socEnd;
            int socStart = scenario.vehicles[random].socStart;

            // Berechnet die höchst möglich benötigte Ladezeit.
            int power = 0;
            foreach (PlugType p in plugs)
            {
                if (power == 0) power = lowestPlugPowers[(int)p];
                if (lowestPlugPowers[(int)p] < power) power = lowestPlugPowers[(int)p];
            }

            double maxChargingDuration = ((socEnd - socStart) / 100.0 * capacity) / power;

            // Erstellt die Endzeit der Buchung.

            DateTime endTime;
            if (maxChargingDuration <= Math.Min(8, 24 - startTime.Hour))
            {
                endTime = startTime.AddHours(gen.Next((int)Math.Ceiling(maxChargingDuration), Math.Min(8, 24 - startTime.Hour)));
            }
            else
            {
                endTime = startTime.AddHours(gen.Next(2, Math.Min(8, 24 - startTime.Hour)));
            }

            return new Booking
            {
                capacity = capacity,
                plugs = plugs,
                socEnd = socEnd,
                socStart = socStart,
                user = "",
                startTime = startTime,
                endTime = endTime,
                station = null,
                active = false,
                priority = rollPriority(),
                location = scenario.location
            };
        }

        /// <summary>
        /// Generiert die Priorität des Benutzers.
        /// </summary>
        /// <returns>Zufällige Priorität.</returns>
        private static UserType rollPriority()
        {
            double probability = gen.NextDouble();

            if (probability > probabilityVIP + probabilityGUEST) return UserType.EMPLOYEE;
            if (probability > probabilityGUEST) return UserType.VIP;
            return UserType.GUEST;
        }

        /*
        public static List<Booking> generateBookings(Scenario scenario)
        {
            // Calculate the lowest power avaibable given a certain plugtype.
            lowestPlugPowers = lowestPowerPerPlugType(scenario);

            if ((scenario.duration * Simulator.tickLength.TotalSeconds) / (24 * 60 * 60) < 1) return distributeLessThanDay(scenario);

            double totalSeconds = scenario.duration * Simulator.tickLength.TotalSeconds;
            double totalDays = totalSeconds / (24 * 60 * 60);
            int duration = (int) totalDays;
            double leftover = totalDays % 1;
            List<Booking> bookingList = new List<Booking>();

            for (int i = 0; i < duration; i++)
            {
                bool exists = false;
                if (scenario.rushhours.Count != 0)
                {
                    int dailyBookingCount = scenario.bookingCountPerDay;
                    DateTime minRushhourStart = new DateTime();

                    foreach (Rushhour rushhour in scenario.rushhours.Where(x => x.start.Day == scenario.start.AddDays(i).Day))
                    {
                        exists = true;
                        minRushhourStart = minRushhourStart == new DateTime() ? rushhour.start : minRushhourStart > rushhour.start ? rushhour.start : minRushhourStart;
                        var startTimes = rushhour.run();
                        if (rushhour.bookings > dailyBookingCount)
                        {
                            startTimes.RemoveRange(dailyBookingCount - 1 < 0 ? 0 : dailyBookingCount - 1, rushhour.run().Count - dailyBookingCount);
                        }
                        if (startTimes.Count != 0)
                            foreach (DateTime start in startTimes)
                            {
                                bookingList.Add(generateBooking(start, scenario));
                                dailyBookingCount--;
                            };
                    }
                    if (dailyBookingCount != 0 && exists == true)
                    {
                        List<Booking> linDistBookings = new List<Booking>(); ;
                        linearBookingDistribution(linDistBookings, scenario, i);
                        linDistBookings.RemoveRange(linDistBookings.FindIndex(x => x.startTime >= minRushhourStart), scenario.bookingCountPerDay - dailyBookingCount);
                        bookingList.AddRange(linDistBookings);
                    }
                    if (exists == false) linearBookingDistribution(bookingList, scenario, i);
                }
                else
                {
                    linearBookingDistribution(bookingList, scenario, i);
                }
            }
            if (leftover != 0 && leftover * 24 * 60 >= (24 * 60) / scenario.bookingCountPerDay)
            {
                bookingList.AddRange(distributeLessThanDay(scenario));
            }

            return bookingList;
        }

        private static List<Booking> distributeLessThanDay(Scenario scenario)
        {
            List<Booking> bookingList = new List<Booking>();
            double leftover = (scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60) % 1;
            int dailyBookingCount = (int)(scenario.bookingCountPerDay * leftover);

            if (scenario.rushhours.FindAll(x => x.start >= scenario.start && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)).Count != 0)
            {
                DateTime minRushhourStart = new DateTime();
                foreach (Rushhour rh in scenario.rushhours.Where(x => x.start >= scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)) && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)))
                {
                    minRushhourStart = minRushhourStart == new DateTime() ? rh.start : minRushhourStart > rh.start ? rh.start : minRushhourStart;
                    List<DateTime> startTimes = rh.run();
                    if (rh.end > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds))
                    {
                        startTimes.RemoveAll(x => x > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds));
                    }
                    if (startTimes.Count != 0)
                        foreach (DateTime start in startTimes)
                        {
                            bookingList.Add(generateBooking(start, scenario));
                            dailyBookingCount--;
                            if (dailyBookingCount <= 0) break;
                        };
                    if (dailyBookingCount <= 0) break;
                }
                if (dailyBookingCount < 0)
                {
                    List<Booking> linDistBookings = new List<Booking>(); ;
                    linearBookingDistribution(linDistBookings, scenario, 0);
                    linDistBookings.RemoveRange(linDistBookings.FindIndex(x => x.startTime >= minRushhourStart), scenario.bookingCountPerDay - Math.Abs(dailyBookingCount));
                    bookingList.AddRange(linDistBookings);
                }
            }
            else
            {
                linearBookingDistribution(bookingList, scenario, 0);
                bookingList.RemoveAll(x => x.startTime > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds) && x.startTime < scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)));
                bookingList.RemoveRange(bookingList.Count - 1, bookingList.Count - dailyBookingCount);
            }
            return bookingList;
        }

        /// <summary>
        /// Roll user priority.
        /// </summary>
        /// <returns>Random user role.</returns>
        private static UserType rollPriority()
        {
            double probability = gen.NextDouble();

            if (probability > probabilityVIP + probabilityGUEST) return UserType.EMPLOYEE;
            if (probability > probabilityGUEST) return UserType.VIP;
            return UserType.GUEST;
        }

        /// <summary>
        /// Calculates whatever plug of a certain Plugtype in a specific scenario delivers the lowest power.
        /// </summary>
        /// <param name="scenario">Simulated scenario.</param>
        private static int[] lowestPowerPerPlugType(Scenario scenario)
        {
            int[] lowestPlugPowers = new int[Enum.GetNames(typeof(PlugType)).Length];
            Array.Fill(lowestPlugPowers, 999);
            foreach (Zone z in scenario.location.zones)
            {
                foreach (Station s in z.stations)
                {
                    foreach (Plug p in s.plugs)
                    {
                        lowestPlugPowers[(int)p.type] = p.power < lowestPlugPowers[(int)p.type] ? p.power : lowestPlugPowers[(int)p.type];
                    }
                }
            }
            return lowestPlugPowers;
        }

        /// <summary>
        /// Generates a booking using given values.
        /// </summary>
        /// <param name="startTime">Point of time where charging could begin.</param>
        /// <param name="scenario">Simulated scenario.</param>
        /// <returns>A generated booking.</returns>
        private static Booking generateBooking(DateTime startTime, Scenario scenario)
        {
            int random = gen.Next(scenario.vehicles.Count);

            // Randomize booking fields
            List<PlugType> plugs = scenario.vehicles[random].plugs;
            int capacity = scenario.vehicles[random].capacity;
            int socEnd = scenario.vehicles[random].socEnd;
            int socStart = scenario.vehicles[random].socStart;

            // Calculate lowest power delivery in order to estimate highest possible charging duration afterwards.
            int power = 0;
            foreach (PlugType p in plugs)
            {
                if (power == 0) power = lowestPlugPowers[(int) p];
                if (lowestPlugPowers[(int) p] < power) power = lowestPlugPowers[(int) p];
            }

            double maxChargingDuration = ((socEnd - socStart) / 100.0 * capacity) / power;

            // Calculate booking end time.
            
            DateTime endTime;
            if (maxChargingDuration <= Math.Min(8, 24 - startTime.Hour))
            {
                endTime = startTime.AddHours(gen.Next((int)Math.Ceiling(maxChargingDuration), Math.Min(8, 24 - startTime.Hour)));
            } 
            else
            {
                endTime = startTime.AddHours(gen.Next(2, Math.Min(8, 24 - startTime.Hour)));
            }

            return new Booking
            {
                capacity = capacity,
                plugs = plugs,
                socEnd = socEnd,
                socStart = socStart,
                user = "",
                startTime = startTime,
                endTime = endTime,
                station = null,
                active = false,
                priority = rollPriority(),
                location = scenario.location
            };                     
        }

        /// <summary>
        /// Fills a given list with evenly distributed and generated bookings. 
        /// </summary>
        /// <param name="bookingList">A list of bookings to be filled.</param>
        /// <param name="scenario">The scenario being simulated.</param>
        /// <param name="passedDays">How many days have passed in the simulation.</param>
        public static void linearBookingDistribution(List<Booking> bookingList, Scenario scenario, int passedDays)
        {
            // Create an equal amount of linearly distributed bookings for each simulated day
            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                DateTime currentDay = scenario.start.AddDays(passedDays);
                TimeSpan normalize = new TimeSpan(-currentDay.Hour, -currentDay.Minute, -currentDay.Second);
                currentDay += normalize;
                // Booking request is set this many minutes after startHour
                double minutesUntilRequest = (((endHour - startHour) * 60) / scenario.bookingCountPerDay) * j;

                DateTime start = currentDay.AddHours(startHour).AddMinutes(minutesUntilRequest);
                // Length of booking timespan equals min(8, hours until midnight)
                // DateTime end = start.AddHours(gen.Next(2, Math.Min(8, 24 - start.Hour)));

                Booking b = generateBooking(start, scenario);
                bookingList.Add(b);
            }
        }*/
    }
}
