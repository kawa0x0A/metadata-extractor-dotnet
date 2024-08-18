namespace MetadataExtractor.Formats.Jfxx
{
    public sealed class JfxxDirectory : Directory
    {
        public const int TagExtensionCode = 5;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagExtensionCode, "Extension Code" }
        };

        public JfxxDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new JfxxDescriptor(this));
        }

        public override string Name => "JFXX";

        public int GetExtensionCode()
        {
            return this.GetInt(TagExtensionCode);
        }
    }
}
