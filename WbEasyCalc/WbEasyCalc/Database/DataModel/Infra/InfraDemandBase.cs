using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class InfraDemandBase
    {
        public int DemandBaseId { get; set; }
        public int ValueId { get; set; }
        public double DemandBase { get; set; }
        public int DemandPatternId { get; set; }
    }
}
