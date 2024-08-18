using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Png
{
    public sealed class PngChunkReader
    {
        private static readonly byte[] _pngSignatureBytes = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        public static async IAsyncEnumerable<PngChunk> ExtractAsync(SequentialReader reader, ICollection<PngChunkType> desiredChunkTypes)
        {
            reader = reader.WithByteOrder(isMotorolaByteOrder: true);

            if (!_pngSignatureBytes.SequenceEqual(await reader.GetBytesAsync(_pngSignatureBytes.Length)))
                throw new PngProcessingException("PNG signature mismatch");

            var seenImageHeader = false;
            var seenImageTrailer = false;
            var chunks = new List<PngChunk>();
            var seenChunkTypes = new HashSet<PngChunkType>();

            while (!seenImageTrailer)
            {
                var chunkDataLength = await reader.GetInt32Async();

                if (chunkDataLength < 0)
                    throw new PngProcessingException("PNG chunk length exceeds maximum");

                if (chunkDataLength == 0)
                {
                    break;
                }

                var chunkType = new PngChunkType(await reader.GetBytesAsync(4));
                var willStoreChunk = desiredChunkTypes is null || desiredChunkTypes.Contains(chunkType);

                byte[]? chunkData;
                if (willStoreChunk)
                {
                    chunkData = await reader.GetBytesAsync(chunkDataLength);
                }
                else
                {
                    chunkData = null;
                    reader.Skip(Math.Min(chunkDataLength, reader.Available()));
                }

                if (reader.Available() >= 4)
                {
                    reader.Skip(4);
                }

                if (willStoreChunk && seenChunkTypes.Contains(chunkType) && !chunkType.AreMultipleAllowed)
                    throw new PngProcessingException($"Observed multiple instances of PNG chunk '{chunkType}', for which multiples are not allowed");

                if (chunkType.Equals(PngChunkType.IHDR))
                    seenImageHeader = true;
                else if (!seenImageHeader)
                    throw new PngProcessingException($"First chunk should be '{PngChunkType.IHDR}', but '{chunkType}' was observed");

                if (chunkType.Equals(PngChunkType.IEND))
                    seenImageTrailer = true;

                if (chunkData is not null)
                    chunks.Add(new PngChunk(chunkType, chunkData));

                seenChunkTypes.Add(chunkType);
            }

            foreach (var chunk in chunks)
            {
                yield return chunk;
            }
        }
    }
}
