using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Exif.Makernotes;

public sealed class NikonPictureControl1Directory : Directory
{
    public const int TagPictureControlVersion = 0;
    public const int TagPictureControlName = 4;
    public const int TagPictureControlBase = 24;

    public const int TagPictureControlAdjust = 48;
    public const int TagPictureControlQuickAdjust = 49;
    public const int TagSharpness = 50;
    public const int TagContrast = 51;
    public const int TagBrightness = 52;
    public const int TagSaturation = 53;
    public const int TagHueAdjustment = 54;
    public const int TagFilterEffect = 55;
    public const int TagToningEffect = 56;
    public const int TagToningSaturation = 57;

    private static readonly Dictionary<int, string> _tagNameMap = new()
    {
        { TagPictureControlVersion, "Picture Control Version" },
        { TagPictureControlName, "Picture Control Name" },
        { TagPictureControlBase, "Picture Control Base" },
        { TagPictureControlAdjust, "Picture Control Adjust" },
        { TagPictureControlQuickAdjust, "Picture Control Quick Adjust" },
        { TagSharpness, "Sharpness" },
        { TagContrast, "Contrast" },
        { TagBrightness, "Brightness" },
        { TagSaturation, "Saturation" },
        { TagHueAdjustment, "Hue Adjustment" },
        { TagFilterEffect, "Filter Effect" },
        { TagToningEffect, "Toning Effect" },
        { TagToningSaturation, "Toning Saturation" },
    };

    public NikonPictureControl1Directory() : base(_tagNameMap)
    {
        SetDescriptor(new NikonPictureControl1Descriptor(this));
    }

    public override string Name => "Nikon PictureControl 1";

    internal static async Task<NikonPictureControl1Directory> FromBytes(byte[] bytes)
    {
        const int ExpectedLength = 58;

        if (bytes.Length != ExpectedLength)
        {
            throw new ArgumentException($"Must have {ExpectedLength} bytes.");
        }

        SequentialByteArrayReader reader = new(bytes);

        NikonPictureControl1Directory directory = new();

        directory.Set(TagPictureControlVersion, await reader.GetStringValueAsync(4));
        directory.Set(TagPictureControlName, await reader.GetStringValueAsync(20));
        directory.Set(TagPictureControlBase, await reader.GetStringValueAsync(20));
        reader.Skip(4);
        directory.Set(TagPictureControlAdjust, await reader.GetByteAsync());
        directory.Set(TagPictureControlQuickAdjust, await reader.GetByteAsync());
        directory.Set(TagSharpness, await reader.GetByteAsync());
        directory.Set(TagContrast, await reader.GetByteAsync());
        directory.Set(TagBrightness, await reader.GetByteAsync());
        directory.Set(TagSaturation, await reader.GetByteAsync());
        directory.Set(TagHueAdjustment, await reader.GetByteAsync());
        directory.Set(TagFilterEffect, await reader.GetByteAsync());
        directory.Set(TagToningEffect, await reader.GetByteAsync());
        directory.Set(TagToningSaturation, await reader.GetByteAsync());

        return directory;
    }
}
