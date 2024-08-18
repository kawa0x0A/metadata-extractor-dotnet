namespace MetadataExtractor.Formats.Exif
{
    public class PanasonicRawWbInfoDescriptor(PanasonicRawWbInfoDirectory directory) : TagDescriptor<PanasonicRawWbInfoDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PanasonicRawWbInfoDirectory.TagWbType1 or PanasonicRawWbInfoDirectory.TagWbType2 or PanasonicRawWbInfoDirectory.TagWbType3 or PanasonicRawWbInfoDirectory.TagWbType4 or PanasonicRawWbInfoDirectory.TagWbType5 or PanasonicRawWbInfoDirectory.TagWbType6 or PanasonicRawWbInfoDirectory.TagWbType7 => GetWbTypeDescription(tagType),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetWbTypeDescription(int tagType)
        {
            if (!Directory.TryGetUshort(tagType, out ushort value))
                return null;
            return ExifDescriptorBase<PanasonicRawWbInfoDirectory>.GetWhiteBalanceDescription(value);
        }
    }
}
