namespace MetadataExtractor.Formats.Exif
{
    public class PanasonicRawDistortionDescriptor(PanasonicRawDistortionDirectory directory) : TagDescriptor<PanasonicRawDistortionDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PanasonicRawDistortionDirectory.TagDistortionParam02 => GetDistortionParam02Description(),
                PanasonicRawDistortionDirectory.TagDistortionParam04 => GetDistortionParam04Description(),
                PanasonicRawDistortionDirectory.TagDistortionScale => GetDistortionScaleDescription(),
                PanasonicRawDistortionDirectory.TagDistortionCorrection => GetDistortionCorrectionDescription(),
                PanasonicRawDistortionDirectory.TagDistortionParam08 => GetDistortionParam08Description(),
                PanasonicRawDistortionDirectory.TagDistortionParam09 => GetDistortionParam09Description(),
                PanasonicRawDistortionDirectory.TagDistortionParam11 => GetDistortionParam11Description(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetDistortionParam02Description()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionParam02, out short value))
                return null;

            return new Rational(value, 32678).ToString();
        }

        public string? GetDistortionParam04Description()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionParam04, out short value))
                return null;

            return new Rational(value, 32678).ToString();
        }

        public string? GetDistortionScaleDescription()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionScale, out short value))
                return null;

            return (1 / (1 + value / 32768)).ToString();
        }

        public string? GetDistortionCorrectionDescription()
        {
            if (!Directory.TryGetInt(PanasonicRawDistortionDirectory.TagDistortionCorrection, out int value))
                return null;

            var mask = 0x000f;
            return (value & mask) switch
            {
                0 => "Off",
                1 => "On",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetDistortionParam08Description()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionParam08, out short value))
                return null;

            return new Rational(value, 32678).ToString();
        }

        public string? GetDistortionParam09Description()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionParam09, out short value))
                return null;

            return new Rational(value, 32678).ToString();
        }

        public string? GetDistortionParam11Description()
        {
            if (!Directory.TryGetShort(PanasonicRawDistortionDirectory.TagDistortionParam11, out short value))
                return null;

            return new Rational(value, 32678).ToString();
        }

    }
}
