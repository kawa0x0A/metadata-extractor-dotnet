namespace MetadataExtractor.Formats.Jpeg
{
    public class JpegDnlDirectory : Directory
    {
        /// <summary>The image's height, gleaned from DNL data instead of an SOFx segment</summary>
        public const int TagImageHeight = 1;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagImageHeight, "Image Height" }
        };

        public JpegDnlDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new JpegDnlDescriptor(this));
        }

        public override string Name => "JPEG DNL";
    }
}
