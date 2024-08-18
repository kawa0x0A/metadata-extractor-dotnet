namespace MetadataExtractor.Formats.Adobe
{
    public class AdobeJpegDirectory : Directory
    {
        public const int TagDctEncodeVersion = 0;

        public const int TagApp14Flags0 = 1;

        public const int TagApp14Flags1 = 2;

        public const int TagColorTransform = 3;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagDctEncodeVersion, "DCT Encode Version" },
            { TagApp14Flags0, "Flags 0" },
            { TagApp14Flags1, "Flags 1" },
            { TagColorTransform, "Color Transform" }
        };

        public AdobeJpegDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new AdobeJpegDescriptor(this));
        }

        public override string Name => "Adobe JPEG";
    }
}
