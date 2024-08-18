namespace MetadataExtractor.Formats.Flir
{
    internal enum FlirMainTagType : ushort
    {
        Unused = 0,
        Pixels = 1,
        GainMap = 2,
        OffsMap = 3,
        DeadMap = 4,
        GainDeadMap = 5,
        CoarseMap = 6,
        ImageMap = 7,

        BasicData = 0x20,
        Measure,
        ColorPal
    }
}
