using MetadataExtractor.Util;
using System.Text;

namespace MetadataExtractor
{
    public class TagDescriptor<T>(T directory) : ITagDescriptor where T : Directory
    {
        protected T Directory { get; } = directory;

        public virtual string? GetDescription(int tagType)
        {
            /*
            var obj = Directory.GetObject(tagType);
            if (obj is null)
                return null;

            if (obj is ICollection { Count: > 16 } collection)
                return $"[{collection.Count} values]";
            */

            if (Directory.TryGetString(tagType, out string? description))
            {
                return description;
            }
            else if (Directory.TryGetInt(tagType, out int value))
            {
                return value.ToString();
            }
            else if (Directory.TryGetArray(tagType, out Array? array))
            {
                return $"[{array?.Length} values]";
            }

            return null;
        }

        public static string? ConvertBytesToVersionString(int[] components, int majorDigits)
        {
            if (components is null)
                return null;

            var version = new StringBuilder();

            for (var i = 0; i < 4 && i < components.Length; i++)
            {
                if (i == majorDigits)
                    version.Append('.');
                var c = (char)components[i];
                if (c < '0')
                    c += '0';
                if (i == 0 && c == '0')
                    continue;
                version.Append(c);
            }

            if (version.Length == 0)
                return null;

            return version.ToString();
        }

        protected string? GetVersionBytesDescription(int tagType, int majorDigits)
        {
            var values = Directory.GetArray<int[]>(tagType);
            return values is null ? null : ConvertBytesToVersionString(values, majorDigits);
        }

        protected string? GetIndexedDescription(int tagType, params string?[] descriptions)
        {
            return GetIndexedDescription(tagType, 0, descriptions);
        }

        protected string? GetIndexedDescription(int tagType, int baseIndex, params string?[] descriptions)
        {
            if (!Directory.TryGetUint(tagType, out uint index))
                return null;

            var arrayIndex = index - baseIndex;

            if (arrayIndex >= 0 && arrayIndex < descriptions.Length)
            {
                var description = descriptions[arrayIndex];
                if (description is not null)
                    return description;
            }

            return "Unknown (" + index + ")";
        }

        protected string? GetBooleanDescription(int tagType, string trueValue, string falseValue)
        {
            if (!Directory.TryGetBoolean(tagType, out var value))
                return null;

            return value
                ? trueValue
                : falseValue;
        }

        protected string? GetByteLengthDescription(int tagType)
        {
            var bytes = Directory.GetArray<byte[]>(tagType);
            if (bytes is null)
                return null;
            return $"({bytes.Length} byte{(bytes.Length == 1 ? string.Empty : "s")})";
        }

        protected string? GetSimpleRational(int tagType)
        {
            if (!Directory.TryGetRational(tagType, out Rational value))
                return null;
            return value.ToSimpleString();
        }

        protected string? GetDecimalRational(int tagType, int decimalPlaces)
        {
            if (!Directory.TryGetRational(tagType, out Rational value))
                return null;
            return string.Format("{0:F" + decimalPlaces + "}", value.ToDouble());
        }

        protected string? GetFormattedInt(int tagType, string format)
        {
            if (!Directory.TryGetInt(tagType, out int value))
                return null;
            return string.Format(format, value);
        }

        protected string? GetFormattedString(int tagType, string format)
        {
            var value = Directory.GetString(tagType);
            if (value is null)
                return null;
            return string.Format(format, value);
        }

        protected string? GetEpochTimeDescription(int tagType)
        {
            return Directory.TryGetLong(tagType, out long value)
                ? DateUtil.FromUnixTime(value).ToString("ddd MMM dd HH:mm:ss zzz yyyy")
                : null;
        }

        protected string? GetBitFlagDescription(int tagType, params object?[] labels)
        {
            if (!Directory.TryGetInt(tagType, out int value))
                return null;
            var parts = new List<string>();
            var bitIndex = 0;
            while (labels.Length > bitIndex)
            {
                var labelObj = labels[bitIndex];
                if (labelObj is not null)
                {
                    var isBitSet = (value & 1) == 1;
                    if (labelObj is string[] { Length: 2 } labelPair)
                    {
                        parts.Add(labelPair[isBitSet ? 1 : 0]);
                    }
                    else if (isBitSet && labelObj is string label)
                    {
                        parts.Add(label);
                    }
                }
                value >>= 1;
                bitIndex++;
            }

            return string.Join(", ", parts);
        }

        protected string? GetStringFrom7BitBytes(int tagType)
        {
            var bytes = Directory.GetArray<byte[]>(tagType);
            if (bytes is null)
                return null;
            var length = bytes.Length;
            for (var index = 0; index < bytes.Length; index++)
            {
                var i = bytes[index] & 0xFF;
                if (i == 0 || i > 0x7F)
                {
                    length = index;
                    break;
                }
            }
            return Encoding.UTF8.GetString(bytes, 0, length);
        }

        protected string? GetStringFromUtf8Bytes(int tag)
        {
            var values = Directory.GetArray<byte[]>(tag);
            if (values is null)
                return null;

            try
            {
                return Encoding.UTF8
                    .GetString(values, 0, values.Length)
                    .Trim('\0', ' ', '\r', '\n', '\t');
            }
            catch
            {
                return null;
            }
        }

        protected string? GetRationalOrDoubleString(int tagType)
        {
            if (Directory.TryGetRational(tagType, out Rational rational))
                return rational.ToSimpleString();

            if (Directory.TryGetDouble(tagType, out double d))
                return d.ToString("0.###");

            return null;
        }

        protected static string GetFStopDescription(double fStop) => $"f/{Math.Round(fStop, 1, MidpointRounding.AwayFromZero):0.0}";

        protected static string GetFocalLengthDescription(double mm) => $"{mm:0.#} mm";

        protected string? GetLensSpecificationDescription(int tagId)
        {
            var values = Directory.GetArray<Rational[]>(tagId);

            if (values is null || values.Length != 4 || values[0].IsZero && values[2].IsZero)
                return null;

            var sb = new StringBuilder();

            if (values[0] == values[1])
                sb.Append(values[0].ToSimpleString()).Append("mm");
            else
                sb.Append(values[0].ToSimpleString()).Append('-').Append(values[1].ToSimpleString()).Append("mm");

            if (!values[2].IsZero)
            {
                sb.Append(' ');

                if (values[2] == values[3])
                    sb.Append(GetFStopDescription(values[2].ToDouble()));
                else
                    sb.Append("f/")
                      .Append(Math.Round(values[2].ToDouble(), 1, MidpointRounding.AwayFromZero).ToString("0.0"))
                      .Append('-')
                      .Append(Math.Round(values[3].ToDouble(), 1, MidpointRounding.AwayFromZero).ToString("0.0"));
            }

            return sb.ToString();
        }

        protected string? GetOrientationDescription(int tag)
        {
            return GetIndexedDescription(tag, 1,
                "Top, left side (Horizontal / normal)",
                "Top, right side (Mirror horizontal)",
                "Bottom, right side (Rotate 180)", "Bottom, left side (Mirror vertical)",
                "Left side, top (Mirror horizontal and rotate 270 CW)",
                "Right side, top (Rotate 90 CW)",
                "Right side, bottom (Mirror horizontal and rotate 90 CW)",
                "Left side, bottom (Rotate 270 CW)");
        }

        protected string? GetShutterSpeedDescription(int tagId)
        {
            if (!Directory.TryGetFloat(tagId, out float apexValue))
                return null;

            if (apexValue <= 1)
            {
                var apexPower = (float)(1 / Math.Exp(apexValue * Math.Log(2)));
                var apexPower10 = (long)Math.Round(apexPower * 10.0);
                var fApexPower = apexPower10 / 10.0f;
                return fApexPower + " sec";
            }
            else
            {
                var apexPower = (int)Math.Exp(apexValue * Math.Log(2));
                return "1/" + apexPower + " sec";
            }
        }

        protected string? GetEncodedTextDescription(int tagType)
        {
            var commentBytes = Directory.GetArray<byte[]>(tagType);

            if (commentBytes is null)
                return null;

            if (commentBytes.Length == 0)
                return string.Empty;

            var encodingMap = new Dictionary<string, Encoding>
            {
                ["ASCII"] = Encoding.ASCII,
                ["UTF8"] = Encoding.UTF8,
#pragma warning disable SYSLIB0001 // Type or member is obsolete
                ["UTF7"] = Encoding.UTF7,
#pragma warning restore SYSLIB0001 // Type or member is obsolete
                ["UTF32"] = Encoding.UTF32,
                ["UNICODE"] = Encoding.BigEndianUnicode,
            };

            try
            {
                encodingMap["JIS"] = Encoding.GetEncoding("Shift-JIS");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                if (commentBytes.Length >= 8)
                {
                    var idCode = Encoding.UTF8.GetString(commentBytes, 0, 8).TrimEnd('\0', ' ');
                    if (encodingMap.TryGetValue(idCode, out var encoding))
                    {
                        var text = encoding.GetString(commentBytes, 8, commentBytes.Length - 8);
                        if (encoding == Encoding.ASCII)
                            text = text.Trim('\0', ' ');
                        return text;
                    }
                }

                return Encoding.UTF8.GetString(commentBytes, 0, commentBytes.Length).Trim('\0', ' ');
            }
            catch
            {
                return null;
            }
        }
    }
}
