namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusFocusInfoMakernoteDescriptor(OlympusFocusInfoMakernoteDirectory directory) : TagDescriptor<OlympusFocusInfoMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                OlympusFocusInfoMakernoteDirectory.TagFocusInfoVersion => GetFocusInfoVersionDescription(),
                OlympusFocusInfoMakernoteDirectory.TagAutoFocus => GetAutoFocusDescription(),
                OlympusFocusInfoMakernoteDirectory.TagFocusDistance => GetFocusDistanceDescription(),
                OlympusFocusInfoMakernoteDirectory.TagAfPoint => GetAfPointDescription(),
                OlympusFocusInfoMakernoteDirectory.TagExternalFlash => GetExternalFlashDescription(),
                OlympusFocusInfoMakernoteDirectory.TagExternalFlashBounce => GetExternalFlashBounceDescription(),
                OlympusFocusInfoMakernoteDirectory.TagExternalFlashZoom => GetExternalFlashZoomDescription(),
                OlympusFocusInfoMakernoteDirectory.TagManualFlash => GetManualFlashDescription(),
                OlympusFocusInfoMakernoteDirectory.TagMacroLed => GetMacroLedDescription(),
                OlympusFocusInfoMakernoteDirectory.TagSensorTemperature => GetSensorTemperatureDescription(),
                OlympusFocusInfoMakernoteDirectory.TagImageStabilization => GetImageStabilizationDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetFocusInfoVersionDescription()
        {
            return GetVersionBytesDescription(OlympusFocusInfoMakernoteDirectory.TagFocusInfoVersion, 4);
        }

        public string? GetAutoFocusDescription()
        {
            return GetIndexedDescription(OlympusFocusInfoMakernoteDirectory.TagAutoFocus,
                "Off", "On");
        }

        public string GetFocusDistanceDescription()
        {
            if (!Directory.TryGetRational(OlympusFocusInfoMakernoteDirectory.TagFocusDistance, out Rational value))
                return "inf";
            if (value.Numerator == 0xFFFFFFFF || value.Numerator == 0x00000000)
                return "inf";

            return value.Numerator / 1000.0 + " m";
        }

        public string? GetAfPointDescription()
        {
            if (!Directory.TryGetShort(OlympusFocusInfoMakernoteDirectory.TagAfPoint, out short value))
                return null;

            return value.ToString();
        }

        public string? GetExternalFlashDescription()
        {
            var values = Directory.GetArray<ushort[]>(OlympusFocusInfoMakernoteDirectory.TagExternalFlash);

            if (values is null || values.Length < 2)
                return null;

            var join = $"{values[0]} {values[1]}";

            return join switch
            {
                "0 0" => "Off",
                "1 0" => "On",
                _ => "Unknown (" + join + ")",
            };
        }

        public string? GetExternalFlashBounceDescription()
        {
            return GetIndexedDescription(OlympusFocusInfoMakernoteDirectory.TagExternalFlashBounce,
                "Bounce or Off", "Direct");
        }

        public string? GetExternalFlashZoomDescription()
        {
            var values = Directory.GetArray<ushort[]>(OlympusFocusInfoMakernoteDirectory.TagExternalFlashZoom);

            if (values is null)
            {
                if (!Directory.TryGetShort(OlympusFocusInfoMakernoteDirectory.TagExternalFlashZoom, out short value))
                    return null;

                values = new ushort[1];
                values[0] = (ushort)value;
            }

            if (values.Length == 0)
                return null;

            var join = $"{values[0]}" + (values.Length > 1 ? $"{values[1]}" : "");

            return join switch
            {
                "0" => "Off",
                "1" => "On",
                "0 0" => "Off",
                "1 0" => "On",
                _ => "Unknown (" + join + ")",
            };
        }

        public string? GetManualFlashDescription()
        {
            var values = Directory.GetArray<short[]>(OlympusFocusInfoMakernoteDirectory.TagManualFlash);

            if (values is null)
                return null;

            if (values[0] == 0)
                return "Off";

            if (values[1] == 1)
                return "Full";
            return "On (1/" + values[1] + " strength)";
        }

        public string? GetMacroLedDescription()
        {
            return GetIndexedDescription(OlympusFocusInfoMakernoteDirectory.TagMacroLed,
                "Off", "On");
        }

        public string? GetSensorTemperatureDescription()
        {
            return Directory.GetString(OlympusFocusInfoMakernoteDirectory.TagSensorTemperature);
        }

        public string? GetImageStabilizationDescription()
        {
            var values = Directory.GetArray<byte[]>(OlympusFocusInfoMakernoteDirectory.TagImageStabilization);
            if (values is null)
                return null;

            if ((values[0] | values[1] | values[2] | values[3]) == 0x0)
                return "Off";
            return "On, " + ((values[43] & 1) > 0 ? "Mode 1" : "Mode 2");
        }
    }
}
