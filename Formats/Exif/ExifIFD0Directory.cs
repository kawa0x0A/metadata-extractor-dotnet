namespace MetadataExtractor.Formats.Exif
{
    public class ExifIfd0Directory : ExifDirectoryBase
    {
        public const int TagExifSubIfdOffset = 0x8769;

        public const int TagGpsInfoOffset = 0x8825;

        public ExifIfd0Directory() : base(_tagNameMap)
        {
            SetDescriptor(new ExifIfd0Descriptor(this));
        }

        private static readonly Dictionary<int, string> _tagNameMap = [];

        static ExifIfd0Directory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public override string Name => "Exif IFD0";
    }
}
