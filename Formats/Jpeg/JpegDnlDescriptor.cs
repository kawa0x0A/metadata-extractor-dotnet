namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegDnlDescriptor(JpegDnlDirectory directory) : TagDescriptor<JpegDnlDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                JpegDnlDirectory.TagImageHeight => GetImageHeightDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetImageHeightDescription()
        {
            var value = Directory.GetString(JpegDnlDirectory.TagImageHeight);

            return value is null ? null : value + " pixels";
        }
    }
}
