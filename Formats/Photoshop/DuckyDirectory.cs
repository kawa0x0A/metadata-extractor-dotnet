namespace MetadataExtractor.Formats.Photoshop
{
    public class DuckyDirectory : Directory
    {
        public const int TagQuality = 1;
        public const int TagComment = 2;
        public const int TagCopyright = 3;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagQuality, "Quality" },
            { TagComment, "Comment" },
            { TagCopyright, "Copyright" }
        };

        public DuckyDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new TagDescriptor<DuckyDirectory>(this));
        }

        public override string Name => "Ducky";
    }
}
