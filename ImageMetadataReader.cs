using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.WebP;
using MetadataExtractor.Util;

using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor
{
    public static class ImageMetadataReader
    {
        public static async Task<DirectoryList> ReadMetadataAsync(Stream stream)
        {
            var fileType = FileTypeDetector.DetectFileType(stream);

            var directories = new List<Directory>();

#pragma warning disable format

            directories.AddRange(fileType switch
            {
                FileType.Jpeg      => await JpegMetadataReader.ReadMetadataAsync(stream),
                FileType.Png       => await PngMetadataReader.ReadMetadataAsync(stream),
                FileType.WebP      => await WebPMetadataReader.ReadMetadataAsync(stream),

                FileType.Unknown   => throw new ImageProcessingException("File format could not be determined"),
                _                  => Enumerable.Empty<Directory>()
            });

#pragma warning restore format

            directories.Add(new FileTypeDirectory(fileType));

            return directories;
        }

        public static async Task<DirectoryList> ReadMetadataAsync(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                directories.AddRange(await ReadMetadataAsync(stream));

            directories.Add(FileMetadataReader.Read(filePath));

            return directories;
        }
    }
}
