using System.Drawing;

namespace GeometryModel
{
    public abstract class Geometry
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public Color StrokeColor { get; set; }
        public float StrokeWidthPoints { get; set; }

    }
}
