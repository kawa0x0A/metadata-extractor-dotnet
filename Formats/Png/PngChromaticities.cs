using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Png
{
    public sealed class PngChromaticities
    {
        public int WhitePointX { get; }
        public int WhitePointY { get; }
        public int RedX { get; }
        public int RedY { get; }
        public int GreenX { get; }
        public int GreenY { get; }
        public int BlueX { get; }
        public int BlueY { get; }

        public PngChromaticities(byte[] bytes)
        {
            if (bytes.Length != 8 * 4)
                throw new PngProcessingException("Invalid number of bytes");

            var reader = new SequentialByteArrayReader(bytes);

            try
            {
                WhitePointX = reader.GetInt32();
                WhitePointY = reader.GetInt32();
                RedX = reader.GetInt32();
                RedY = reader.GetInt32();
                GreenX = reader.GetInt32();
                GreenY = reader.GetInt32();
                BlueX = reader.GetInt32();
                BlueY = reader.GetInt32();
            }
            catch (IOException ex)
            {
                throw new PngProcessingException(ex);
            }
        }
    }
}
