using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Photoshop
{
    public sealed class DuckyReader : IJpegSegmentMetadataReader
    {
        public const string JpegSegmentPreamble = "Ducky";

        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.AppC };

        public IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            return segments
                .Where(segment => segment.Bytes.Length >= JpegSegmentPreamble.Length && JpegSegmentPreamble == Encoding.UTF8.GetString(segment.Bytes, 0, JpegSegmentPreamble.Length))
                .SelectAwait(async segment => (Directory)await ExtractAsync(new SequentialByteArrayReader(segment.Bytes, JpegSegmentPreamble.Length)));
        }

        public static async Task<DuckyDirectory> ExtractAsync(SequentialReader reader)
        {
            var directory = new DuckyDirectory();

            try
            {
                while (true)
                {
                    var tag = await reader.GetUInt16Async();

                    if (tag == 0)
                        break;

                    int length = await reader.GetUInt16Async();

                    switch (tag)
                    {
                        case DuckyDirectory.TagQuality:
                            {
                                if (length != 4)
                                {
                                    directory.AddError("Unexpected length for the quality tag");
                                    return directory;
                                }
                                directory.Set(tag, await reader.GetUInt32Async());
                                break;
                            }
                        case DuckyDirectory.TagComment:
                        case DuckyDirectory.TagCopyright:
                            {
                                reader.Skip(4);
                                length -= 4;
                                if (length < 0)
                                {
                                    directory.AddError("Unexpected length for a text tag");
                                    return directory;
                                }
                                directory.Set(tag, await reader.GetStringAsync(length, Encoding.BigEndianUnicode));
                                break;
                            }
                        default:
                            {
                                directory.Set(tag, await reader.GetBytesAsync(length));
                                break;
                            }
                    }
                }
            }
            catch (IOException e)
            {
                directory.AddError(e.Message);
            }

            return directory;
        }
    }
}
