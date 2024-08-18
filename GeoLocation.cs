namespace MetadataExtractor
{
    public sealed class GeoLocation(double latitude, double longitude)
    {
        public double Latitude { get; } = latitude;

        public double Longitude { get; } = longitude;

        public bool IsZero => Latitude == 0 && Longitude == 0;

        public static string DecimalToDegreesMinutesSecondsString(double value)
        {
            var dms = DecimalToDegreesMinutesSeconds(value);
            return $"{dms[0]:0.##}\u00b0 {dms[1]:0.##}' {dms[2]:0.##}\"";
        }

        public static double[] DecimalToDegreesMinutesSeconds(double value)
        {
            var d = (int)value;
            var m = Math.Abs((value % 1) * 60);
            var s = (m % 1) * 60;
            return [d, (int)m, s];
        }

        public static double? DegreesMinutesSecondsToDecimal(Rational degs, Rational mins, Rational secs, bool isNegative)
        {
            var value = Math.Abs(degs.ToDouble()) + mins.ToDouble() / 60.0d + secs.ToDouble() / 3600.0d;
            if (double.IsNaN(value))
                return null;
            if (isNegative)
                value *= -1;
            return value;
        }

        private bool Equals(GeoLocation other) => Latitude.Equals(other.Latitude) &&
                                                  Longitude.Equals(other.Longitude);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is GeoLocation location && Equals(location);
        }

        public override int GetHashCode() => unchecked((Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode());

        public override string ToString() => Latitude + ", " + Longitude;

        public string ToDmsString() => DecimalToDegreesMinutesSecondsString(Latitude) + ", " + DecimalToDegreesMinutesSecondsString(Longitude);
    }
}
