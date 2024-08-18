namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegSegment
    {
        public JpegSegmentType Type { get; }

        public byte[] Bytes { get; }

        public long Offset { get; }

        public JpegSegment(JpegSegmentType type, byte[] bytes, long offset)
        {
            Type = type;
            Bytes = bytes;
            Offset = offset;
        }

        public override string ToString() => $"[{Type}] {Bytes.Length:N0} bytes at offset {Offset}";
    }
}
