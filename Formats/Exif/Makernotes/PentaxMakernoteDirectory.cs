namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class PentaxMakernoteDirectory : Directory
    {
        public const int TagCaptureMode = 0x0001;

        public const int TagQualityLevel = 0x0002;

        public const int TagFocusMode = 0x0003;

        public const int TagFlashMode = 0x0004;

        public const int TagWhiteBalance = 0x0007;

        public const int TagDigitalZoom = 0x000A;

        public const int TagSharpness = 0x000B;

        public const int TagContrast = 0x000C;

        public const int TagSaturation = 0x000D;

        public const int TagIsoSpeed = 0x0014;

        public const int TagColour = 0x0017;

        public const int TagPrintImageMatchingInfo = 0x0E00;

        public const int TagTimeZone = 0x1000;

        public const int TagDaylightSavings = 0x1001;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagCaptureMode, "Capture Mode" },
            { TagQualityLevel, "Quality Level" },
            { TagFocusMode, "Focus Mode" },
            { TagFlashMode, "Flash Mode" },
            { TagWhiteBalance, "White Balance" },
            { TagDigitalZoom, "Digital Zoom" },
            { TagSharpness, "Sharpness" },
            { TagContrast, "Contrast" },
            { TagSaturation, "Saturation" },
            { TagIsoSpeed, "ISO Speed" },
            { TagColour, "Colour" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" },
            { TagTimeZone, "Time Zone" },
            { TagDaylightSavings, "Daylight Savings" }
        };

        public PentaxMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new PentaxMakernoteDescriptor(this));
        }

        public override string Name => "Pentax Makernote";
    }
}
