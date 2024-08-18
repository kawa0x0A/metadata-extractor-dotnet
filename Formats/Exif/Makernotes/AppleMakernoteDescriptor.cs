using static MetadataExtractor.Formats.Exif.Makernotes.AppleMakernoteDirectory;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class AppleMakernoteDescriptor(AppleMakernoteDirectory directory) : TagDescriptor<AppleMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                TagHdrImageType => GetHdrImageTypeDescription(),
                TagAccelerationVector => GetAccelerationVectorDescription(),
                _ => base.GetDescription(tagType)
            };
        }

        public string? GetHdrImageTypeDescription()
        {
            return GetIndexedDescription(TagHdrImageType, 3, "HDR Image", "Original Image");
        }

        public string? GetAccelerationVectorDescription()
        {
            var values = Directory.GetArray<Rational[]>(TagAccelerationVector);
            if (values is null || values.Length != 3)
                return null;
            return $"{values[0].Absolute.ToDouble():N2}g {(values[0].IsPositive ? "left" : "right")}, " +
                   $"{values[1].Absolute.ToDouble():N2}g {(values[1].IsPositive ? "down" : "up")}, " +
                   $"{values[2].Absolute.ToDouble():N2}g {(values[2].IsPositive ? "forward" : "backward")}";
        }
    }
}
