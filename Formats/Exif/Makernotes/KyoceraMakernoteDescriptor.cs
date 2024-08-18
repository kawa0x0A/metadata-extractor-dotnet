namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class KyoceraMakernoteDescriptor(KyoceraMakernoteDirectory directory) : TagDescriptor<KyoceraMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                KyoceraMakernoteDirectory.TagProprietaryThumbnail => GetProprietaryThumbnailDataDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetProprietaryThumbnailDataDescription()
        {
            return GetByteLengthDescription(KyoceraMakernoteDirectory.TagProprietaryThumbnail);
        }
    }
}
