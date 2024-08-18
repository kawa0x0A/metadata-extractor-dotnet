namespace MetadataExtractor.Formats.FileSystem
{
    public sealed class FileMetadataReader
    {
        public static FileMetadataDirectory Read(string file)
        {
            var attr = File.GetAttributes(file);

            if ((attr & FileAttributes.Directory) != 0)
                throw new IOException("File object must reference a file");

            var fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
                throw new IOException("File does not exist");

            var directory = new FileMetadataDirectory();

            directory.Set(FileMetadataDirectory.TagFileName, Path.GetFileName(file));
            directory.Set(FileMetadataDirectory.TagFileSize, fileInfo.Length);
            directory.Set(FileMetadataDirectory.TagFileModifiedDate, fileInfo.LastWriteTime);

            return directory;
        }
    }
}
