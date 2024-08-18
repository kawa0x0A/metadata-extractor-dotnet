namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class NikonType1MakernoteDirectory : Directory
    {
        public const int TagUnknown1 = 0x0002;
        public const int TagQuality = 0x0003;
        public const int TagColorMode = 0x0004;
        public const int TagImageAdjustment = 0x0005;
        public const int TagCcdSensitivity = 0x0006;
        public const int TagWhiteBalance = 0x0007;
        public const int TagFocus = 0x0008;
        public const int TagUnknown2 = 0x0009;
        public const int TagDigitalZoom = 0x000A;
        public const int TagConverter = 0x000B;
        public const int TagUnknown3 = 0x0F00;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagCcdSensitivity, "CCD Sensitivity" },
            { TagColorMode, "Color Mode" },
            { TagDigitalZoom, "Digital Zoom" },
            { TagConverter, "Fisheye Converter" },
            { TagFocus, "Focus" },
            { TagImageAdjustment, "Image Adjustment" },
            { TagQuality, "Quality" },
            { TagUnknown1, "Makernote Unknown 1" },
            { TagUnknown2, "Makernote Unknown 2" },
            { TagUnknown3, "Makernote Unknown 3" },
            { TagWhiteBalance, "White Balance" }
        };

        public NikonType1MakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new NikonType1MakernoteDescriptor(this));
        }

        public override string Name => "Nikon Makernote";
    }
}
