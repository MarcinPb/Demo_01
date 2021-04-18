using GeometryModel;
using System.Linq;
using WpfApplication1.Utility;

namespace WpfApplication1.ShapeModel
{
    public class Shp 
    {
        public Shp() { }
        public Shp(DomainObjectData domainObjectData) 
        {
            Id = domainObjectData.ID;
            X = domainObjectData.Geometry.First().X;
            Y = domainObjectData.Geometry.First().Y;
            TypeId = (uint)
                (domainObjectData.ObjectType == ObjectTypes.Junction ? 2 : 
                (domainObjectData.ObjectType == ObjectTypes.Pipe ? 6 : 7)
                );
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
