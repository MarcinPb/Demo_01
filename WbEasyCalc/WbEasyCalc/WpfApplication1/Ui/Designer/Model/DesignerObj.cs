using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1.Ui.Designer.Model
{
    public class DesignerObj : ICloneable
    {
        public int ObjId { get; set; }
        public int ObjTypeId { get; set; }
        public string Label { get; set; }
        public List<Point> Geometry { get; set; }
        public bool IsActive { get; set; }
        public int? ZoneId { get; set; }
        public int? AssociatedId { get; set; }
        public int? TargetId { get; set; }
        public double Xp { get; set; }
        public double Yp { get; set; }


        public Dictionary<string, object> Fields { get; set; }

        public object Clone()
        {
            return new DesignerObj
            {
                ObjId = ObjId,
                ObjTypeId = ObjTypeId,
                Label = Label,
                Geometry = Geometry.Select(x => new Point(x.X, x.Y)).ToList(),
                IsActive = IsActive,
                ZoneId = ZoneId,
                AssociatedId = AssociatedId,
                TargetId = TargetId,
                Xp = Xp,
                Yp = Yp,

                Fields = null,
            };
        }
    }
}
