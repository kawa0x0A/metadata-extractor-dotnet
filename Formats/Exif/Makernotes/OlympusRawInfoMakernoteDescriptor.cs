namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusRawInfoMakernoteDescriptor(OlympusRawInfoMakernoteDirectory directory) : TagDescriptor<OlympusRawInfoMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                OlympusRawInfoMakernoteDirectory.TagRawInfoVersion => GetVersionBytesDescription(OlympusRawInfoMakernoteDirectory.TagRawInfoVersion, 4),
                OlympusRawInfoMakernoteDirectory.TagColorMatrix2 => GetColorMatrix2Description(),
                OlympusRawInfoMakernoteDirectory.TagYCbCrCoefficients => GetYCbCrCoefficientsDescription(),
                OlympusRawInfoMakernoteDirectory.TagLightSource => GetOlympusLightSourceDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetColorMatrix2Description()
        {
            var values = Directory.GetArray<short[]>(OlympusRawInfoMakernoteDirectory.TagColorMatrix2);

            if (values is null)
                return null;

            return string.Join(" ", values.Select(b => b.ToString()).ToArray());
        }

        public string? GetYCbCrCoefficientsDescription()
        {
            var values = Directory.GetArray<ushort[]>(OlympusRawInfoMakernoteDirectory.TagYCbCrCoefficients);

            if (values is null)
                return null;

            var ret = new Rational[values.Length / 2];
            for (var i = 0; i < values.Length / 2; i++)
            {
                ret[i] = new Rational(values[2 * i], values[2 * i + 1]);
            }

            return string.Join(" ", ret.Select(r => r.ToDecimal().ToString()).ToArray());
        }

        public string? GetOlympusLightSourceDescription()
        {
            if (!Directory.TryGetUshort(OlympusRawInfoMakernoteDirectory.TagLightSource, out ushort value))
                return null;

            return value switch
            {
                0 => "Unknown",
                16 => "Shade",
                17 => "Cloudy",
                18 => "Fine Weather",
                20 => "Tungsten (Incandescent)",
                22 => "Evening Sunlight",
                33 => "Daylight Fluorescent",
                34 => "Day White Fluorescent",
                35 => "Cool White Fluorescent",
                36 => "White Fluorescent",
                256 => "One Touch White Balance",
                512 => "Custom 1-4",
                _ => "Unknown (" + value + ")",
            };
        }
    }
}
