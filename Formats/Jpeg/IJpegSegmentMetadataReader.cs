namespace MetadataExtractor.Formats.Jpeg
{
    public interface IJpegSegmentMetadataReader
    {
        ICollection<JpegSegmentType> SegmentTypes { get; }

        IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments);
    }
}
