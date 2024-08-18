using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Png
{
    public sealed class PngHeader
    {
        public PngHeader(byte[] bytes)
        {
            if (bytes.Length != 13)
                throw new PngProcessingException("PNG header chunk must have exactly 13 data bytes");

            var reader = new SequentialByteArrayReader(bytes);

            ImageWidth = reader.GetInt32();
            ImageHeight = reader.GetInt32();
            BitsPerSample = reader.GetByte();
            ColorType = PngColorType.FromNumericValue(reader.GetByte());
            CompressionType = reader.GetByte();
            FilterMethod = reader.GetByte();
            InterlaceMethod = reader.GetByte();
        }

        public int ImageWidth { get; }

        public int ImageHeight { get; }

        public byte BitsPerSample { get; }

        public PngColorType ColorType { get; }

        public byte CompressionType { get; }

        public byte FilterMethod { get; }

        public byte InterlaceMethod { get; }
    }
}
