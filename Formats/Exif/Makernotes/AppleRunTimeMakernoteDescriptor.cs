using System.Text;

namespace MetadataExtractor.Formats.Exif.Makernotes;

public sealed class AppleRunTimeMakernoteDescriptor : TagDescriptor<AppleRunTimeMakernoteDirectory>
{
    public AppleRunTimeMakernoteDescriptor(AppleRunTimeMakernoteDirectory directory) : base(directory)
    {
    }

    public override string? GetDescription(int tagType)
    {
        return tagType switch
        {
            AppleRunTimeMakernoteDirectory.TagFlags => GetFlagsDescription(),
            AppleRunTimeMakernoteDirectory.TagValue => GetValueDescription(),
            _ => base.GetDescription(tagType),
        };
    }

    public string? GetFlagsDescription()
    {
        if (Directory.TryGetInt(AppleRunTimeMakernoteDirectory.TagFlags, out var value))
        {
            StringBuilder sb = new();

            if ((value & 0x1) != 0)
                sb.Append("Valid");
            else
                sb.Append("Invalid");

            if ((value & 0x2) != 0)
                sb.Append(", rounded");

            if ((value & 0x4) != 0)
                sb.Append(", positive infinity");

            if ((value & 0x8) != 0)
                sb.Append(", negative infinity");

            if ((value & 0x10) != 0)
                sb.Append(", indefinite");

            return sb.ToString();
        }

        return base.GetDescription(AppleRunTimeMakernoteDirectory.TagFlags);
    }

    public string? GetValueDescription()
    {
        if (Directory.TryGetLong(AppleRunTimeMakernoteDirectory.TagValue, out var value) &&
            Directory.TryGetLong(AppleRunTimeMakernoteDirectory.TagScale, out var scale))
        {
            return $"{value / scale} seconds";
        }

        return base.GetDescription(AppleRunTimeMakernoteDirectory.TagValue);
    }
}
