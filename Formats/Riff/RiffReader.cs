using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Riff
{
    public sealed class RiffReader
    {
        public static async Task ProcessRiffAsync(SequentialReader reader, IRiffHandler handler)
        {
            try
            {
                reader = reader.WithByteOrder(isMotorolaByteOrder: false);

                var fileFourCc = await reader.GetStringAsync(4, Encoding.ASCII);
                if (fileFourCc != "RIFF")
                    throw new RiffProcessingException("Invalid RIFF header: " + fileFourCc);

                int fileSize = await reader.GetInt32Async();
                int sizeLeft = fileSize;
                string identifier = await reader.GetStringAsync(4, Encoding.ASCII);
                sizeLeft -= 4;

                if (!handler.ShouldAcceptRiffIdentifier(identifier))
                    return;

                var maxPosition = reader.Position + sizeLeft;

                await ProcessChunksAsync(reader, maxPosition, handler);
            }
            catch (Exception e) when (e is ImageProcessingException or IOException)
            {
                handler.AddError(e.Message);
            }
        }

        private static async Task ProcessChunksAsync(SequentialReader reader, long maxPosition, IRiffHandler handler)
        {
            while (reader.Position < maxPosition - 8)
            {
                string chunkFourCc = await reader.GetStringAsync(4, Encoding.ASCII);
                int chunkSize = await reader.GetInt32Async();

                if (chunkSize < 0 || chunkSize + reader.Position > maxPosition)
                    throw new RiffProcessingException("Invalid RIFF chunk size");

                if (chunkFourCc == "LIST" || chunkFourCc == "RIFF")
                {
                    if (chunkSize < 4)
                        break;
                    string listName = await reader.GetStringAsync(4, Encoding.ASCII);
                    if (handler.ShouldAcceptList(listName))
                        await ProcessChunksAsync(reader, reader.Position + chunkSize - 4, handler);
                    else
                        reader.Skip(chunkSize - 4);
                }
                else
                {
                    if (handler.ShouldAcceptChunk(chunkFourCc))
                    {
                        handler.ProcessChunk(chunkFourCc, await reader.GetBytesAsync(chunkSize));
                    }
                    else
                    {
                        reader.Skip(chunkSize);
                    }

                    if (chunkSize % 2 == 1)
                    {
                        reader.Skip(1);
                    }
                }
            }
        }
    }
}
