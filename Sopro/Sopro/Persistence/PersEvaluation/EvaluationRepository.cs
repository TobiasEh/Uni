using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
