namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class SonyType6MakernoteDescriptor(SonyType6MakernoteDirectory directory) : TagDescriptor<SonyType6MakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                SonyType6MakernoteDirectory.TagMakernoteThumbVersion => GetMakernoteThumbVersionDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetMakernoteThumbVersionDescription()
        {
            return GetVersionBytesDescription(SonyType6MakernoteDirectory.TagMakernoteThumbVersion, 2);
        }
    }
}
