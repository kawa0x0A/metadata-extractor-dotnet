using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Jfxx
{
    public sealed class JfxxReader : JpegSegmentWithPreambleMetadataReader
    {
        public const string JpegSegmentPreamble = "JFXX";

        protected override byte[] PreambleBytes { get; } = Encoding.ASCII.GetBytes(JpegSegmentPreamble);

        public override ICollection<JpegSegmentType> SegmentTypes { get; } = [JpegSegmentType.App0];

        protected override async IAsyncEnumerable<Directory> ExtractAsync(byte[] segmentBytes, int preambleLength)
        {
            yield return await Task.FromResult(ExtractAsync(new ByteArrayReader(segmentBytes)));
        }

        public static JfxxDirectory ExtractAsync(IndexedReader reader)
        {
            var directory = new JfxxDirectory();

            try
            {
                directory.Set(JfxxDirectory.TagExtensionCode, reader.GetByte(JfxxDirectory.TagExtensionCode));
            }
            catch (IOException e)
            {
                directory.AddError(e.Message);
            }

            return directory;
        }
    }
}
