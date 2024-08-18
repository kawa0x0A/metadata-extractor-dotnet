using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Util;
using System.Text;
using XmpCore;
using XmpCore.Options;

namespace MetadataExtractor.Formats.Xmp
{
    public sealed class XmpReader : IJpegSegmentMetadataReader
    {
        public const string JpegSegmentPreamble = "http://ns.adobe.com/xap/1.0/\0";
        public const string JpegSegmentPreambleExtension = "http://ns.adobe.com/xmp/extension/\0";

        private static byte[] JpegSegmentPreambleBytes { get; } = Encoding.UTF8.GetBytes(JpegSegmentPreamble);
        private static byte[] JpegSegmentPreambleExtensionBytes { get; } = Encoding.UTF8.GetBytes(JpegSegmentPreambleExtension);

        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.App1 };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            var segmentList = await segments.ToListAsync();

            foreach (var segment in segmentList)
            {
                if (IsXmpSegment(segment))
                {
                    yield return Extract(segment.Bytes, JpegSegmentPreambleBytes.Length, segment.Bytes.Length - JpegSegmentPreambleBytes.Length);
                }
            }

            var extensionGroups = segmentList.Where(IsExtendedXmpSegment).GroupBy(GetExtendedDataGuid);

            foreach (var extensionGroup in extensionGroups)
            {
                var buffer = new MemoryStream();
                foreach (var segment in extensionGroup)
                {
                    var n = JpegSegmentPreambleExtensionBytes.Length + 32 + 4 + 4;
                    buffer.Write(segment.Bytes, n, segment.Bytes.Length - n);
                }

                buffer.Position = 0;
                var directory = new XmpDirectory();
                var xmpMeta = XmpMetaFactory.Parse(buffer);
                directory.SetXmpMeta(xmpMeta);
                yield return directory;
            }
        }

        private static string GetExtendedDataGuid(JpegSegment segment) => Encoding.UTF8.GetString(segment.Bytes, JpegSegmentPreambleExtensionBytes.Length, 32);

        private static bool IsXmpSegment(JpegSegment segment) => segment.Bytes.StartsWith(JpegSegmentPreambleBytes);
        private static bool IsExtendedXmpSegment(JpegSegment segment) => segment.Bytes.StartsWith(JpegSegmentPreambleExtensionBytes);

        public static XmpDirectory Extract(byte[] xmpBytes) => Extract(xmpBytes, 0, xmpBytes.Length);

        public static XmpDirectory Extract(byte[] xmpBytes, int offset, int length)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Must be zero or greater.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Must be zero or greater.");
            if (xmpBytes.Length < offset + length)
                throw new ArgumentException("Extends beyond length of byte array.", nameof(length));

            while (xmpBytes[offset + length - 1] == 0)
                length--;

            var directory = new XmpDirectory();
            try
            {
                var parseOptions = new ParseOptions();
                parseOptions.SetXMPNodesToLimit(new Dictionary<string, int>() { { "photoshop:DocumentAncestors", 1000 } });

                var xmpMeta = XmpMetaFactory.ParseFromBuffer(xmpBytes, offset, length, parseOptions);
                directory.SetXmpMeta(xmpMeta);
            }
            catch (XmpException e)
            {
                directory.AddError("Error processing XMP data: " + e.Message);
            }
            return directory;
        }
    }
}
