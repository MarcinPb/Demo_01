using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class InfraChangeableDataLists
    {
        public List<InfraZone> ZoneDict { get; set; }
        public List<InfraDemandPattern> DemandPatternDict { get; set; }
        public List<InfraObj> InfraObjList { get; set; } = new List<InfraObj>();
        public List<InfraValue> InfraValueList { get; set; } = new List<InfraValue>();
        public List<InfraGeometry> InfraGeometryList { get; set; } = new List<InfraGeometry>();
    }
}
