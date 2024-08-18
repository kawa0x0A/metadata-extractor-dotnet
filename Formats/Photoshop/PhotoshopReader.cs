using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Photoshop
{
    public sealed class PhotoshopReader : JpegSegmentWithPreambleMetadataReader
    {
        public const string JpegSegmentPreamble = "Photoshop 3.0";

        protected override byte[] PreambleBytes { get; } = Encoding.ASCII.GetBytes(JpegSegmentPreamble);

        public override ICollection<JpegSegmentType> SegmentTypes { get; } = new[] { JpegSegmentType.AppD };

        protected override async IAsyncEnumerable<Directory> ExtractAsync(byte[] segmentBytes, int preambleLength)
        {
            if (segmentBytes.Length >= preambleLength + 1)
            {
                await foreach (var a in ExtractAsync(
                    reader: new SequentialByteArrayReader(segmentBytes, preambleLength + 1),
                    length: segmentBytes.Length - preambleLength - 1))
                {
                    yield return a;
                }
            }
        }

        public static async IAsyncEnumerable<Directory> ExtractAsync(SequentialReader reader, int length)
        {
            var photoshopDirectory = new PhotoshopDirectory();

            var directories = new List<Directory> { photoshopDirectory };

            var pos = 0;
            int clippingPathCount = 0;
            while (pos < length)
            {
                try
                {
                    var signature = await reader.GetStringAsync(4, Encoding.UTF8);
                    pos += 4;

                    var tagType = await reader.GetUInt16Async();
                    pos += 2;

                    var descriptionLength = await reader.GetByteAsync();
                    pos += 1;

                    if (descriptionLength + pos > length)
                        throw new ImageProcessingException("Invalid string length");

                    var description = new StringBuilder();

                    while (descriptionLength > 0)
                    {
                        description.Append((char)await reader.GetByteAsync());
                        pos++;
                        descriptionLength--;
                    }

                    if (pos % 2 != 0)
                    {
                        reader.Skip(1);
                        pos++;
                    }

                    var byteCount = await reader.GetInt32Async();
                    pos += 4;

                    var tagBytes = await reader.GetBytesAsync(byteCount);
                    pos += byteCount;

                    if (pos % 2 != 0)
                    {
                        reader.Skip(1);
                        pos++;
                    }

                    if (signature != "8BIM")
                        continue;

                    switch (tagType)
                    {
                        case PhotoshopDirectory.TagIptc:
                            await foreach (var iptcDirectory in IptcReader.ExtractAsync(new SequentialByteArrayReader(tagBytes), tagBytes.Length))
                            {
                                iptcDirectory.Parent = photoshopDirectory;
                                directories.Add(iptcDirectory);
                            }
                            break;
                        case PhotoshopDirectory.TagIccProfileBytes:
                            var iccDirectory = IccReader.Extract(new ByteArrayReader(tagBytes));
                            iccDirectory.Parent = photoshopDirectory;
                            directories.Add(iccDirectory);
                            break;
                        case PhotoshopDirectory.TagExifData1:
                        case PhotoshopDirectory.TagExifData3:
                            var exifDirectories = ExifReader.ExtractAsync(new ByteArrayReader(tagBytes), exifStartOffset: 0);
                            await foreach (var exifDirectory in exifDirectories.Where(d => d.Parent is null))
                                exifDirectory.Parent = photoshopDirectory;
                            await foreach (var exifDirectory in exifDirectories)
                                directories.Add(exifDirectory);
                            break;
                        case PhotoshopDirectory.TagXmpData:
                            var xmpDirectory = XmpReader.Extract(tagBytes);
                            xmpDirectory.Parent = photoshopDirectory;
                            directories.Add(xmpDirectory);
                            break;
                        default:
                            if (tagType is >= PhotoshopDirectory.TagClippingPathBlockStart and <= PhotoshopDirectory.TagClippingPathBlockEnd)
                            {
                                clippingPathCount++;
                                Array.Resize(ref tagBytes, tagBytes.Length + description.Length + 1);

                                for (int i = tagBytes.Length - description.Length - 1; i < tagBytes.Length; i++)
                                {
                                    if (i % (tagBytes.Length - description.Length - 1 + description.Length) == 0)
                                        tagBytes[i] = (byte)description.Length;
                                    else
                                        tagBytes[i] = (byte)description[i - (tagBytes.Length - description.Length - 1)];
                                }
                                PhotoshopDirectory.TagNameMap[PhotoshopDirectory.TagClippingPathBlockStart + clippingPathCount - 1] = "Path Info " + clippingPathCount;
                                photoshopDirectory.Set(PhotoshopDirectory.TagClippingPathBlockStart + clippingPathCount - 1, tagBytes);
                            }
                            else
                                photoshopDirectory.Set(tagType, tagBytes);
                            break;
                    }

                    if (tagType is >= 0x0fa0 and <= 0x1387)
                        PhotoshopDirectory.TagNameMap[tagType] = $"Plug-in {tagType - 0x0fa0 + 1} Data";
                }
                catch (Exception ex)
                {
                    photoshopDirectory.AddError(ex.Message);
                    break;
                }
            }

            foreach (var directory in directories)
            {
                yield return directory;
            }
        }
    }
}
