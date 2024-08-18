namespace MetadataExtractor.Formats.Riff
{
    public interface IRiffHandler
    {
        bool ShouldAcceptRiffIdentifier(string identifier);

        bool ShouldAcceptChunk(string fourCc);

        bool ShouldAcceptList(string fourCc);

        void ProcessChunk(string fourCc, byte[] payload);

        void AddError(string errorMessage);
    }
}
