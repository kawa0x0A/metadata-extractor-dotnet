namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class AppleMakernoteDirectory : Directory
    {
#pragma warning disable format
        public const int TagRunTime            = 0x0003;
        public const int TagAccelerationVector = 0x0008;
        public const int TagHdrImageType       = 0x000a;

        public const int TagBurstUuid          = 0x000b;
        public const int TagContentIdentifier  = 0x0011;
        public const int TagImageUniqueId      = 0x0015;
        public const int TagLivePhotoId        = 0x0017;
#pragma warning restore format

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagRunTime, "Run Time" },
            { TagAccelerationVector, "Acceleration Vector" },
            { TagHdrImageType, "HDR Image Type" },
            { TagBurstUuid, "Burst UUID" },
            { TagContentIdentifier, "Content Identifier" },
            { TagImageUniqueId, "Image Unique ID" },
            { TagLivePhotoId, "Live Photo ID" }
        };

        public AppleMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new AppleMakernoteDescriptor(this));
        }

        public override string Name => "Apple Makernote";
    }
}
