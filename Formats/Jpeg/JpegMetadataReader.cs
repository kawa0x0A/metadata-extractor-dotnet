using MetadataExtractor.Formats.Adobe;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Flir;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jfif;
using MetadataExtractor.Formats.Jfxx;
using MetadataExtractor.Formats.Photoshop;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;
using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor.Formats.Jpeg
{
    public static class JpegMetadataReader
    {
        private static readonly ICollection<IJpegSegmentMetadataReader> _allReaders = AllReaders.ToList();

        public static IEnumerable<IJpegSegmentMetadataReader> AllReaders
        {
            get
            {
                yield return new JpegReader();
                yield return new JpegCommentReader();
                yield return new JfifReader();
                yield return new JfxxReader();
                yield return new ExifReader();
                yield return new XmpReader();
                yield return new IccReader();
                yield return new PhotoshopReader();
                yield return new DuckyReader();
                yield return new IptcReader();
                yield return new AdobeJpegReader();
                yield return new JpegDhtReader();
                yield return new JpegDnlReader();
                yield return new FlirReader();
            }
        }

        public static async Task<DirectoryList> ReadMetadataAsync(Stream stream, ICollection<IJpegSegmentMetadataReader>? readers = null)
        {
            return await ProcessAsync(stream, readers);
        }

        public static async Task<DirectoryList> ReadMetadataAsync(string filePath, ICollection<IJpegSegmentMetadataReader>? readers = null)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                directories.AddRange(await ReadMetadataAsync(stream, readers));

            directories.Add(FileMetadataReader.Read(filePath));

            return directories;
        }

        public static async Task<DirectoryList> ProcessAsync(Stream stream, ICollection<IJpegSegmentMetadataReader>? readers = null)
        {
            readers ??= _allReaders;

            var segmentTypes = new HashSet<JpegSegmentType>(readers.SelectMany(reader => reader.SegmentTypes));

            var segments = JpegSegmentReader.ReadSegmentsAsync(new SequentialStreamReader(stream), segmentTypes);

            return ProcessJpegSegments(readers, await segments.ToListAsync());
        }

        public static DirectoryList ProcessJpegSegments(IEnumerable<IJpegSegmentMetadataReader> readers, ICollection<JpegSegment> segments)
        {
            var directories = new List<Directory>();

            foreach (var reader in readers)
            {
                var readerSegmentTypes = reader.SegmentTypes;
                var readerSegments = segments.Where(s => readerSegmentTypes.Contains(s.Type));
                directories.AddRange(reader.ReadJpegSegmentsAsync(readerSegments.ToAsyncEnumerable()).ToBlockingEnumerable());
            }

            return directories;
        }
    }
}
