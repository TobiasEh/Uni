using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.History
{
    public class Evaluation : IEvaluation
    {
        [Range(0,1)]
        public List<Suggestion> suggestions { get; set; }
        public double bookingSuccessRate{ get; set; }
        public double unneccessaryWorkload { get; set; }
        public double neccessaryWorkload { get; set; }
        [EnumLength(1,typeof(PlugType))]
        public List<double> plugDistributionAccepted { get; set; }
        [EnumLength(1, typeof(PlugType))]
        public List<double> plugDistributionDeclined { get; set; }

        public bool addSuggestion(Suggestion suggestion)
        {
            var pCount = suggestions.Count;
            suggestions.Add(suggestion);
            return pCount == suggestions.Count+1;
        }
        public bool removeSuggestion(Suggestion suggestion)
        {
            var pCount = suggestions.Count;
            suggestions.Remove(suggestion);
            return pCount == suggestions.Count + 1;
        }
    }
}