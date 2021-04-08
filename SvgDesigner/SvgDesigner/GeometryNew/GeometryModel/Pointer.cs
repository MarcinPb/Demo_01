namespace GeometryModel
{
    public class Pointer
    {
        public Point2D Point { get; set; }
        public UnitVector2D Direction { get; set; }

        public override string ToString()
        {
            return $"P=({this.Point.ToString()}), D=({this.Direction.ToString()})";
        }
    }
}
