namespace GeometryReader.Extensions
{
    public static class ConversionExtensions
    {
        public const double FeetToMetersRatio = 0.3048;
        public const double MetersToFeetRatio = 3.2808399;

        public static double FeetToMeters(this double feet)
        {
            return feet * FeetToMetersRatio;
        }

        public static double MetersToFeet(this double feet)
        {
            return feet * MetersToFeetRatio;
        }
    }
}
