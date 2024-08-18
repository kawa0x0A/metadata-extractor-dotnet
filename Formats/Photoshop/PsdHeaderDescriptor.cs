namespace MetadataExtractor.Formats.Photoshop
{
    public sealed class PsdHeaderDescriptor(PsdHeaderDirectory directory) : TagDescriptor<PsdHeaderDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PsdHeaderDirectory.TagChannelCount => GetChannelCountDescription(),
                PsdHeaderDirectory.TagBitsPerChannel => GetBitsPerChannelDescription(),
                PsdHeaderDirectory.TagColorMode => GetColorModeDescription(),
                PsdHeaderDirectory.TagImageHeight => GetImageHeightDescription(),
                PsdHeaderDirectory.TagImageWidth => GetImageWidthDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetChannelCountDescription()
        {
            if (!Directory.TryGetInt(PsdHeaderDirectory.TagChannelCount, out int value))
                return null;
            return value + " channel" + (value == 1 ? string.Empty : "s");
        }

        public string? GetBitsPerChannelDescription()
        {
            if (!Directory.TryGetInt(PsdHeaderDirectory.TagBitsPerChannel, out int value))
                return null;
            return value + " bit" + (value == 1 ? string.Empty : "s") + " per channel";
        }

        public string? GetColorModeDescription()
        {
            return GetIndexedDescription(PsdHeaderDirectory.TagColorMode,
                "Bitmap",
                "Grayscale",
                "Indexed",
                "RGB",
                "CMYK",
                null,
                null,
                "Multichannel",
                "Duotone",
                "Lab");
        }

        public string? GetImageHeightDescription()
        {
            if (!Directory.TryGetInt(PsdHeaderDirectory.TagImageHeight, out int value))
                return null;
            return value + " pixel" + (value == 1 ? string.Empty : "s");
        }

        public string? GetImageWidthDescription()
        {
            if (!Directory.TryGetInt(PsdHeaderDirectory.TagImageWidth, out int value))
                return null;
            return value + " pixel" + (value == 1 ? string.Empty : "s");
        }
    }
}
