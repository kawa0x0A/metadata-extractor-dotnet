namespace MetadataExtractor.Formats.Exif
{
    public class PanasonicRawWbInfo2Descriptor(PanasonicRawWbInfo2Directory directory) : TagDescriptor<PanasonicRawWbInfo2Directory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PanasonicRawWbInfo2Directory.TagWbType1 or PanasonicRawWbInfo2Directory.TagWbType2 or PanasonicRawWbInfo2Directory.TagWbType3 or PanasonicRawWbInfo2Directory.TagWbType4 or PanasonicRawWbInfo2Directory.TagWbType5 or PanasonicRawWbInfo2Directory.TagWbType6 or PanasonicRawWbInfo2Directory.TagWbType7 => GetWbTypeDescription(tagType),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetWbTypeDescription(int tagType)
        {
            if (!Directory.TryGetUshort(tagType, out ushort value))
                return null;
            return ExifDescriptorBase<PanasonicRawWbInfo2Directory>.GetWhiteBalanceDescription(value);
        }
    }
}
