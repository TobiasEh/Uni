using Sopro.Interfaces.ControllerHistory;
using System.Collections.Generic;


namespace Sopro.Interfaces.Persistence
{
    public interface IEvaluationRepository
    {
        public List<IEvaluation> import(string path);
        public void export(List<IEvaluation> list, string path);
    }
}
