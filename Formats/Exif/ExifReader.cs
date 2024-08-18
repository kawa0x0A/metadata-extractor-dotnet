using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Tiff;
using MetadataExtractor.IO;
using MetadataExtractor.Util;
using System.Text;

namespace MetadataExtractor.Formats.Exif
{
    public sealed class ExifReader : JpegSegmentWithPreambleMetadataReader
    {
        public const string JpegSegmentPreamble = "Exif\x0\x0";

        private static readonly byte[] _preambleBytes = Encoding.ASCII.GetBytes(JpegSegmentPreamble);

        public static bool StartsWithJpegExifPreamble(byte[] bytes) => bytes.StartsWith(_preambleBytes);

        public static int JpegSegmentPreambleLength => _preambleBytes.Length;

        protected override byte[] PreambleBytes => _preambleBytes;

        public override ICollection<JpegSegmentType> SegmentTypes { get; } = [JpegSegmentType.App1];

        protected override IAsyncEnumerable<Directory> ExtractAsync(byte[] segmentBytes, int preambleLength)
        {
            return ExtractAsync(new ByteArrayReader(segmentBytes, baseOffset: preambleLength), preambleLength);
        }

        public static async IAsyncEnumerable<Directory> ExtractAsync(IndexedReader reader, int exifStartOffset)
        {
            var directories = new List<Directory>();
            var exifTiffHandler = new ExifTiffHandler(directories, exifStartOffset);

            try
            {
                await Task.FromResult(TiffReader.ProcessTiffAsync(reader, exifTiffHandler));
            }
            catch (Exception e)
            {
                exifTiffHandler.Error("Exception processing TIFF data: " + e.Message);
            }

            foreach (var directory in directories)
            {
                yield return directory;
            }
        }
    }
}
