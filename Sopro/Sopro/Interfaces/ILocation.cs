using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoPro.Interfaces
{
    interface ILocation
    {
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
    }
}
