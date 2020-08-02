using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class EditRushourViewModel
    {
        public List<DateTime> startTimes { get; set; } = new List<DateTime>();
        public List<DateTime> endTimes { get; set; } = new List<DateTime>();
        public List<double> spreads { get; set; } = new List<double>();
        public List<Rushhour> rushhours { get; set; } = new List<Rushhour>();
        public DateTime max { get; set; }

        public EditRushourViewModel() { }

        public EditRushourViewModel(Scenario s) 
        {
            rushhours = s.rushhours;
            foreach(Rushhour r in s.rushhours)
            {
                startTimes.Add(r.start);
                endTimes.Add(r.end);
                spreads.Add(r.spread);
                max = s.start.AddDays(s.duration);
            }
        }
    }
}
