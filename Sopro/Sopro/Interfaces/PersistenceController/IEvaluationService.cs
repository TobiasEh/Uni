using System.Collections.Generic;
using Sopro.Interfaces.ControllerHistory;
using Sopro.Interfaces.Persistence;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IEvaluationService : IEvaluationRepository
    {
        public List<IEvaluation> import(string path);

        public void export(List<IEvaluation> list, string path);
    }
}
