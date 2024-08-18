namespace MetadataExtractor.Util
{
    public static class FileTypeDetector
    {
        private static readonly ByteTrie<FileType> _root = new(defaultValue: FileType.Unknown)
        {
            { FileType.Jpeg, new byte[] { 0xff, 0xd8 } },
            { FileType.Png, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52 } },
        };

        private static readonly int _bytesNeeded = _root.MaxDepth;

        public static FileType DetectFileType(Stream stream)
        {
            if (!stream.CanSeek)
                throw new ArgumentException("Must support seek", nameof(stream));

            var bytes = new byte[_bytesNeeded];
            var bytesRead = stream.Read(bytes, 0, bytes.Length);

            if (bytesRead == 0)
                return FileType.Unknown;

            stream.Seek(-bytesRead, SeekOrigin.Current);

            return _root.Find(bytes);
        }
    }
}
