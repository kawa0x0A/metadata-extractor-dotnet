using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.IO;
using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor.Formats.Tiff
{
    public static class TiffMetadataReader
    {
        public static async Task<DirectoryList> ReadMetadataAsync(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.RandomAccess))
            {
                var handler = new ExifTiffHandler(directories, exifStartOffset: 0);
                await TiffReader.ProcessTiffAsync(new IndexedSeekingReader(stream), handler);
            }

            directories.Add(FileMetadataReader.Read(filePath));

            return directories;
        }

        public static async Task<DirectoryList> ReadMetadataAsync(Stream stream)
        {
            var directories = new List<Directory>();

            var handler = new ExifTiffHandler(directories, exifStartOffset: 0);
            await TiffReader.ProcessTiffAsync(new IndexedCapturingReader(stream), handler);

            return directories;
        }
    }
}
