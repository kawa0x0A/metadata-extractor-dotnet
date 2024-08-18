namespace MetadataExtractor.Formats.Exif
{
    public sealed class ExifThumbnailDirectory : ExifDirectoryBase
    {
        public const int TagThumbnailOffset = 0x0201;

        public const int TagThumbnailLength = 0x0202;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagThumbnailOffset, "Thumbnail Offset" },
            { TagThumbnailLength, "Thumbnail Length" }
        };

        static ExifThumbnailDirectory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public ExifThumbnailDirectory(int exifStartOffset) : base(_tagNameMap)
        {
            SetDescriptor(new ExifThumbnailDescriptor(this));

            ExifStartOffset = exifStartOffset;
        }

        public override string Name => "Exif Thumbnail";

        public int ExifStartOffset { get; }

        public int? AdjustedThumbnailOffset => this.TryGetInt(TagThumbnailOffset, out int offset) ? offset + ExifStartOffset : null;
    }
}
