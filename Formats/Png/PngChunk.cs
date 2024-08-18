namespace MetadataExtractor.Formats.Png
{
    public sealed class PngChunk
    {
        public PngChunk(PngChunkType chunkType, byte[] bytes)
        {
            ChunkType = chunkType;
            Bytes = bytes;
        }

        public PngChunkType ChunkType { get; }

        public byte[] Bytes { get; }
    }
}
