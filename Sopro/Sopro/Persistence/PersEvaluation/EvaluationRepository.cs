using Sopro.Interfaces.ControllerHistory;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;

namespace Sopro.Persistence.PersEvaluation
{
    public class EvaluationRepository : IEvaluationRepository
    {
        public List<IEvaluation> import(string path)
        {
            return new List<IEvaluation>();
        }
        public void export(List<IEvaluation> list, string path)
        {
            
        }
    }
}
