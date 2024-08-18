using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using MetadataExtractor.Util;
using System.Text;

namespace MetadataExtractor.Formats.Icc
{
    public sealed class IccReader : IJpegSegmentMetadataReader
    {
        public const string JpegSegmentPreamble = "ICC_PROFILE";
        private static readonly byte[] _jpegSegmentPreambleBytes = Encoding.UTF8.GetBytes(JpegSegmentPreamble);

        private const int JpegSegmentPreambleLength = 14;

        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = [JpegSegmentType.App2];

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            var iccSegments = await segments.Where(segment => segment.Bytes.Length > JpegSegmentPreambleLength && segment.Bytes.StartsWith(_jpegSegmentPreambleBytes)).ToListAsync();

            if (iccSegments.Count == 0)
                yield break;

            byte[] buffer;
            if (iccSegments.Count == 1)
            {
                buffer = new byte[iccSegments[0].Bytes.Length - JpegSegmentPreambleLength];
                Array.Copy(iccSegments[0].Bytes, JpegSegmentPreambleLength, buffer, 0, iccSegments[0].Bytes.Length - JpegSegmentPreambleLength);
            }
            else
            {
                var totalLength = iccSegments.Sum(s => s.Bytes.Length - JpegSegmentPreambleLength);
                buffer = new byte[totalLength];
                for (int i = 0, pos = 0; i < iccSegments.Count; i++)
                {
                    var segment = iccSegments[i];
                    Array.Copy(segment.Bytes, JpegSegmentPreambleLength, buffer, pos, segment.Bytes.Length - JpegSegmentPreambleLength);
                    pos += segment.Bytes.Length - JpegSegmentPreambleLength;
                }
            }

            yield return Extract(new ByteArrayReader(buffer));
        }

        public static IccDirectory Extract(IndexedReader reader)
        {
            var directory = new IccDirectory();

            try
            {
                var profileByteCount = reader.GetInt32(IccDirectory.TagProfileByteCount);
                directory.Set(IccDirectory.TagProfileByteCount, profileByteCount);

                Set4ByteString(directory, IccDirectory.TagCmmType, reader);
                SetInt32(directory, IccDirectory.TagProfileVersion, reader);
                Set4ByteString(directory, IccDirectory.TagProfileClass, reader);
                Set4ByteString(directory, IccDirectory.TagColorSpace, reader);
                Set4ByteString(directory, IccDirectory.TagProfileConnectionSpace, reader);
                SetDate(directory, IccDirectory.TagProfileDateTime, reader);
                Set4ByteString(directory, IccDirectory.TagSignature, reader);
                Set4ByteString(directory, IccDirectory.TagPlatform, reader);
                SetInt32(directory, IccDirectory.TagCmmFlags, reader);
                Set4ByteString(directory, IccDirectory.TagDeviceMake, reader);

                var model = reader.GetInt32(IccDirectory.TagDeviceModel);

                if (model != 0)
                {
                    if (model <= 0x20202020)
                    {
                        directory.Set(IccDirectory.TagDeviceModel, model);
                    }
                    else
                    {
                        directory.Set(IccDirectory.TagDeviceModel, GetStringFromUInt32(unchecked((uint)model)));
                    }
                }

                SetInt32(directory, IccDirectory.TagRenderingIntent, reader);
                SetInt64(directory, IccDirectory.TagDeviceAttr, reader);

                var xyz = new[] { reader.GetS15Fixed16(IccDirectory.TagXyzValues), reader.GetS15Fixed16(IccDirectory.TagXyzValues + 4), reader.GetS15Fixed16(IccDirectory.TagXyzValues + 8) };
                directory.Set(IccDirectory.TagXyzValues, xyz);

                var tagCount = reader.GetInt32(IccDirectory.TagTagCount);
                directory.Set(IccDirectory.TagTagCount, tagCount);

                for (var i = 0; i < tagCount; i++)
                {
                    var pos = IccDirectory.TagTagCount + 4 + i * 12;
                    var tagType = reader.GetInt32(pos);
                    var tagPtr = reader.GetInt32(pos + 4);
                    var tagLen = reader.GetInt32(pos + 8);
                    var b = reader.GetBytes(tagPtr, tagLen);
                    directory.Set(tagType, b);
                }
            }
            catch (Exception ex)
            {
                directory.AddError("Exception reading ICC profile: " + ex.Message);
            }

            return directory;
        }

        private static void Set4ByteString(Directory directory, int tagType, IndexedReader reader)
        {
            var i = reader.GetUInt32(tagType);
            if (i != 0)
                directory.Set(tagType, GetStringFromUInt32(i));
        }

        private static void SetInt32(Directory directory, int tagType, IndexedReader reader)
        {
            var i = reader.GetInt32(tagType);
            if (i != 0)
                directory.Set(tagType, i);
        }

        private static void SetInt64(Directory directory, int tagType, IndexedReader reader)
        {
            var l = reader.GetInt64(tagType);
            if (l != 0)
                directory.Set(tagType, l);
        }

        private static void SetDate(IccDirectory directory, int tagType, IndexedReader reader)
        {
            var year = reader.GetUInt16(tagType);
            var month = reader.GetUInt16(tagType + 2);
            var day = reader.GetUInt16(tagType + 4);
            var hours = reader.GetUInt16(tagType + 6);
            var minutes = reader.GetUInt16(tagType + 8);
            var seconds = reader.GetUInt16(tagType + 10);

            if (DateUtil.IsValidDate(year, month, day) &&
                DateUtil.IsValidTime(hours, minutes, seconds))
                directory.Set(tagType, new DateTime(year, month, day, hours, minutes, seconds, kind: DateTimeKind.Utc));
            else
                directory.AddError($"ICC data describes an invalid date/time: year={year} month={month} day={day} hour={hours} minute={minutes} second={seconds}");
        }

        public static string GetStringFromUInt32(uint d)
        {
            var b = new[]
            {
                unchecked((byte)(d >> 24)),
                unchecked((byte)(d >> 16)),
                unchecked((byte)(d >> 8)),
                unchecked((byte)d)
            };

            return Encoding.UTF8.GetString(b, 0, b.Length);
        }
    }
}
