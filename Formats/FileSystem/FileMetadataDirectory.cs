namespace MetadataExtractor.Formats.FileSystem
{
    public class FileMetadataDirectory : Directory
    {
        public const int TagFileName = 1;
        public const int TagFileSize = 2;
        public const int TagFileModifiedDate = 3;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagFileName, "File Name" },
            { TagFileSize, "File Size" },
            { TagFileModifiedDate, "File Modified Date" }
        };

        public FileMetadataDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new FileMetadataDescriptor(this));
        }

        public override string Name => "File";
    }
}
