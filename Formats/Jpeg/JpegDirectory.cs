namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegDirectory : Directory
    {
        public const int TagCompressionType = -3;

        public const int TagDataPrecision = 0;

        public const int TagImageHeight = 1;

        public const int TagImageWidth = 3;

        public const int TagNumberOfComponents = 5;

        public const int TagComponentData1 = 6;

        public const int TagComponentData2 = 7;

        public const int TagComponentData3 = 8;

        public const int TagComponentData4 = 9;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagCompressionType, "Compression Type" },
            { TagDataPrecision, "Data Precision" },
            { TagImageWidth, "Image Width" },
            { TagImageHeight, "Image Height" },
            { TagNumberOfComponents, "Number of Components" },
            { TagComponentData1, "Component 1" },
            { TagComponentData2, "Component 2" },
            { TagComponentData3, "Component 3" },
            { TagComponentData4, "Component 4" }
        };

        public JpegDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new JpegDescriptor(this));
        }

        public override string Name => "JPEG";

        public JpegComponent? GetComponent(int componentNumber)
        {
            var tagType = TagComponentData1 + componentNumber;

            return GetJpegComponent(tagType);
        }

        public int GetImageWidth()
        {
            return GetInt(TagImageWidth);
        }

        public int GetImageHeight()
        {
            return GetInt(TagImageHeight);
        }

        public int GetNumberOfComponents()
        {
            return GetInt(TagNumberOfComponents);
        }
    }
}
