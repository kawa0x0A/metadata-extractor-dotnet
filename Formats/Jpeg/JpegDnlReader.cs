using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegDnlReader : IJpegSegmentMetadataReader
    {
        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.Dnl };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            await foreach (var segment in segments.Select(async segment => (Directory)await Extract(new SequentialByteArrayReader(segment.Bytes))))
            {
                yield return await segment;
            }
        }

        public static async Task<JpegDnlDirectory> Extract(SequentialReader reader)
        {
            var directory = new JpegDnlDirectory();

            try
            {
                directory.Set(JpegDnlDirectory.TagImageHeight, await reader.GetUInt16Async());
            }
            catch (IOException me)
            {
                directory.AddError(me.ToString());
            }

            return directory;
        }
    }
}
