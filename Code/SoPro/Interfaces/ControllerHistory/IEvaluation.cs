﻿using Sopro.Models.History;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces.ControllerHistory
{
    public interface IEvaluation
    {
        public List<Suggestion> suggestions { get; set; }
        public double bookingSuccessRate { get; set; }
        public double unneccessaryWorkload { get; set; }
        public double neccessaryWorkload { get; set; }
        public List<double> plugDistributionAccepted { get; set; }
        public List<double> plugDistributionDeclined { get; set; }
        public ExecutedScenario scenario { get; set; }
        public DateTime executed { get; set; }

        public bool addSuggestion(Suggestion suggestion);
        public bool removeSuggestion(Suggestion suggestion);


    }
}
