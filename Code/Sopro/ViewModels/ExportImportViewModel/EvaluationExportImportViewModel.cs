using Sopro.Interfaces.ControllerHistory;
using Sopro.Models.History;
using Sopro.Models.Simulation;
using Sopro.ViewModels.ExportImportViewModel;
using System.Collections.Generic;
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
        public ExecutedScenarioExportImportViewModel scenario { get; set; }


        public EvaluationExportImportViewModel() { }

        public EvaluationExportImportViewModel(IEvaluation e)
        {
            suggestions = e.suggestions;
            bookingSuccessRate = e.bookingSuccessRate;
            unneccessaryWorkload = e.unneccessaryWorkload;
            neccessaryWorkload = e.neccessaryWorkload;
            plugDistributionAccepted = e.plugDistributionAccepted;
            plugDistributionDeclined = e.plugDistributionDeclined;
            scenario = new ExecutedScenarioExportImportViewModel(e.scenario);
        }

        public IEvaluation generateEvaluation()
        {
            IEvaluation eva = new Evaluation();
            eva.suggestions = suggestions;
            eva.bookingSuccessRate = bookingSuccessRate;
            eva.unneccessaryWorkload = unneccessaryWorkload;
            eva.neccessaryWorkload = neccessaryWorkload;
            eva.plugDistributionAccepted = plugDistributionAccepted;
            eva.plugDistributionDeclined = plugDistributionDeclined;
            eva.scenario = scenario.generateScenario();

            return eva;
        }
    }
}
