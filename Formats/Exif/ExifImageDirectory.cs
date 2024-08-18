namespace MetadataExtractor.Formats.Exif
{
    public sealed class ExifImageDirectory : ExifDirectoryBase
    {
        public ExifImageDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new ExifImageDescriptor(this));
        }

        private static readonly Dictionary<int, string> _tagNameMap = [];

        static ExifImageDirectory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public override string Name => "Exif Image";
    }
}
