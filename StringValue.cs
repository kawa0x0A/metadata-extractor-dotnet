using System.Text;

namespace MetadataExtractor
{
    public readonly struct StringValue(byte[] bytes, Encoding? encoding) : IConvertible
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public byte[] Bytes { get; } = bytes;

        public Encoding? Encoding { get; } = encoding;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

        string IConvertible.ToString(IFormatProvider? provider) => ToString();

        double IConvertible.ToDouble(IFormatProvider? provider) => double.Parse(ToString());

        decimal IConvertible.ToDecimal(IFormatProvider? provider) => decimal.Parse(ToString());

        float IConvertible.ToSingle(IFormatProvider? provider) => float.Parse(ToString());

        bool IConvertible.ToBoolean(IFormatProvider? provider) => bool.Parse(ToString());

        byte IConvertible.ToByte(IFormatProvider? provider) => byte.Parse(ToString());

        char IConvertible.ToChar(IFormatProvider? provider)
        {
            var s = ToString();
            if (s.Length != 1)
                throw new FormatException();
            return s[0];
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) => DateTime.Parse(ToString());

        short IConvertible.ToInt16(IFormatProvider? provider) => short.Parse(ToString());

        int IConvertible.ToInt32(IFormatProvider? provider)
        {
            try
            {
                return int.Parse(ToString());
            }
            catch (Exception)
            {
                long val = 0;
                foreach (var b in Bytes)
                {
                    val <<= 8;
                    val += b;
                }
                return (int)val;
            }
        }

        long IConvertible.ToInt64(IFormatProvider? provider) => long.Parse(ToString());

        sbyte IConvertible.ToSByte(IFormatProvider? provider) => sbyte.Parse(ToString());

        ushort IConvertible.ToUInt16(IFormatProvider? provider) => ushort.Parse(ToString());

        uint IConvertible.ToUInt32(IFormatProvider? provider)
        {
            try
            {
                return uint.Parse(ToString());
            }
            catch (Exception)
            {
                ulong val = 0;
                foreach (var b in Bytes)
                {
                    val <<= 8;
                    val += b;
                }
                return (uint)val;
            }
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider) => ulong.Parse(ToString());

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) => Convert.ChangeType(ToString(), conversionType, provider);

        public override string ToString() => ToString(Encoding ?? DefaultEncoding);

        public string ToString(Encoding encoder) => encoder.GetString(Bytes, 0, Bytes.Length);

        public string ToString(int index, int count) => (Encoding ?? DefaultEncoding).GetString(Bytes, index, count);
    }
}
