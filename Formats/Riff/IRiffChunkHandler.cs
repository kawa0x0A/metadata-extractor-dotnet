namespace MetadataExtractor.Formats.Riff
{
    public interface IRiffChunkHandler
    {
        void ProcessChunk(string fourCc, byte[] payload);
    }
}
