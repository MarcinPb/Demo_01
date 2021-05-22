using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryModel
{
    [Serializable]
    public class WaterGemsData
    {
        public WaterGemsData() {}

        public List<DomainObjectData> DomainObjectDataList { get; set; }
        public Dictionary<int, string> ZoneDict { get; set; }
        public Dictionary<int, string> ObjTypeDict { get; set; }

    }
}
