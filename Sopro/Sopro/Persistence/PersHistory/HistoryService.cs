using Sopro.Interfaces.PersistenceController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersHistory
{
    public class HistoryService : HistoryRepository, IHistroyService
    {
        public List<History> import()
        {
            return new List<History>();
        }
        public void export(List<History> list)
        {

        }
    }
}
