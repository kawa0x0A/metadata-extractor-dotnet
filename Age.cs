using System.Text;

namespace MetadataExtractor
{
    public sealed class Age(int years, int months, int days, int hours, int minutes, int seconds)
    {
        public static Age? FromPanasonicString(string s)
        {
            ArgumentNullException.ThrowIfNull(s);

            if (s.Length != 19 || s.StartsWith("9999:99:99", StringComparison.Ordinal))
                return null;

            if (int.TryParse(s.AsSpan(0, 4), out var years) &&
                int.TryParse(s.AsSpan(5, 2), out var months) &&
                int.TryParse(s.AsSpan(8, 2), out var days) &&
                int.TryParse(s.AsSpan(11, 2), out var hours) &&
                int.TryParse(s.AsSpan(14, 2), out var minutes) &&
                int.TryParse(s.AsSpan(17, 2), out var seconds))
            {
                return new Age(years, months, days, hours, minutes, seconds);
            }

            return null;
        }

        public int Years { get; } = years;
        public int Months { get; } = months;
        public int Days { get; } = days;
        public int Hours { get; } = hours;
        public int Minutes { get; } = minutes;
        public int Seconds { get; } = seconds;

        public override string ToString()
        {
            return $"{Years:D4}:{Months:D2}:{Days:D2} {Hours:D2}:{Minutes:D2}:{Seconds:D2}";
        }

        public string ToFriendlyString()
        {
            var result = new StringBuilder();
            AppendAgePart(result, Years, "year");
            AppendAgePart(result, Months, "month");
            AppendAgePart(result, Days, "day");
            AppendAgePart(result, Hours, "hour");
            AppendAgePart(result, Minutes, "minute");
            AppendAgePart(result, Seconds, "second");
            return result.ToString();
        }

        private static void AppendAgePart(StringBuilder result, int num, string singularName)
        {
            if (num == 0)
                return;
            if (result.Length != 0)
                result.Append(' ');
            result.Append(num).Append(' ').Append(singularName);
            if (num != 1)
                result.Append('s');
        }

        private bool Equals(Age other)
        {
            return other is not null && Years == other.Years && Months == other.Months && Days == other.Days && Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is Age age && Equals(age);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Years, Months, Days, Hours, Minutes, Seconds);
        }
    }
}
