using Sopro.Models.History;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EvaluationViewModel
    {
        public List<Suggestion> suggestions { get; set; }
        public double bookingSuccessRate { get; set; }
        public double unneccessaryWorkload { get; set; }
        public double neccessaryWorkload { get; set; }
        [EnumLength(1, typeof(PlugType))]
        public List<double> plugDistributionAccepted { get; set; }
        [EnumLength(1, typeof(PlugType))]
        public List<double> plugDistributionDeclined { get; set; }
        public ExecutedScenario scenario { get; set; }
        public List<double> locationWorkload { get; set; }
        public List<List<double>> stationWorkload { get; set; }

        public EvaluationViewModel(Evaluation evaluation)
        {
            suggestions = evaluation.suggestions;
            bookingSuccessRate = evaluation.bookingSuccessRate;
            unneccessaryWorkload = evaluation.unneccessaryWorkload;
            neccessaryWorkload = evaluation.unneccessaryWorkload;
            plugDistributionAccepted = evaluation.plugDistributionAccepted;
            plugDistributionDeclined = evaluation.plugDistributionDeclined;
            scenario = evaluation.scenario;
            locationWorkload = evaluation.scenario.getLocationWorkload();
            stationWorkload = transformStationWorkload(evaluation.scenario.getStationWorkload());
        }

        private List<List<double>> transformStationWorkload(List<List<double>> stationWorkload)
        {
            List<List<double>> transformedWorkload = new List<List<double>>();
            int steps = stationWorkload.Count;
            int stations = stationWorkload[0].Count;

            for (int i = 0; i < stations; ++i)
            {
                List<double> workload = new List<double>();
                foreach (List<double> d in stationWorkload)
                {
                    workload.Add(d[i] * 100);
                }
                transformedWorkload.Add(workload);
            }
            return transformedWorkload;
        }
    }
}
