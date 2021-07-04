using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class InfraDemandPatternCurve
    {
        public int DemandPatternCurveId { get; set; }
        public int DemandPatternId { get; set; }
        public double TimeFromStart { get; set; }
        public double Multiplier { get; set; }
    }
}
