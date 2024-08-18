using System.Text;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusRawDevelopmentMakernoteDescriptor(OlympusRawDevelopmentMakernoteDirectory directory) : TagDescriptor<OlympusRawDevelopmentMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevVersion => GetRawDevVersionDescription(),
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevColorSpace => GetRawDevColorSpaceDescription(),
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevEngine => GetRawDevEngineDescription(),
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevNoiseReduction => GetRawDevNoiseReductionDescription(),
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevEditStatus => GetRawDevEditStatusDescription(),
                OlympusRawDevelopmentMakernoteDirectory.TagRawDevSettings => GetRawDevSettingsDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetRawDevVersionDescription()
        {
            return GetVersionBytesDescription(OlympusRawDevelopmentMakernoteDirectory.TagRawDevVersion, 4);
        }

        public string? GetRawDevColorSpaceDescription()
        {
            return GetIndexedDescription(OlympusRawDevelopmentMakernoteDirectory.TagRawDevColorSpace,
                "sRGB", "Adobe RGB", "Pro Photo RGB");
        }

        public string? GetRawDevEngineDescription()
        {
            return GetIndexedDescription(OlympusRawDevelopmentMakernoteDirectory.TagRawDevEngine,
                "High Speed", "High Function", "Advanced High Speed", "Advanced High Function");
        }

        public string? GetRawDevNoiseReductionDescription()
        {
            if (!Directory.TryGetInt(OlympusRawDevelopmentMakernoteDirectory.TagRawDevNoiseReduction, out int value))
                return null;

            if (value == 0)
                return "(none)";

            var sb = new StringBuilder();
            var v = (ushort)value;

#pragma warning disable format
            if ((v & 1) != 0) sb.Append("Noise Reduction, ");
            if (((v >> 1) & 1) != 0) sb.Append("Noise Filter, ");
            if (((v >> 2) & 1) != 0) sb.Append("Noise Filter (ISO Boost), ");
#pragma warning restore format

            return sb.ToString(0, sb.Length - 2);
        }

        public string? GetRawDevEditStatusDescription()
        {
            if (!Directory.TryGetInt(OlympusRawDevelopmentMakernoteDirectory.TagRawDevEditStatus, out int value))
                return null;

            return value switch
            {
                0 => "Original",
                1 => "Edited (Landscape)",
                6 or 8 => "Edited (Portrait)",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetRawDevSettingsDescription()
        {
            if (!Directory.TryGetInt(OlympusRawDevelopmentMakernoteDirectory.TagRawDevSettings, out int value))
                return null;

            if (value == 0)
                return "(none)";

            var sb = new StringBuilder();
            var v = (ushort)value;

#pragma warning disable format
            if ((v & 1) != 0) sb.Append("WB Color Temp, ");
            if (((v >> 1) & 1) != 0) sb.Append("WB Gray Point, ");
            if (((v >> 2) & 1) != 0) sb.Append("Saturation, ");
            if (((v >> 3) & 1) != 0) sb.Append("Contrast, ");
            if (((v >> 4) & 1) != 0) sb.Append("Sharpness, ");
            if (((v >> 5) & 1) != 0) sb.Append("Color Space, ");
            if (((v >> 6) & 1) != 0) sb.Append("High Function, ");
            if (((v >> 7) & 1) != 0) sb.Append("Noise Reduction, ");
#pragma warning restore format

            return sb.ToString(0, sb.Length - 2);
        }
    }
}
