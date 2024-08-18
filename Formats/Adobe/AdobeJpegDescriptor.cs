namespace MetadataExtractor.Formats.Adobe
{
    public class AdobeJpegDescriptor(AdobeJpegDirectory directory) : TagDescriptor<AdobeJpegDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                AdobeJpegDirectory.TagColorTransform => GetColorTransformDescription(),
                AdobeJpegDirectory.TagDctEncodeVersion => GetDctEncodeVersionDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string GetDctEncodeVersionDescription()
        {
            if (!Directory.TryGetInt(AdobeJpegDirectory.TagDctEncodeVersion, out int value))
                return string.Empty;

            return value == 0x64 ? "100" : value.ToString();
        }

        public string? GetColorTransformDescription()
        {
            return GetIndexedDescription(AdobeJpegDirectory.TagColorTransform,
                "Unknown (RGB or CMYK)",
                "YCbCr",
                "YCCK");
        }
    }
}
