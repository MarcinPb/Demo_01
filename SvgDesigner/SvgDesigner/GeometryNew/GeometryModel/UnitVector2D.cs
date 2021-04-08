using System;

namespace GeometryModel
{
    public class UnitVector2D
    {
        private UnitVector2D()
        {
        }

        public static UnitVector2D Build(double x, double y)
        {
            var hypotenuse = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return new UnitVector2D
            {
                X = x / hypotenuse,
                Y = y / hypotenuse,
            };
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public override string ToString()
        {
            return $"{this.X};{this.Y}";
        }
    }
}