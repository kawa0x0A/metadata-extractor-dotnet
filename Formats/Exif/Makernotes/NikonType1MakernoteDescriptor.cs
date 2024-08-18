namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class NikonType1MakernoteDescriptor(NikonType1MakernoteDirectory directory) : TagDescriptor<NikonType1MakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                NikonType1MakernoteDirectory.TagQuality => GetQualityDescription(),
                NikonType1MakernoteDirectory.TagColorMode => GetColorModeDescription(),
                NikonType1MakernoteDirectory.TagImageAdjustment => GetImageAdjustmentDescription(),
                NikonType1MakernoteDirectory.TagCcdSensitivity => GetCcdSensitivityDescription(),
                NikonType1MakernoteDirectory.TagWhiteBalance => GetWhiteBalanceDescription(),
                NikonType1MakernoteDirectory.TagFocus => GetFocusDescription(),
                NikonType1MakernoteDirectory.TagDigitalZoom => GetDigitalZoomDescription(),
                NikonType1MakernoteDirectory.TagConverter => GetConverterDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetConverterDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagConverter,
                "None", "Fisheye converter");
        }

        public string? GetDigitalZoomDescription()
        {
            if (!Directory.TryGetRational(NikonType1MakernoteDirectory.TagDigitalZoom, out Rational value))
                return null;
            return value.Numerator == 0
                ? "No digital zoom"
                : value.ToSimpleString() + "x digital zoom";
        }

        public string? GetFocusDescription()
        {
            if (!Directory.TryGetRational(NikonType1MakernoteDirectory.TagFocus, out Rational value))
                return null;
            return value.Numerator == 1 && value.Denominator == 0
                ? "Infinite"
                : value.ToSimpleString();
        }

        public string? GetWhiteBalanceDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagWhiteBalance,
                "Auto", "Preset", "Daylight", "Incandescence", "Florescence", "Cloudy", "SpeedLight");
        }

        public string? GetCcdSensitivityDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagCcdSensitivity,
                "ISO80", null, "ISO160", null, "ISO320", "ISO100");
        }

        public string? GetImageAdjustmentDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagImageAdjustment,
                "Normal", "Bright +", "Bright -", "Contrast +", "Contrast -");
        }

        public string? GetColorModeDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagColorMode,
                1,
                "Color", "Monochrome");
        }

        public string? GetQualityDescription()
        {
            return GetIndexedDescription(NikonType1MakernoteDirectory.TagQuality,
                1,
                "VGA Basic", "VGA Normal", "VGA Fine", "SXGA Basic", "SXGA Normal", "SXGA Fine");
        }
    }
}
