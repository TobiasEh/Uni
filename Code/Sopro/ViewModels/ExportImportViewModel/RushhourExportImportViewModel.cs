using Sopro.Models.Simulation;
using System;

namespace Sopro.ViewModels.ExportImportViewModel
{
    public class RushhourExportImportViewModel
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public double spread { get; set; } = 1;

        public RushhourExportImportViewModel() { }

        public RushhourExportImportViewModel(Rushhour rushhour)
        {
            start = rushhour.start;
            end = rushhour.end;
            spread = rushhour.spread;
        }

        public Rushhour generateRushhour()
        {
            Rushhour r = new Rushhour();
            r.spread = spread;
            r.start = start;
            r.end = end;

            return r;
        }
    }
}
