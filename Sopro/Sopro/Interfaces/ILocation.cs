using sopro2020_abgabe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro2020_abgabe.Interfaces
{
    interface ILocation
    {
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
    }
}
