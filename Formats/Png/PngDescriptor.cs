using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Png
{
    public sealed class PngDescriptor(PngDirectory directory) : TagDescriptor<PngDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                PngDirectory.TagColorType => GetColorTypeDescription(),
                PngDirectory.TagCompressionType => GetCompressionTypeDescription(),
                PngDirectory.TagFilterMethod => GetFilterMethodDescription(),
                PngDirectory.TagInterlaceMethod => GetInterlaceMethodDescription(),
                PngDirectory.TagPaletteHasTransparency => GetPaletteHasTransparencyDescription(),
                PngDirectory.TagSrgbRenderingIntent => GetIsSrgbColorSpaceDescription(),
                PngDirectory.TagTextualData => GetTextualDataDescription(),
                PngDirectory.TagBackgroundColor => GetBackgroundColorDescription(),
                PngDirectory.TagUnitSpecifier => GetUnitSpecifierDescription(),
                PngDirectory.TagLastModificationTime => GetLastModificationTimeDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetColorTypeDescription()
        {
            if (!Directory.TryGetInt(PngDirectory.TagColorType, out int value))
                return null;
            return PngColorType.FromNumericValue(value).Description;
        }

        public string? GetCompressionTypeDescription()
        {
            return GetIndexedDescription(PngDirectory.TagCompressionType, "Deflate");
        }

        public string? GetFilterMethodDescription()
        {
            return GetIndexedDescription(PngDirectory.TagFilterMethod, "Adaptive");
        }

        public string? GetInterlaceMethodDescription()
        {
            return GetIndexedDescription(PngDirectory.TagInterlaceMethod, "No Interlace", "Adam7 Interlace");
        }

        public string? GetPaletteHasTransparencyDescription()
        {
            return GetIndexedDescription(PngDirectory.TagPaletteHasTransparency, null, "Yes");
        }

        public string? GetIsSrgbColorSpaceDescription()
        {
            return GetIndexedDescription(PngDirectory.TagSrgbRenderingIntent, "Perceptual", "Relative Colorimetric", "Saturation", "Absolute Colorimetric");
        }

        public string? GetUnitSpecifierDescription()
        {
            return GetIndexedDescription(PngDirectory.TagUnitSpecifier, "Unspecified", "Metres");
        }

        public string? GetLastModificationTimeDescription()
        {
            if (!Directory.TryGetDateTime(PngDirectory.TagLastModificationTime, out DateTime value))
                return null;

            return value.ToString("yyyy:MM:dd HH:mm:ss");
        }

        public string GetTextualDataDescription()
        {
            var pairs = Directory.GetKeyValuePair(PngDirectory.TagTextualData);

            return string.Join(
                    "\n",
                    $"{pairs.Key}: {pairs.Value}"
                    );
        }

        public string? GetBackgroundColorDescription()
        {
            var bytes = Directory.GetArray<byte[]>(PngDirectory.TagBackgroundColor);
            if (bytes is null)
                return null;

            var reader = new SequentialByteArrayReader(bytes);
            try
            {
                switch (bytes.Length)
                {
                    case 1:
                        return $"Palette Index {reader.GetByteAsync()}";
                    case 2:
                        return $"Greyscale Level {reader.GetUInt16Async()}";
                    case 6:
                        return $"R {reader.GetUInt16Async()}, G {reader.GetUInt16Async()}, B {reader.GetUInt16Async()}";
                }
            }
            catch (IOException)
            {
                return null;
            }

            return null;
        }
    }
}
