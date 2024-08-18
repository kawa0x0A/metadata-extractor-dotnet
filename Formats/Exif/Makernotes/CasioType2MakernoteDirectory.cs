namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class CasioType2MakernoteDirectory : Directory
    {
        public const int TagThumbnailDimensions = 0x0002;

        public const int TagThumbnailSize = 0x0003;

        public const int TagThumbnailOffset = 0x0004;

        public const int TagQualityMode = 0x0008;

        public const int TagImageSize = 0x0009;

        public const int TagFocusMode1 = 0x000D;

        public const int TagIsoSensitivity = 0x0014;

        public const int TagWhiteBalance1 = 0x0019;

        public const int TagFocalLength = 0x001D;

        public const int TagSaturation = 0x001F;

        public const int TagContrast = 0x0020;

        public const int TagSharpness = 0x0021;

        public const int TagPrintImageMatchingInfo = 0x0E00;

        public const int TagPreviewThumbnail = 0x2000;

        public const int TagWhiteBalanceBias = 0x2011;

        public const int TagWhiteBalance2 = 0x2012;

        public const int TagObjectDistance = 0x2022;

        public const int TagFlashDistance = 0x2034;

        public const int TagRecordMode = 0x3000;

        public const int TagSelfTimer = 0x3001;

        public const int TagQuality = 0x3002;

        public const int TagFocusMode2 = 0x3003;

        public const int TagTimeZone = 0x3006;

        public const int TagBestShotMode = 0x3007;

        public const int TagCcdIsoSensitivity = 0x3014;

        public const int TagColourMode = 0x3015;

        public const int TagEnhancement = 0x3016;

        public const int TagFilter = 0x3017;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagThumbnailDimensions, "Thumbnail Dimensions" },
            { TagThumbnailSize, "Thumbnail Size" },
            { TagThumbnailOffset, "Thumbnail Offset" },
            { TagQualityMode, "Quality Mode" },
            { TagImageSize, "Image Size" },
            { TagFocusMode1, "Focus Mode" },
            { TagIsoSensitivity, "ISO Sensitivity" },
            { TagWhiteBalance1, "White Balance" },
            { TagFocalLength, "Focal Length" },
            { TagSaturation, "Saturation" },
            { TagContrast, "Contrast" },
            { TagSharpness, "Sharpness" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" },
            { TagPreviewThumbnail, "Casio Preview Thumbnail" },
            { TagWhiteBalanceBias, "White Balance Bias" },
            { TagWhiteBalance2, "White Balance" },
            { TagObjectDistance, "Object Distance" },
            { TagFlashDistance, "Flash Distance" },
            { TagRecordMode, "Record Mode" },
            { TagSelfTimer, "Self Timer" },
            { TagQuality, "Quality" },
            { TagFocusMode2, "Focus Mode" },
            { TagTimeZone, "Time Zone" },
            { TagBestShotMode, "BestShot Mode" },
            { TagCcdIsoSensitivity, "CCD ISO Sensitivity" },
            { TagColourMode, "Colour Mode" },
            { TagEnhancement, "Enhancement" },
            { TagFilter, "Filter" }
        };

        public CasioType2MakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new CasioType2MakernoteDescriptor(this));
        }

        public override string Name => "Casio Makernote";
    }
}
