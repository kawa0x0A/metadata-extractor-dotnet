using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Exif.Makernotes;

public sealed class NikonPictureControl2Directory : Directory
{
    public const int TagPictureControlVersion = 0;
    public const int TagPictureControlName = 4;
    public const int TagPictureControlBase = 24;
    public const int TagPictureControlAdjust = 48;
    public const int TagPictureControlQuickAdjust = 49;

    public const int TagSharpness = 51;

    public const int TagClarity = 53;

    public const int TagContrast = 55;

    public const int TagBrightness = 57;

    public const int TagSaturation = 59;

    public const int TagHue = 61;

    public const int TagFilterEffect = 63;
    public const int TagToningEffect = 64;
    public const int TagToningSaturation = 65;

    private static readonly Dictionary<int, string> _tagNameMap = new()
    {
        { TagPictureControlVersion, "Picture Control Version" },
        { TagPictureControlName, "Picture Control Name" },
        { TagPictureControlBase, "Picture Control Base" },
        { TagPictureControlAdjust, "Picture Control Adjust" },
        { TagPictureControlQuickAdjust, "Picture Control Quick Adjust" },
        { TagSharpness, "Sharpness" },
        { TagClarity, "Clarity" },
        { TagContrast, "Contrast" },
        { TagBrightness, "Brightness" },
        { TagSaturation, "Saturation" },
        { TagHue, "Hue" },
        { TagFilterEffect, "Filter Effect" },
        { TagToningEffect, "Toning Effect" },
        { TagToningSaturation, "Toning Saturation" },
    };

    public NikonPictureControl2Directory() : base(_tagNameMap)
    {
        SetDescriptor(new NikonPictureControl2Descriptor(this));
    }

    public override string Name => "Nikon PictureControl 2";

    internal static async Task<NikonPictureControl2Directory> FromBytesAsync(byte[] bytes)
    {
        const int ExpectedLength = 68;

        if (bytes.Length != ExpectedLength)
        {
            throw new ArgumentException($"Must have {ExpectedLength} bytes.");
        }

        SequentialByteArrayReader reader = new(bytes);

        NikonPictureControl2Directory directory = new();

        directory.Set(TagPictureControlVersion, await reader.GetStringValueAsync(4));
        directory.Set(TagPictureControlName, await reader.GetStringValueAsync(20));
        directory.Set(TagPictureControlBase, await reader.GetStringValueAsync(20));
        reader.Skip(4);
        directory.Set(TagPictureControlAdjust, await reader.GetByteAsync());
        directory.Set(TagPictureControlQuickAdjust, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagSharpness, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagClarity, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagContrast, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagBrightness, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagSaturation, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagHue, await reader.GetByteAsync());
        reader.Skip(1);
        directory.Set(TagFilterEffect, await reader.GetByteAsync());
        directory.Set(TagToningEffect, await reader.GetByteAsync());
        directory.Set(TagToningSaturation, await reader.GetByteAsync());
        reader.Skip(2);

        return directory;
    }
}
