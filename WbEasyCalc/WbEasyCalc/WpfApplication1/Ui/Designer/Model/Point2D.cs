using System;

namespace Database.DataModel.Infra
{
    [Serializable]
    public class Point2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        //public override string ToString()
        //{
        //    return $"{X};{Y}";
        //}
    }
}
