﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.History
{
    public class ExpandInfrastructureSuggestion : Suggestion
    {
        public ExpandInfrastructureSuggestion(int stations, int zones) : base(stations, zones)
        {
            suggestion = "Hello, is me machine, u will need " + zones + " zones and " + stations + "stations more";
        }
    }
}
