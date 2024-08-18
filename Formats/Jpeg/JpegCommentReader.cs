using System.Text;

namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegCommentReader : IJpegSegmentMetadataReader
    {
        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.Com };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            await foreach (var segment in segments.Select(segment => (Directory)new JpegCommentDirectory(new StringValue(segment.Bytes, Encoding.UTF8))))
            {
                yield return segment;
            }
        }
    }
}
