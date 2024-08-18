namespace MetadataExtractor.Formats.Jpeg
{
    public enum JpegSegmentType : byte
    {
        Tem = 0x01,

        Sof0 = 0xC0,

        Sof1 = 0xC1,

        Sof2 = 0xC2,

        Sof3 = 0xC3,

        Dht = 0xC4,

        Sof5 = 0xC5,

        Sof6 = 0xC6,

        Sof7 = 0xC7,

        Sof9 = 0xC9,

        Sof10 = 0xCA,

        Sof11 = 0xCB,

        Dac = 0xCC,

        Sof13 = 0xCD,

        Sof14 = 0xCE,

        Sof15 = 0xCF,

        Rst0 = 0xD0,

        Rst1 = 0xD1,

        Rst2 = 0xD2,

        Rst3 = 0xD3,

        Rst4 = 0xD4,

        Rst5 = 0xD5,

        Rst6 = 0xD6,

        Rst7 = 0xD7,

        Soi = 0xD8,

        Eoi = 0xD9,

        Sos = 0xDA,

        Dqt = 0xDB,

        Dnl = 0xDC,

        Dri = 0xDD,

        Dhp = 0xDE,

        Exp = 0xDF,

        App0 = 0xE0,

        App1 = 0xE1,

        App2 = 0xE2,

        App3 = 0xE3,

        App4 = 0xE4,

        App5 = 0xE5,

        App6 = 0xE6,

        App7 = 0xE7,

        App8 = 0xE8,

        App9 = 0xE9,

        AppA = 0xEA,

        AppB = 0xEB,

        AppC = 0xEC,

        AppD = 0xED,

        AppE = 0xEE,

        AppF = 0xEF,

        Com = 0xFE
    }

    public static class JpegSegmentTypeExtensions
    {
        public static bool CanContainMetadata(this JpegSegmentType type)
        {
            return type switch
            {
                JpegSegmentType.Soi or JpegSegmentType.Dac or JpegSegmentType.Dhp or JpegSegmentType.Dht or JpegSegmentType.Dnl or JpegSegmentType.Dqt or JpegSegmentType.Dri or JpegSegmentType.Exp => false,
                _ => true,
            };
        }

        public static IReadOnlyList<JpegSegmentType> CanContainMetadataTypes { get; }
            = Enum.GetValues(typeof(JpegSegmentType)).Cast<JpegSegmentType>().Where(type => type.CanContainMetadata()).ToList();

        public static bool ContainsPayload(this JpegSegmentType type)
        {
            return type switch
            {
                JpegSegmentType.Soi or JpegSegmentType.Eoi or JpegSegmentType.Rst0 or JpegSegmentType.Rst1 or JpegSegmentType.Rst2 or JpegSegmentType.Rst3 or JpegSegmentType.Rst4 or JpegSegmentType.Rst5 or JpegSegmentType.Rst6 or JpegSegmentType.Rst7 => false,
                _ => true,
            };
        }

        public static bool IsApplicationSpecific(this JpegSegmentType type) => type is >= JpegSegmentType.App0 and <= JpegSegmentType.AppF;
    }
}
