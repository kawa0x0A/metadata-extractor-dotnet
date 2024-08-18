namespace MetadataExtractor.Formats.Exif
{
    public class ExifSubIfdDirectory : ExifDirectoryBase
    {
        public const int TagInteropOffset = 0xA005;

        public ExifSubIfdDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new ExifSubIfdDescriptor(this));
        }

        private static readonly Dictionary<int, string> _tagNameMap = [];

        static ExifSubIfdDirectory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public override string Name => "Exif SubIFD";
    }
}
