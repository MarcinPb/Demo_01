using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class InfraSpecialFieldId
    {
        public int Demand_AssociatedElement { get; set; }
        public int Demand_BaseFlow { get; set; }
        public int Demand_DemandPattern { get; set; }
        public int DemandCollection { get; set; }
        public int HMIActiveTopologyIsActive { get; set; }
        public int Label { get; set; }
        public int Physical_Zone { get; set; }
        public int Scada_TargetElement { get; set; }
        public int Physical_NodeElevation { get; set; }
    }
}
