namespace MetadataExtractor.Formats.Flir
{
    public sealed class FlirHeaderDirectory : Directory
    {
        public override string Name => "FLIR Header";

        public const int TagCreatorSoftware = 0;

        private static readonly Dictionary<int, string> _nameByTag = new()
        {
            { TagCreatorSoftware, "Creator Software" }
        };

        public FlirHeaderDirectory() : base(_nameByTag)
        {
            SetDescriptor(new TagDescriptor<FlirHeaderDirectory>(this));
        }
    }
}
