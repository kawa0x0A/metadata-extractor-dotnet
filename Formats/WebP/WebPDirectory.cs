namespace MetadataExtractor.Formats.WebP
{
    public class WebPDirectory : Directory
    {
        public const int TagImageHeight = 1;
        public const int TagImageWidth = 2;
        public const int TagHasAlpha = 3;
        public const int TagIsAnimation = 4;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagImageHeight, "Image Height" },
            { TagImageWidth, "Image Width" },
            { TagHasAlpha, "Has Alpha" },
            { TagIsAnimation, "Is Animation" }
        };

        public WebPDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new WebPDescriptor(this));
        }

        public override string Name => "WebP";
    }
}
