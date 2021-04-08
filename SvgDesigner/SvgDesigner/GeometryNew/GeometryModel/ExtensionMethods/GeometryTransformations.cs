using System;

namespace GeometryModel.ExtensionMethods
{
    public static class GeometryTransformations
    {
        public static Point2D Move(this Point2D point, double x, double y)
        {
            return new Point2D(point.X + x, point.Y + y);
        }

        public static Point2D Scale(this Point2D point, double x, double y)
        {
            return new Point2D(point.X * x, point.Y * y);
        }

        public static Point2D Rotate(this Point2D point, double angleRadians)
        {
            double sin = Math.Sin(angleRadians);
            double cos = Math.Cos(angleRadians);
            double x = point.X * cos - point.Y * sin;
            double y = point.X * sin + point.Y * cos;
            Point2D result = new Point2D(x, y);
            return result;
        }

        public static double DistanceTo(this Point2D point, Point2D other)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - point.X, 2) + Math.Pow(point.Y - point.Y, 2));
            return distance;
        }

        public static double DegreesToRadians(this double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}
