namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class PentaxMakernoteDescriptor(PentaxMakernoteDirectory directory) : TagDescriptor<PentaxMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PentaxMakernoteDirectory.TagCaptureMode => GetCaptureModeDescription(),
                PentaxMakernoteDirectory.TagQualityLevel => GetQualityLevelDescription(),
                PentaxMakernoteDirectory.TagFocusMode => GetFocusModeDescription(),
                PentaxMakernoteDirectory.TagFlashMode => GetFlashModeDescription(),
                PentaxMakernoteDirectory.TagWhiteBalance => GetWhiteBalanceDescription(),
                PentaxMakernoteDirectory.TagDigitalZoom => GetDigitalZoomDescription(),
                PentaxMakernoteDirectory.TagSharpness => GetSharpnessDescription(),
                PentaxMakernoteDirectory.TagContrast => GetContrastDescription(),
                PentaxMakernoteDirectory.TagSaturation => GetSaturationDescription(),
                PentaxMakernoteDirectory.TagIsoSpeed => GetIsoSpeedDescription(),
                PentaxMakernoteDirectory.TagColour => GetColourDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetColourDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagColour,
                1,
                "Normal", "Black & White", "Sepia");
        }

        public string? GetIsoSpeedDescription()
        {
            if (!Directory.TryGetInt(PentaxMakernoteDirectory.TagIsoSpeed, out int value))
                return null;

            return value switch
            {
                10 => "ISO 100",
                16 => "ISO 200",
                100 => "ISO 100",
                200 => "ISO 200",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetSaturationDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagSaturation,
                "Normal", "Low", "High");
        }

        public string? GetContrastDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagContrast,
                "Normal", "Low", "High");
        }

        public string? GetSharpnessDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagSharpness,
                "Normal", "Soft", "Hard");
        }

        public string? GetDigitalZoomDescription()
        {
            if (!Directory.TryGetFloat(PentaxMakernoteDirectory.TagDigitalZoom, out float value))
                return null;
            return value == 0 ? "Off" : value.ToString("0.0###########");
        }

        public string? GetWhiteBalanceDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagWhiteBalance,
                "Auto", "Daylight", "Shade", "Tungsten", "Fluorescent", "Manual");
        }

        public string? GetFlashModeDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagFlashMode,
                1,
                "Auto", "Flash On", null, "Flash Off", null, "Red-eye Reduction");
        }

        public string? GetFocusModeDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagFocusMode,
                2,
                "Custom", "Auto");
        }

        public string? GetQualityLevelDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagQualityLevel,
                "Good", "Better", "Best");
        }

        public string? GetCaptureModeDescription()
        {
            return GetIndexedDescription(PentaxMakernoteDirectory.TagCaptureMode,
                "Auto", "Night-scene", "Manual", null, "Multiple");
        }
    }
}
