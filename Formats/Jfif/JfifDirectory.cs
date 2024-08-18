namespace MetadataExtractor.Formats.Jfif
{
    public sealed class JfifDirectory : Directory
    {
        public const int TagVersion = 5;

        public const int TagUnits = 7;

        public const int TagResX = 8;
        public const int TagResY = 10;
        public const int TagThumbWidth = 12;
        public const int TagThumbHeight = 13;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagVersion, "Version" },
            { TagUnits, "Resolution Units" },
            { TagResY, "Y Resolution" },
            { TagResX, "X Resolution" },
            { TagThumbWidth, "Thumbnail Width Pixels" },
            { TagThumbHeight, "Thumbnail Height Pixels" }
        };

        public JfifDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new JfifDescriptor(this));
        }

        public override string Name => "JFIF";

        public int GetVersion()
        {
            return this.GetInt(TagVersion);
        }

        public int GetResUnits()
        {
            return this.GetInt(TagUnits);
        }

        public int GetImageWidth()
        {
            return this.GetInt(TagResY);
        }

        public int GetImageHeight()
        {
            return this.GetInt(TagResX);
        }
    }
}
