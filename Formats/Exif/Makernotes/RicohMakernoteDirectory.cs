namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class RicohMakernoteDirectory : Directory
    {
        public const int TagMakernoteDataType = 0x0001;
        public const int TagVersion = 0x0002;
        public const int TagPrintImageMatchingInfo = 0x0E00;
        public const int TagRicohCameraInfoMakernoteSubIfdPointer = 0x2001;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagMakernoteDataType, "Makernote Data Type" },
            { TagVersion, "Version" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" },
            { TagRicohCameraInfoMakernoteSubIfdPointer, "Ricoh Camera Info Makernote Sub-IFD" }
        };

        public RicohMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new RicohMakernoteDescriptor(this));
        }

        public override string Name => "Ricoh Makernote";
    }
}
