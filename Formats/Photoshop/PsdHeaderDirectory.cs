namespace MetadataExtractor.Formats.Photoshop
{
    public class PsdHeaderDirectory : Directory
    {
        public const int TagChannelCount = 1;

        public const int TagImageHeight = 2;

        public const int TagImageWidth = 3;

        public const int TagBitsPerChannel = 4;

        public const int TagColorMode = 5;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagChannelCount, "Channel Count" },
            { TagImageHeight, "Image Height" },
            { TagImageWidth, "Image Width" },
            { TagBitsPerChannel, "Bits Per Channel" },
            { TagColorMode, "Color Mode" }
        };

        public PsdHeaderDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new PsdHeaderDescriptor(this));
        }

        public override string Name => "PSD Header";
    }
}
