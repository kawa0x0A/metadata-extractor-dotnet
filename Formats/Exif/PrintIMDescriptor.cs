namespace MetadataExtractor.Formats.Exif
{
    public class PrintIMDescriptor(PrintIMDirectory directory) : TagDescriptor<PrintIMDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            switch (tagType)
            {
                case PrintIMDirectory.TagPrintImVersion:
                    return base.GetDescription(tagType);
                default:
                    if (!Directory.TryGetUint(tagType, out uint value))
                        return null;
                    return "0x" + value.ToString("x8");
            }
        }
    }
}
