namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class KyoceraMakernoteDirectory : Directory
    {
        public const int TagProprietaryThumbnail = 0x0001;
        public const int TagPrintImageMatchingInfo = 0x0E00;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagProprietaryThumbnail, "Proprietary Thumbnail Format Data" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" }
        };

        public KyoceraMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new KyoceraMakernoteDescriptor(this));
        }

        public override string Name => "Kyocera/Contax Makernote";
    }
}
