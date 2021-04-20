using System.Linq;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Utility;

namespace WpfApplication1.Model
{
    public class Shp
    {
        public Shp() { }
        public Shp(DesignerObj domainObjectData)
        {
            Id = domainObjectData.ObjId;
            X = domainObjectData.Geometry.First().X;
            Y = domainObjectData.Geometry.First().Y;
            TypeId = (uint)domainObjectData.ObjTypeId;
            Name = domainObjectData.Label;
        }


        public int Id { get; set; }
        public uint TypeId { get; set; }

        public string Name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return $"{Id} - '{Name}'";
        }
    }
}
