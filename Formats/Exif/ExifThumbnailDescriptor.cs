namespace MetadataExtractor.Formats.Exif
{
    public sealed class ExifThumbnailDescriptor : ExifDescriptorBase<ExifThumbnailDirectory>
    {
        public ExifThumbnailDescriptor(ExifThumbnailDirectory directory) : base(directory)
        {
        }

        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                ExifThumbnailDirectory.TagThumbnailOffset => GetThumbnailOffsetDescription(),
                ExifThumbnailDirectory.TagThumbnailLength => GetThumbnailLengthDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetThumbnailLengthDescription()
        {
            var value = Directory.GetString(ExifThumbnailDirectory.TagThumbnailLength);
            return value is null ? null : value + " bytes";
        }

        public string? GetThumbnailOffsetDescription()
        {
            var offset = Directory.AdjustedThumbnailOffset;

            return offset is null ? null : offset + " bytes";
        }
    }
}
