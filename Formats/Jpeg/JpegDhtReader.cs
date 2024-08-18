using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegDhtReader : IJpegSegmentMetadataReader
    {
        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.Dht };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            HuffmanTablesDirectory? directory = null;

            await foreach (var segment in segments)
            {
                directory ??= new HuffmanTablesDirectory();

                await ExtractAsync(new SequentialByteArrayReader(segment.Bytes), directory);
            }

            if (directory is not null)
            {
                yield return directory;
                yield break;
            }
        }

        public static async Task ExtractAsync(SequentialReader reader, HuffmanTablesDirectory directory)
        {
            try
            {
                while (reader.Available() > 0)
                {
                    byte header = await reader.GetByteAsync();
                    HuffmanTableClass tableClass = HuffmanTable.TypeOf((header & 0xF0) >> 4);
                    int tableDestinationId = header & 0xF;

                    byte[] lBytes = await GetBytesAsync(reader, 16);
                    int vCount = 0;
                    foreach (byte b in lBytes)
                    {
                        vCount += (b & 0xFF);
                    }
                    byte[] vBytes = await GetBytesAsync(reader, vCount);
                    directory.AddTable(new HuffmanTable(tableClass, tableDestinationId, lBytes, vBytes));
                }
            }
            catch (IOException me)
            {
                directory.AddError(me.ToString());
            }
        }

        private static async Task<byte[]> GetBytesAsync(SequentialReader reader, int count)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                byte b = await reader.GetByteAsync();
                if (b == 0xFF)
                {
                    byte stuffing = await reader.GetByteAsync();
                    if (stuffing != 0x00)
                    {
                        throw new MetadataException("Marker " + (JpegSegmentType)stuffing + " found inside DHT segment");
                    }
                }
                bytes[i] = b;
            }
            return bytes;
        }
    }
}
