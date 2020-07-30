using Sopro.Interfaces;
using Sopro.Interfaces.ControllerHistory;
using Sopro.Models.History;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EvaluationExportImportViewModel
    {
        public List<Suggestion> suggestions { get; set; }
        public double bookingSuccessRate { get; set; }
        public double unneccessaryWorkload { get; set; }
        public double neccessaryWorkload { get; set; }
        public List<double> plugDistributionAccepted { get; set; }
        public List<double> plugDistributionDeclined { get; set; }
    }

    public EvaluationExportImportViewModel() { }

    public EvaluationExportImportViewModel(IEvaluation e) 
    {
        suggestions = e.suggestions;
    }
}
