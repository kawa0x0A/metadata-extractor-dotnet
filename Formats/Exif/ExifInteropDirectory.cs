namespace MetadataExtractor.Formats.Exif
{
    public class ExifInteropDirectory : ExifDirectoryBase
    {
        private static readonly Dictionary<int, string> _tagNameMap = [];

        static ExifInteropDirectory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public ExifInteropDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new ExifInteropDescriptor(this));
        }

        public override string Name => "Interoperability";
    }
}
