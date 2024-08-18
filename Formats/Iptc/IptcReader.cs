using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Iptc
{
    public sealed class IptcReader : IJpegSegmentMetadataReader
    {
        internal const byte IptcMarkerByte = 0x1c;

        ICollection<JpegSegmentType> IJpegSegmentMetadataReader.SegmentTypes { get; } = new[] { JpegSegmentType.AppD };

        public async IAsyncEnumerable<Directory> ReadJpegSegmentsAsync(IAsyncEnumerable<JpegSegment> segments)
        {
            await foreach (var segment in segments
            .Where(segment => segment.Bytes.Length != 0 && segment.Bytes[0] == IptcMarkerByte)
            .Select(segment => (Directory)ExtractAsync(new SequentialByteArrayReader(segment.Bytes), segment.Bytes.Length)))
            {
                yield return segment;
            }
        }

        public static async IAsyncEnumerable<IptcDirectory> ExtractAsync(SequentialReader reader, long length)
        {
            var directory = new IptcDirectory();

            var offset = 0;

            while (offset < length)
            {
                byte startByte;
                try
                {
                    startByte = await reader.GetByteAsync();
                    offset++;
                }
                catch (IOException)
                {
                    directory.AddError("Unable to read starting byte of IPTC tag");
                    break;
                }

                if (startByte != IptcMarkerByte)
                {
                    if (offset != length)
                        directory.AddError($"Invalid IPTC tag marker at offset {offset - 1}. Expected '0x{IptcMarkerByte:x2}' but got '0x{startByte:x}'.");
                    break;
                }

                if (offset + 4 > length)
                {
                    directory.AddError("Too few bytes remain for a valid IPTC tag");
                    break;
                }

                int directoryType;
                int tagType;
                int tagByteCount;
                try
                {
                    directoryType = await reader.GetByteAsync();
                    tagType = await reader.GetByteAsync();
                    tagByteCount = await reader.GetUInt16Async();
                    if (tagByteCount > 0x7FFF)
                    {
                        tagByteCount = ((tagByteCount & 0x7FFF) << 16) | await reader.GetUInt16Async();
                        offset += 2;
                    }
                    offset += 4;
                }
                catch (IOException)
                {
                    directory.AddError("IPTC data segment ended mid-way through tag descriptor");
                    break;
                }

                if (offset + tagByteCount > length)
                {
                    directory.AddError("Data for tag extends beyond end of IPTC segment");
                    break;
                }

                try
                {
                    ProcessTagAsync(reader, directory, directoryType, tagType, tagByteCount);
                }
                catch (IOException)
                {
                    directory.AddError("Error processing IPTC tag");
                    break;
                }

                offset += tagByteCount;
            }

            yield return directory;
        }

        private static async void ProcessTagAsync(SequentialReader reader, Directory directory, int directoryType, int tagType, int tagByteCount)
        {
            var tagIdentifier = tagType | (directoryType << 8);

            if (tagByteCount == 0)
            {
                directory.Set(tagIdentifier, string.Empty);
                return;
            }

            switch (tagIdentifier)
            {
                case IptcDirectory.TagCodedCharacterSet:
                    {
                        var bytes = await reader.GetBytesAsync(tagByteCount);
                        var charset = Iso2022Converter.ConvertEscapeSequenceToEncodingName(bytes);
                        charset ??= Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        directory.Set(tagIdentifier, charset);
                        return;
                    }

                case IptcDirectory.TagEnvelopeRecordVersion:
                case IptcDirectory.TagApplicationRecordVersion:
                case IptcDirectory.TagFileVersion:
                case IptcDirectory.TagArmVersion:
                case IptcDirectory.TagProgramVersion:
                    {
                        if (tagByteCount == 2)
                        {
                            var shortValue = await reader.GetUInt16Async();
                            reader.Skip(tagByteCount - 2);
                            directory.Set(tagIdentifier, shortValue);
                            return;
                        }
                        break;
                    }

                case IptcDirectory.TagUrgency:
                    {
                        directory.Set(tagIdentifier, await reader.GetByteAsync());
                        reader.Skip(tagByteCount - 1);
                        return;
                    }
            }

            var encodingName = directory.GetString(IptcDirectory.TagCodedCharacterSet);
            Encoding? encoding = null;
            if (encodingName is not null)
            {
                try
                {
                    encoding = Encoding.GetEncoding(encodingName);
                }
                catch (ArgumentException)
                { }
            }

            StringValue str;
            if (encoding is not null)
                str = await reader.GetStringValueAsync(tagByteCount, encoding);
            else
            {
                var bytes = await reader.GetBytesAsync(tagByteCount);
                encoding = Iso2022Converter.GuessEncoding(bytes);
                str = new StringValue(bytes, encoding);
            }

            if (directory.ContainsArrayTag(tagIdentifier))
            {
                var oldStrings = directory.GetArray<StringValue[]>(tagIdentifier);

                StringValue[] newStrings;
                if (oldStrings is null)
                {
                    newStrings = new StringValue[1];
                }
                else
                {
                    newStrings = new StringValue[oldStrings.Length + 1];
                    Array.Copy(oldStrings, 0, newStrings, 0, oldStrings.Length);
                }
                newStrings[^1] = str;
                directory.Set(tagIdentifier, newStrings);
            }
            else
            {
                directory.Set(tagIdentifier, str);
            }
        }
    }
}
