using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class InfraGeometry
    {
        public int GeometryId { get; set; }
        public int ValueId { get; set; }
        public int OrderNo { get; set; }
        public double Xp { get; set; }
        public double Yp { get; set; }
    }
}
