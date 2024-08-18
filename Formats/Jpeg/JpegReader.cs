using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegReader : IJpegSegmentMetadataReader
    {
        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new HashSet<JpegSegmentType>
        {
            JpegSegmentType.Sof0, JpegSegmentType.Sof1, JpegSegmentType.Sof2, JpegSegmentType.Sof3,
            JpegSegmentType.Sof5, JpegSegmentType.Sof6, JpegSegmentType.Sof7, JpegSegmentType.Sof9,
            JpegSegmentType.Sof10, JpegSegmentType.Sof11, JpegSegmentType.Sof13, JpegSegmentType.Sof14,
            JpegSegmentType.Sof15
        };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            await foreach (var segment in segments.SelectAwait(async segment => (Directory)await ExtractAsync(segment)))
            {
                yield return segment;
            }
        }

        public static async Task<JpegDirectory> ExtractAsync(JpegSegment segment)
        {
            var directory = new JpegDirectory();

            directory.Set(JpegDirectory.TagCompressionType, (int)segment.Type - (int)JpegSegmentType.Sof0);

            SequentialReader reader = new SequentialByteArrayReader(segment.Bytes);

            try
            {
                directory.Set(JpegDirectory.TagDataPrecision, await reader.GetByteAsync());
                directory.Set(JpegDirectory.TagImageHeight, await reader.GetUInt16Async());
                directory.Set(JpegDirectory.TagImageWidth, await reader.GetUInt16Async());

                var componentCount = await reader.GetByteAsync();

                directory.Set(JpegDirectory.TagNumberOfComponents, componentCount);

                for (var i = 0; i < componentCount; i++)
                {
                    var componentId = await reader.GetByteAsync();
                    var samplingFactorByte = await reader.GetByteAsync();
                    var quantizationTableNumber = await reader.GetByteAsync();
                    var component = new JpegComponent(componentId, samplingFactorByte, quantizationTableNumber);
                    directory.Set(JpegDirectory.TagComponentData1 + i, component);
                }
            }
            catch (IOException ex)
            {
                directory.AddError(ex.Message);
            }

            return directory;
        }
    }
}
