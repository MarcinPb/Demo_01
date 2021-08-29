using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class DemandSettingObj
    {
        public int ObjId { get; set; }
        public int DemandPatternId { get; set; }
        public double DemandBaseValue { get; set; }
        public bool IsExcluded { get; set; }
    }
}
