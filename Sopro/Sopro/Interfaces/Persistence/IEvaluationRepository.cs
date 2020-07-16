using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IEvaluationRepository
    {
        public List<IEvaluation> import(string path);
        public void export(List<IEvaluation> list, string path);
    }
}
