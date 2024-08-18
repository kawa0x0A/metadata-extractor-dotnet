using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Tiff;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Photoshop
{
    public class PhotoshopTiffHandler : ExifTiffHandler
    {
        private const int TagXmp = 0x02BC;

        private const int TagPhotoshopImageResources = 0x8649;

        private const int TagIccProfiles = 0x8773;

        public PhotoshopTiffHandler(List<Directory> directories) : base(directories, exifStartOffset: 0)
        {
        }

        public override async Task<bool> CustomProcessTagAsync(TiffReaderContext context, int tagId, int valueOffset, int byteCount)
        {
            switch (tagId)
            {
                case TagXmp:
                    Directories.Add(XmpReader.Extract(context.Reader.GetBytes(valueOffset, byteCount)));
                    return true;
                case TagPhotoshopImageResources:
                    await foreach (var directory in PhotoshopReader.ExtractAsync(new SequentialByteArrayReader(context.Reader.GetBytes(valueOffset, byteCount)), byteCount))
                    {
                        Directories.Add(directory);
                    }
                    return true;
                case TagIccProfiles:
                    Directories.Add(IccReader.Extract(new ByteArrayReader(context.Reader.GetBytes(valueOffset, byteCount))));
                    return true;
            }

            return await base.CustomProcessTagAsync(context, tagId, valueOffset, byteCount);
        }
    }
}
