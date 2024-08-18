namespace MetadataExtractor.Formats.FileSystem
{
    public class FileMetadataDescriptor(FileMetadataDirectory directory) : TagDescriptor<FileMetadataDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                FileMetadataDirectory.TagFileSize => GetFileSizeDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        private string? GetFileSizeDescription()
        {
            return Directory.TryGetLong(FileMetadataDirectory.TagFileSize, out long size)
                ? size + " bytes"
                : null;
        }
    }
}
