using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models.ViewModels
{
    public class BarChartModel
    {
        public string Name { get; set; }
        public int NumAssigned { get; set; }
        public int NumClosed { get; set; }
    }
}
