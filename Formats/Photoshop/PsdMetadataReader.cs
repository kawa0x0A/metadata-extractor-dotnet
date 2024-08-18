using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Photoshop
{
    public static class PsdMetadataReader
    {
        public static IAsyncEnumerable<MetadataExtractor.Directory> ReadMetadataAsync(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                directories.AddRange(PsdReader.ExtractAsync(new SequentialStreamReader(stream)).ToBlockingEnumerable());

            directories.Add(FileMetadataReader.Read(filePath));

            return directories.ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<MetadataExtractor.Directory> ReadMetadataAsync(Stream stream)
        {
            return PsdReader.ExtractAsync(new SequentialStreamReader(stream));
        }
    }
}
