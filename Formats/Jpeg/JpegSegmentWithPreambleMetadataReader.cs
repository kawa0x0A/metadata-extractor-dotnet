using MetadataExtractor.Util;

namespace MetadataExtractor.Formats.Jpeg
{
    public abstract class JpegSegmentWithPreambleMetadataReader : IJpegSegmentMetadataReader
    {
        protected abstract byte[] PreambleBytes { get; }

        public abstract ICollection<JpegSegmentType> SegmentTypes { get; }

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            var preamble = PreambleBytes;

            await foreach (var segment in segments
                .Where(segment => segment.Bytes.StartsWith(preamble))
                .SelectMany(segment => ExtractAsync(segment.Bytes, preambleLength: preamble.Length)))
            {
                yield return segment;
            }

        }

        protected abstract IAsyncEnumerable<Directory> ExtractAsync(byte[] segmentBytes, int preambleLength);
    }
}
