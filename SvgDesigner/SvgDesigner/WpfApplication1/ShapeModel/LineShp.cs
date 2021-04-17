using GeometryModel;
using System.Collections.Generic;

namespace WpfApplication1.ShapeModel
{
    public class LineShp : Shp
    {
        //public uint ConnTypeId { get; set; }

        public double X2 { get; set; }
        public double Y2 { get; set; }

        public List<Point2D> Path { get; set; }

    }
}
