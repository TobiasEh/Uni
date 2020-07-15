using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.History;

namespace Sopro.Persistence.PersHistory
{
    public class HistoryRepository : IHistroyRepository
    {
        public List<Histroy> import()
        {
            return new List<History>();
        }
        public void export(List<History> list)
        {
            
        }
    }
}
