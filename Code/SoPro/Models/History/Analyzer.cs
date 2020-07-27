using Sopro.Interfaces.HistorySimulation;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.History
{
    /// <summary>
    /// Klasse die eine Evaluation zu einem bereits ausgeführtem Szenario generiert.
    /// </summary>
    public static class Analyzer
    {
        public static Evaluation evaluation { get; set; }
        public static IEvaluatable scenario { get; set; }
        public static double lowerGate { get; set; }
        public static double upperGate { get; set; }

        /// <summary>
        /// Analysiert ein gegebenes Szenario. Dabei werden verschiedene Leistungsmetriken
        /// quantifiziert. Die Rate der erfolgreichen Buchungen, überflüssige Auslastung,
        /// zusätzlicher Bedarf an Resourcen, Verteilung der Stecker akzeptierter Buchungen
        /// und Verteilung der Stecker abgelehnter Buchungen.
        /// </summary>
        /// <param name="_scenario">Das zu untersuchende Szenario.</param>
        /// <returns></returns>
        public static Evaluation analyze(IEvaluatable _scenario)
        {
            scenario = _scenario;

            List<List<double>> plugTypeDistribution = calcPlugTypeDistribution();

            evaluation = new Evaluation()
            {
                suggestions = createSuggestion(),
                bookingSuccessRate = calcBookingSuccessRate(),
                unneccessaryWorkload = calcUnnecessaryWorkload(),
                neccessaryWorkload = calcNecessaryWorkload(),
                plugDistributionAccepted = plugTypeDistribution[0],
                plugDistributionDeclined = plugTypeDistribution[1]
            };

            return evaluation;
        }

        /// <summary>
        /// Generiere Vorschläge, die dem Planer unterbreitet werden.
        /// </summary>
        /// <returns>Die generierten Vorschläge.</returns>
        private static List<Suggestion> createSuggestion()
        {
            int nZones = 0;
            int nStation = scenario.getStationWorkload().Count;
            var zonePower = new List<int>();

            scenario.location.zones.ForEach( k =>
            {
                int i = 0;
                k.stations.ForEach(x => i += x.maxPower);
                zonePower.Add(i);
            });

            if (calcBookingSuccessRate() < lowerGate)
            {
                nStation = (int)(nStation * calcNecessaryWorkload() / 100);
                int nStationC = nStation;
                foreach (Zone zone in scenario.location.zones)
                {
                    int i = 1;
                    while (zone.maxPower > zonePower[scenario.location.zones.IndexOf(zone)] + (zonePower.Sum() / nStation) * i && nStationC != 0)
                    {
                        nStationC--;
                        i++;
                    }
                }
                if(nStationC < 0)
                {
                    while ((0 - nStationC) * (zonePower.Sum() / nStation) > zonePower.Average())
                    {
                        nZones++;
                        nStationC -= (int)(zonePower.Average() / (zonePower.Sum() / nStation));
                    }
                }
                return new List<Suggestion>() { new ExpandInfrastructureSuggestion(nStation, nZones) };
            }
            else if (calcBookingSuccessRate() > upperGate)
            {
                nStation = (int)(nStation * calcUnnecessaryWorkload() / 100);
                List<int> stationpZone = new List<int>();
                scenario.location.zones.ForEach(x => stationpZone.Add(x.stations.Count));
                if (nStation > stationpZone.Average())
                {
                   nZones = nStation / (int)stationpZone.Average();
                }
                return new List<Suggestion>() { new CondenseInfrastructureSuggestion(nStation, nZones) };
            }
            return new List<Suggestion>();
        }

        /// <summary>
        /// Berechnet den Anteil der akzeptierten Buchungsanfragen,
        /// als Quotient aus akzeptierten Buchungen des Szenarios und allen Buchungen,
        /// im Szenario gestellten, Buchungsanfragen.
        /// </summary>
        /// <returns>Den Anteil der akzeptierten Buchungsanfragen</returns>
        private static double calcBookingSuccessRate()
        {
            return 100 * (double)scenario.getFulfilledRequests() / scenario.getBookings().Count;
        }

        /// <summary>
        /// Berechnet inwiefern die Infrastruktur überdimenstioniert ist.
        /// Berechnet dazu einfach den Kehrwert der prozentualen Auslastung des Standorts.
        /// </summary>
        /// <returns>Zu wie viel Prozent die Infrastruktur überdimensioniert ist.</returns>
        private static double calcUnnecessaryWorkload()
        {
            return 100 - scenario.getLocationWorkload().Max();
        }

        /// <summary>
        /// Berechnet inwiefern die Infrastruktur unterdimensioniert ist.
        /// Berechner dazu einfach den Kehrwert der Buchungs Erfolgsrate.
        /// </summary>
        /// <returns>Zu wie viel Prozen die Infrastruktur unterdimensioniert ist.</returns>
        private static double calcNecessaryWorkload()
        {
            return 100 - calcBookingSuccessRate();
        }

        /// <summary>
        /// Berechnet den prozentualen Anteil der Steckertypen unter den akzeptierten und abgelehnten Buchungen.
        /// </summary>
        /// <returns>
        /// Eine Liste zweier Listen aus double-Werten mit folgendem Inhalt:
        /// 1. Verteilung der Steckertypen der akzeptierten Buchungen.
        /// 2. Verteilung der Steckertypen der abgelehnten Buchungen.
        /// </returns>
        private static List<List<double>> calcPlugTypeDistribution()
        {
            List<double> plugDistributionAccepted = new List<double>();
            List<double> plugDistributionDeclined = new List<double>();

            foreach (PlugType plug in (PlugType[])Enum.GetValues(typeof(PlugType)))
            {
                double acceptedBookingsPlug = 0;
                double acceptedBookings = 0;
                double declinedBookingsPlug = 0;
                double declinedBookings = 0;

                foreach (Booking booking in scenario.getBookings())
                {
                    if (booking.station != null)
                    {
                        if (booking.plugs.Contains(plug))
                            acceptedBookingsPlug++;
                        acceptedBookings++;
                    }
                    else
                    {
                        if (booking.plugs.Contains(plug))
                            declinedBookingsPlug++;
                        declinedBookings++;
                    }
                }

                double quotaAccepted = acceptedBookingsPlug / acceptedBookings;
                double quotaDeclined = declinedBookingsPlug / declinedBookings;

                plugDistributionAccepted.Add(quotaAccepted);
                plugDistributionDeclined.Add(quotaDeclined);
            }
            return new List<List<double>> { plugDistributionAccepted, plugDistributionDeclined };
        }
    }
}
