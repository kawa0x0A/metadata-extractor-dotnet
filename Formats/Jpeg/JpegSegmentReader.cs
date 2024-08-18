using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Jpeg
{
    public static class JpegSegmentReader
    {
        public static async IAsyncEnumerable<MetadataExtractor.Formats.Jpeg.JpegSegment> ReadSegmentsAsync(string filePath, ICollection<JpegSegmentType>? segmentTypes = null)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            await foreach (var segment in ReadSegmentsAsync(new SequentialStreamReader(stream), segmentTypes))
            {
                yield return segment;
            }
        }

        public static async IAsyncEnumerable<JpegSegment> ReadSegmentsAsync(SequentialReader reader, ICollection<JpegSegmentType>? segmentTypes = null)
        {
            if (!reader.IsMotorolaByteOrder)
                throw new JpegProcessingException("Must be big-endian/Motorola byte order.");

            var magicNumber = await reader.GetUInt16Async();

            if (magicNumber != 0xFFD8)
                throw new JpegProcessingException($"JPEG data is expected to begin with 0xFFD8 (ÿØ) not 0x{magicNumber:X4}");

            do
            {
                var segmentIdentifier = await reader.GetByteAsync();
                var segmentTypeByte = await reader.GetByteAsync();

                while (segmentIdentifier != 0xFF || segmentTypeByte == 0xFF || segmentTypeByte == 0)
                {
                    segmentIdentifier = segmentTypeByte;
                    segmentTypeByte = await reader.GetByteAsync();
                }

                var segmentType = (JpegSegmentType)segmentTypeByte;

                if (segmentType == JpegSegmentType.Sos)
                {
                    yield break;
                }

                if (segmentType == JpegSegmentType.Eoi)
                {
                    yield break;
                }

                var segmentLength = (int)await reader.GetUInt16Async();

                segmentLength -= 2;

                if (segmentLength < 0)
                    throw new JpegProcessingException("JPEG segment size would be less than zero");

                if (segmentTypes is null || segmentTypes.Contains(segmentType))
                {
                    var segmentOffset = reader.Position;
                    var segmentBytes = await reader.GetBytesAsync(segmentLength);

                    yield return new JpegSegment(segmentType, segmentBytes, segmentOffset);
                }
                else
                {
                    if (!reader.TrySkip(segmentLength))
                        yield break;
                }
            }
            while (true);
        }
    }
}
