namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class SigmaMakernoteDescriptor(SigmaMakernoteDirectory directory) : TagDescriptor<SigmaMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                SigmaMakernoteDirectory.TagExposureMode => GetExposureModeDescription(),
                SigmaMakernoteDirectory.TagMeteringMode => GetMeteringModeDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        private string? GetMeteringModeDescription()
        {
            var value = Directory.GetString(SigmaMakernoteDirectory.TagMeteringMode);
            if (string.IsNullOrEmpty(value))
                return null;

            return (value![0]) switch
            {
                '8' => "Multi Segment",
                'A' => "Average",
                'C' => "Center Weighted Average",
                _ => value,
            };
        }

        private string? GetExposureModeDescription()
        {
            var value = Directory.GetString(SigmaMakernoteDirectory.TagExposureMode);
            if (string.IsNullOrEmpty(value))
                return null;

            return (value![0]) switch
            {
                'A' => "Aperture Priority AE",
                'M' => "Manual",
                'P' => "Program AE",
                'S' => "Shutter Speed Priority AE",
                _ => value,
            };
        }
    }
}
