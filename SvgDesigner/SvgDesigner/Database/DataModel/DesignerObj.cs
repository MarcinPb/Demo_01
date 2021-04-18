using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class DesignerObj
    {
        public int ObjId { get; set; }
        public int ObjTypeId { get; set; }
        public string Label { get; set; }
        public List<Point2D> Geometry { get; set; }
        public bool IsActive { get; set; }
        public int? ZoneId { get; set; }
        public int? AssociatedId { get; set; }
        public int? TargetId { get; set; }
        public double Xp { get; set; }
        public double Yp { get; set; }


        public Dictionary<string, object> Fields { get; set; }

    }
}
