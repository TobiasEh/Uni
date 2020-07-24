using Sopro.Interfaces.HistorySimulation;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.History
{
    public static class Analyzer
    {
        public static Evaluation evaluation { get; set; }
        public static IEvaluatable scenario { get; set; }
        public static double lowerGate { get; set; }
        public static double upperGate { get; set; }

        
        public static Evaluation analyze(IEvaluatable _scenario)
        {
            scenario = _scenario;
            evaluation = new Evaluation()
            {
                suggestions = createSuggestion(),
                bookingSuccessRate = calcBookingSuccessRate(),
                unneccessaryWorkload = calcUnnecessaryWorkload(),
                neccessaryWorkload = calcNecessaryWorkload(),
                plugDistributionAccepted = calcPlugDistributionAccepted(),
                plugDistributionDeclined = calcPlugDistributionDeclined()
            };

            return evaluation;
        }
        private static List<Suggestion> createSuggestion()
        {
            var nZones = 0;
            int nStation = scenario.getStationWorkload().Count;
            var zonePower = new List<int>();
            // Initialise List with sum of station power per zone.
            scenario.location.zones.ForEach( k =>
            {
                var i = 0;
                k.stations.ForEach(x => i += x.maxPower);
                zonePower.Add(i);
            });
            // Create suggestion based on the calculated Worokload. 
            // If the Booking Success Rate if lower than the lower gate calculate the number of stations to be added
            // and a number of zones based on the average power of the zones and nubmer of stations in each of them.
            // If the Booking Success Rate is higher, an suggestion will be made on how many stations and zones can 
            // be removed.
            // If the Booking Success Rate is inbetween no suggestion will be made.
            if (calcBookingSuccessRate() < lowerGate)
            {
                // Approximation of stations that needs to be added.
                nStation = (int)(nStation * calcNecessaryWorkload() / 100);
                int nStationCopy = nStation;
                // foreach zone, assess if adding a zone does not exceed the zone's max power,
                // reduce a copy of nStation by the number of stations that could be added to the already existing stations.                
                foreach (Zone zone in scenario.location.zones)
                {
                    int i = 1;
                    while (zone.maxPower > zonePower[scenario.location.zones.IndexOf(zone)]+(zonePower.Sum()/nStation)*i && nStationCopy != 0)
                    {
                        nStationCopy--;
                        i++;
                    }
                }
                // If there are remianing zones and the number of stations remaining times the average station power 
                // exceeds the average power of a zone, a zone will be added to the suggestion as long as it exceeds the average of a zone.
                if(nStationCopy > 0)
                {
                    while ((nStationCopy) * (zonePower.Sum() / nStation) > zonePower.Average())
                    {
                        nZones++;
                        nStationCopy -= (int)(zonePower.Average() / (zonePower.Sum() / nStation));
                    }
                }
                return new List<Suggestion>() { new ExpandInfrastructureSuggestion(nStation, nZones) };
            }
            else if (calcBookingSuccessRate() > upperGate)
            {
                nStation = (int)(nStation * calcUnnecessaryWorkload() / 100);
                var stationsPerZone = new List<int>();
                scenario.location.zones.ForEach(x => stationsPerZone.Add(x.stations.Count));
                // Estimates how many zones can be removed by comparing to the average number of stations per zone.
                if (nStation > stationsPerZone.Average())
                {
                   nZones = nStation/ (int)stationsPerZone.Average();
                }
                return new List<Suggestion>() { new CondenseInfrastructureSuggestion(nStation, nZones) };
            }
            else
            {
                return new List<Suggestion>();
            }
            
            
        }
        private static double calcBookingSuccessRate()
        {
            return 100 * (double)scenario.getFulfilledRequests() / ((double)scenario.getBookings().Count);
        }
        private static double calcUnnecessaryWorkload()
        {
            return 100 - scenario.getLocationWorkload().Max();
        }
        private static double calcNecessaryWorkload()
        {
            return 100 - calcBookingSuccessRate();
        }
        private static List<double> calcPlugDistributionAccepted()
        {           
            List<double> percPlug = new List<double>();
            foreach (PlugType plug in (PlugType[])Enum.GetValues(typeof(PlugType)))
            {
                percPlug.Add((double)scenario.getBookings().FindAll(x => x.plugs.Contains(plug) && x.station != null).Count / (double)scenario.getBookings().FindAll(x => x.station != null).Count);
            }

            return percPlug;
        }
        private static List<double> calcPlugDistributionDeclined()
        {
            List<double> percPlug = new List<double>();
            foreach (PlugType plug in (PlugType[])Enum.GetValues(typeof(PlugType)))
            {
                percPlug.Add((double)scenario.getBookings().FindAll(x => x.plugs.Contains(plug) && (x.station == null )).Count / (double)scenario.getBookings().FindAll(x => x.station == null).Count);
            }

            return percPlug;
        }
    }
}
