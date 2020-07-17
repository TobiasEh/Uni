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

        
        public static Evaluation analyze()
        {
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
            int nZones = 0;
            int nStation = scenario.getStationWorkload().Count;
            var zonePower = new List<int>();
            scenario.location.zones.ForEach( k =>
            {
                int i = 0;
                k.stations.ForEach(x => i += x.maxPower);
                zonePower.Add(i);
            });

            /*
             * basically this^
            foreach (Zone zone in scenario.location.zones)
            {
                j = 0;
                foreach (Station station in zone.stations) 
                {
                    j += station.maxPower;
                }
                zonePower.Add(j);
            }
            */

            if (calcBookingSuccessRate() < lowerGate)
            {
                nStation = (int)(nStation * calcNecessaryWorkload());
                int nStationC = nStation;
                foreach (Zone zone in scenario.location.zones)
                {
                    int i = 1;
                    while (zone.maxPower > zonePower[scenario.location.zones.IndexOf(zone)]+(zonePower.Sum()/nStation)*i && nStationC != 0)
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
                nStation = (int)(nStation * calcUnnecessaryWorkload());
                List<int> stationpZone = new List<int>();
                scenario.location.zones.ForEach(x => stationpZone.Add(x.stations.Count));
                if (nStation > stationpZone.Average())
                {
                   nZones = nStation/ (int)stationpZone.Average();
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
            return 100*(double)scenario.getFulfilledRequests()/((double)scenario.getBookings().Count);
        }
        private static double calcUnnecessaryWorkload()
        {
            return 100-scenario.getLocationWorkload().Max();
        }
        private static double calcNecessaryWorkload()
        {
            return 100-calcBookingSuccessRate();
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
