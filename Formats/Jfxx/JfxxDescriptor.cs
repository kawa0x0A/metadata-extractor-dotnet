namespace MetadataExtractor.Formats.Jfxx
{
    public sealed class JfxxDescriptor(JfxxDirectory directory) : TagDescriptor<JfxxDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                JfxxDirectory.TagExtensionCode => GetExtensionCodeDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetExtensionCodeDescription()
        {
            if (!Directory.TryGetInt(JfxxDirectory.TagExtensionCode, out int value))
                return null;

            return value switch
            {
                0x10 => "Thumbnail coded using JPEG",
                0x11 => "Thumbnail stored using 1 byte/pixel",
                0x13 => "Thumbnail stored using 3 bytes/pixel",
                _ => "Unknown extension code " + value,
            };
        }
    }
}
