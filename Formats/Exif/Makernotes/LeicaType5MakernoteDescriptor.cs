namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class LeicaType5MakernoteDescriptor : TagDescriptor<LeicaType5MakernoteDirectory>
    {
        public LeicaType5MakernoteDescriptor(LeicaType5MakernoteDirectory directory) : base(directory)
        {
        }

        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                LeicaType5MakernoteDirectory.TagExposureMode => GetExposureModeDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetExposureModeDescription()
        {
            var values = Directory.GetArray<byte[]>(LeicaType5MakernoteDirectory.TagExposureMode);

            if (values == null || values.Length < 4)
                return null;

            var join = $"{values[0]} {values[1]} {values[2]} {values[3]}";
            var ret = join switch
            {
                "0 0 0 0" => "Program AE",
                "1 0 0 0" => "Aperture-priority AE",
                "1 1 0 0" => "Aperture-priority AE (1)",
                "2 0 0 0" => "Shutter speed priority AE",  // guess
                "3 0 0 0" => "Manual",
                _ => "Unknown (" + join + ")",
            };
            return ret;
        }
    }
}
