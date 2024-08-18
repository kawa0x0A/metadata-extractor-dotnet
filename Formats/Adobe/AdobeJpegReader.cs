using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Adobe
{
    public sealed class AdobeJpegReader : IJpegSegmentMetadataReader
    {
        public const string JpegSegmentPreamble = "Adobe";

        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = [JpegSegmentType.AppE];

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            await foreach (var segment in segments
                .Where(segment => segment.Bytes.Length == 12 && JpegSegmentPreamble.Equals(Encoding.UTF8.GetString(segment.Bytes, 0, JpegSegmentPreamble.Length), StringComparison.OrdinalIgnoreCase))
                .SelectAwait(async bytes => (Directory)await ExtractAsync(new SequentialByteArrayReader(bytes.Bytes))))
            {
                yield return segment;
            }
        }

        public static async Task<AdobeJpegDirectory> ExtractAsync(SequentialReader reader)
        {
            reader = reader.WithByteOrder(isMotorolaByteOrder: false);

            var directory = new AdobeJpegDirectory();

            try
            {
                if (await reader.GetStringAsync(JpegSegmentPreamble.Length, Encoding.ASCII) != JpegSegmentPreamble)
                {
                    directory.AddError("Invalid Adobe JPEG data header.");
                    return directory;
                }

                directory.Set(AdobeJpegDirectory.TagDctEncodeVersion, await reader.GetUInt16Async());
                directory.Set(AdobeJpegDirectory.TagApp14Flags0, await reader.GetUInt16Async());
                directory.Set(AdobeJpegDirectory.TagApp14Flags1, await reader.GetUInt16Async());
                directory.Set(AdobeJpegDirectory.TagColorTransform, await reader.GetSByteAsync());
            }
            catch (IOException ex)
            {
                directory.AddError("IO exception processing data: " + ex.Message);
            }

            return directory;
        }
    }
}
