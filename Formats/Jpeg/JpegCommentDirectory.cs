namespace MetadataExtractor.Formats.Jpeg
{
    public class JpegCommentDirectory : Directory
    {
        public const int TagComment = 0;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagComment, "JPEG Comment" }
        };

        public JpegCommentDirectory(StringValue comment) : base(_tagNameMap)
        {
            SetDescriptor(new JpegCommentDescriptor(this));
            Set(TagComment, comment);
        }

        public override string Name => "JpegComment";
    }
}
