namespace MetadataExtractor.Formats.Exif
{
    public class PrintIMDirectory : Directory
    {
        public const int TagPrintImVersion = 0x0000;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagPrintImVersion, "PrintIM Version" }
        };

        public PrintIMDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new PrintIMDescriptor(this));
        }

        public override string Name => "PrintIM";
    }
}
