using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Riff;

using MetadataExtractor.IO;

using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor.Formats.WebP
{
    public static class WebPMetadataReader
    {
        public static async Task<DirectoryList> ReadMetadataAsync(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                directories.AddRange(await ReadMetadataAsync(stream));

            directories.Add(FileMetadataReader.Read(filePath));

            return directories;
        }

        public static async Task<DirectoryList> ReadMetadataAsync(Stream stream)
        {
            var directories = new List<Directory>();

            await RiffReader.ProcessRiffAsync(new SequentialStreamReader(stream), new WebPRiffHandler(directories));

            return directories;
        }
    }
}
