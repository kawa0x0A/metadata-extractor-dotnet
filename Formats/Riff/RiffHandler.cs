namespace MetadataExtractor.Formats.Riff
{
    public abstract class RiffHandler(List<Directory> directories, Dictionary<string, Func<List<Directory>, IRiffChunkHandler>> handlers) : IRiffHandler
    {
        public void ProcessChunk(string fourCc, byte[] payload)
        {
            if (!handlers.TryGetValue(fourCc, out Func<List<Directory>, IRiffChunkHandler>? createHandler))
                return;
            var handler = createHandler(directories);
            handler.ProcessChunk(fourCc, payload);
        }

        public bool ShouldAcceptChunk(string fourCc) => handlers.ContainsKey(fourCc);

        public abstract bool ShouldAcceptRiffIdentifier(string identifier);

        public abstract bool ShouldAcceptList(string fourCc);

        public void AddError(string errorMessage)
        {
            directories.Add(new ErrorDirectory(errorMessage));
        }
    }
}
