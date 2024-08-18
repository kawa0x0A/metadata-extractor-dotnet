using System.ComponentModel;
using System.Globalization;

namespace MetadataExtractor
{
    [Serializable]
    [TypeConverter(typeof(RationalConverter))]
    public readonly struct Rational(long numerator, long denominator) : IConvertible, IEquatable<Rational>
    {
        public long Denominator { get; } = denominator;

        public long Numerator { get; } = numerator;

        public double ToDouble() => Numerator == 0 ? 0.0 : Numerator / (double)Denominator;

        public float ToSingle() => Numerator == 0 ? 0.0f : Numerator / (float)Denominator;

        public byte ToByte() => (byte)ToDouble();

        public sbyte ToSByte() => (sbyte)ToDouble();

        public int ToInt32() => (int)ToDouble();

        public uint ToUInt32() => (uint)ToDouble();

        public long ToInt64() => (long)ToDouble();

        public ulong ToUInt64() => (ulong)ToDouble();

        public short ToInt16() => (short)ToDouble();

        public ushort ToUInt16() => (ushort)ToDouble();

        public decimal ToDecimal() => Denominator == 0 ? 0M : Numerator / (decimal)Denominator;
        public bool ToBoolean() => Numerator != 0 && Denominator != 0;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

        bool IConvertible.ToBoolean(IFormatProvider? provider) => ToBoolean();

        char IConvertible.ToChar(IFormatProvider? provider) => throw new NotSupportedException();

        sbyte IConvertible.ToSByte(IFormatProvider? provider) => ToSByte();

        byte IConvertible.ToByte(IFormatProvider? provider) => ToByte();

        short IConvertible.ToInt16(IFormatProvider? provider) => ToInt16();

        ushort IConvertible.ToUInt16(IFormatProvider? provider) => ToUInt16();

        int IConvertible.ToInt32(IFormatProvider? provider) => ToInt32();

        uint IConvertible.ToUInt32(IFormatProvider? provider) => ToUInt32();

        long IConvertible.ToInt64(IFormatProvider? provider) => ToInt64();

        ulong IConvertible.ToUInt64(IFormatProvider? provider) => ToUInt64();

        float IConvertible.ToSingle(IFormatProvider? provider) => ToSingle();

        double IConvertible.ToDouble(IFormatProvider? provider) => ToDouble();

        decimal IConvertible.ToDecimal(IFormatProvider? provider) => ToDecimal();

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) => throw new NotSupportedException();

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) => throw new NotSupportedException();

        public Rational Reciprocal => new(Denominator, Numerator);

        public Rational Absolute => new(Math.Abs(Numerator), Math.Abs(Denominator));

        public bool IsInteger => Denominator == 1 || (Denominator != 0 && Numerator % Denominator == 0) || (Denominator == 0 && Numerator == 0);

        public bool IsZero => Denominator == 0 || Numerator == 0;

        public bool IsPositive => !IsZero && (Numerator > 0 == Denominator > 0);

        public override string ToString() => Numerator + "/" + Denominator;

        public string ToString(IFormatProvider? provider) => Numerator.ToString(provider) + "/" + Denominator.ToString(provider);

        public string ToSimpleString(bool allowDecimal = true, IFormatProvider? provider = null)
        {
            if (Denominator == 0 && Numerator != 0)
                return ToString(provider);

            if (IsInteger)
                return ToInt64().ToString(provider);

            var simplifiedInstance = GetSimplifiedInstance();

            if (allowDecimal)
            {
                var doubleString = simplifiedInstance.ToDouble().ToString(provider);
                if (doubleString.Length < 5)
                    return doubleString;
            }

            return simplifiedInstance.ToString(provider);
        }

        public bool Equals(Rational other) => other.ToDecimal().Equals(ToDecimal());

        public bool EqualsExact(Rational other) => Denominator == other.Denominator && Numerator == other.Numerator;

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            return obj is Rational rational && Equals(rational);
        }

        public override int GetHashCode() => unchecked(Denominator.GetHashCode() * 397) ^ Numerator.GetHashCode();

        public Rational GetSimplifiedInstance()
        {
            static long GCD(long a, long b)
            {
                if (a < 0)
                    a = -a;
                if (b < 0)
                    b = -b;

                while (a != 0 && b != 0)
                {
                    if (a > b)
                        a %= b;
                    else
                        b %= a;
                }

                return a == 0 ? b : a;
            }

            var n = Numerator;
            var d = Denominator;

            if (d < 0)
            {
                n = -n;
                d = -d;
            }

            var gcd = GCD(n, d);

            return new Rational(n / gcd, d / gcd);
        }

        public static bool operator ==(Rational a, Rational b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Rational a, Rational b)
        {
            return !Equals(a, b);
        }

        private sealed class RationalConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            {
                if (sourceType == typeof(string) ||
                    sourceType == typeof(Rational) ||
                    typeof(IConvertible).IsAssignableFrom(sourceType) ||
                    (sourceType.IsArray && typeof(IConvertible).IsAssignableFrom(sourceType.GetElementType())))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            {
                if (value is null)
                    return null;

                var type = value.GetType();

                if (type == typeof(string))
                {
                    var v = ((string)value).Split('/');
                    if (v.Length == 2 && long.TryParse(v[0], out long numerator) && long.TryParse(v[1], out long denominator))
                        return new Rational(numerator, denominator);
                }

                if (type == typeof(Rational))
                    return value;

                if (type.IsArray)
                {
                    var array = (Array)value;
                    if (array.Rank == 1 && (array.Length == 1 || array.Length == 2))
                    {
                        return new Rational(
                            numerator: Convert.ToInt64(array.GetValue(0)),
                            denominator: array.Length == 2 ? Convert.ToInt64(array.GetValue(1)) : 1);
                    }
                }

                return new Rational(Convert.ToInt64(value), 1);
            }

            public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => false;
        }
    }
}
