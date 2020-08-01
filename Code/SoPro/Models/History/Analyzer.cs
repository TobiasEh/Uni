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
        public static double lowerTreshold { get; set; }
        public static double upperTreshold { get; set; }

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
            var stationCount = scenario.getStationWorkload().Count;
            var zonePowerList = new List<int>();
            var bookingSuccesRate = calcBookingSuccessRate();

            // Berechne die Summer der Leistung aller Stecker in der Zone.
            scenario.location.zones.ForEach(k =>
            {
                var i = 0;
                k.stations.ForEach(x => i += x.maxPower);
                zonePowerList.Add(i);
            });

            // 1. Fall: Buchungs Erfolgsrate ist geringer als festgelegter Schwellenwert, also wird mehr Infrastruktur wird benötigt.
            if (bookingSuccesRate < lowerTreshold)
            {
                int requiredStations = (int) Math.Ceiling(calcNecessaryWorkload() / (calcBookingSuccessRate() / stationCount));
                int averagedStationPower = (int) Math.Ceiling(zonePowerList.Sum() / (double)stationCount);
                int availablePower = scenario.location.zones.Sum(x => x.maxPower) - zonePowerList.Sum();

                if (availablePower < requiredStations * averagedStationPower)
                    return new List<Suggestion>() { new ExpandInfrastructureSuggestion(requiredStations, 1) };
                return new List<Suggestion>() { new ExpandInfrastructureSuggestion(requiredStations, 0) };

            }

            // 2. Fall: Buchungs Erfolgsrate ist höher als festgelegter Schwellenwert, also kann Infrastruktur abgebaut werden.
            if (bookingSuccesRate > upperTreshold) 
            {
                int unecessaryStations = (int) Math.Floor(calcUnnecessaryWorkload() / (calcBookingSuccessRate() / stationCount));
                int averagedStationPower = (int)Math.Ceiling((double)zonePowerList.Sum() / (double)stationCount);
                if ((double)zonePowerList.Sum() / (double)scenario.location.zones.Count < unecessaryStations * averagedStationPower)
                    return new List<Suggestion>() { new CondenseInfrastructureSuggestion(unecessaryStations, 1) };
                return new List<Suggestion>() { new CondenseInfrastructureSuggestion(unecessaryStations, 0) };
            }

            // 3. Fall: Buchungs Erfolgsrate ist im erwünschten Bereich. Es wird kein Vorschlag unterbreitet.
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

                // Berechne Anteil des Steckers an der Gesamtanzahl an Steckern. Falls Zähler Null, gebge Null zurück.
                double quotaAccepted = acceptedBookingsPlug > 0 ? acceptedBookingsPlug / acceptedBookings : 0;
                double quotaDeclined = declinedBookingsPlug > 0 ? declinedBookingsPlug / declinedBookings : 0;

                plugDistributionAccepted.Add(quotaAccepted);
                plugDistributionDeclined.Add(quotaDeclined);
            }
            return new List<List<double>> { plugDistributionAccepted, plugDistributionDeclined };
        }
    }
}
