namespace MetadataExtractor.Formats.Exif
{
    public class PanasonicRawIfd0Descriptor(PanasonicRawIfd0Directory directory) : TagDescriptor<PanasonicRawIfd0Directory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PanasonicRawIfd0Directory.TagPanasonicRawVersion => GetVersionBytesDescription(PanasonicRawIfd0Directory.TagPanasonicRawVersion, 2),
                PanasonicRawIfd0Directory.TagOrientation => GetOrientationDescription(PanasonicRawIfd0Directory.TagOrientation),
                _ => base.GetDescription(tagType),
            };
        }
    }
}
