using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Jfif
{
    public sealed class JfifReader : JpegSegmentWithPreambleMetadataReader
    {
        public const string JpegSegmentPreamble = "JFIF";

        protected override byte[] PreambleBytes { get; } = Encoding.ASCII.GetBytes(JpegSegmentPreamble);

        public override ICollection<JpegSegmentType> SegmentTypes { get; } = [JpegSegmentType.App0];

        protected override async IAsyncEnumerable<Directory> ExtractAsync(byte[] segmentBytes, int preambleLength)
        {
            yield return await Task.FromResult(ExtractAsync(new ByteArrayReader(segmentBytes)));
        }

        public static JfifDirectory ExtractAsync(IndexedReader reader)
        {
            var directory = new JfifDirectory();

            try
            {
#pragma warning disable format
                directory.Set(JfifDirectory.TagVersion,     reader.GetUInt16(JfifDirectory.TagVersion));
                directory.Set(JfifDirectory.TagUnits,       reader.GetByte(JfifDirectory.TagUnits));
                directory.Set(JfifDirectory.TagResX,        reader.GetUInt16(JfifDirectory.TagResX));
                directory.Set(JfifDirectory.TagResY,        reader.GetUInt16(JfifDirectory.TagResY));
                directory.Set(JfifDirectory.TagThumbWidth,  reader.GetByte(JfifDirectory.TagThumbWidth));
                directory.Set(JfifDirectory.TagThumbHeight, reader.GetByte(JfifDirectory.TagThumbHeight));
#pragma warning restore format
            }
            catch (IOException e)
            {
                directory.AddError(e.Message);
            }

            return directory;
        }
    }
}
