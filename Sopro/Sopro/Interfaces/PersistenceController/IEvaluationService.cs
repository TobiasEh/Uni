using System.Collections.Generic;
using Sopro.Interfaces.ControllerHistory;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IEvaluationService
    {
        public List<IEvaluation> import(string path);
        public void export(List<IEvaluation> list, string path);
    }
}
