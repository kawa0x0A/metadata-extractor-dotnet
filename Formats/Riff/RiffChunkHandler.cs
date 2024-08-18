namespace MetadataExtractor.Formats.Riff
{
    public abstract class RiffChunkHandler<T> : IRiffChunkHandler
        where T : Directory, new()
    {
        private readonly List<Directory> _directories;

        public RiffChunkHandler(List<Directory> directories)
        {
            _directories = directories;
        }

        public void ProcessChunk(string fourCc, byte[] payload)
        {
            if (payload.Length < MinSize)
                return;

            var directory = new T();
            try
            {
                Populate(directory, payload);
            }
            catch (IOException e)
            {
                directory.AddError($"Exception reading chunk '{fourCc}' : {e.Message}");
            }
            _directories.Add(directory);
        }

        protected abstract int MinSize { get; }

        protected abstract void Populate(T directory, byte[] payload);
    }
}
