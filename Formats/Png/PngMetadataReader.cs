using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Tiff;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;
using MetadataExtractor.Util;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor.Formats.Png
{
    public static class PngMetadataReader
    {
        private static readonly HashSet<PngChunkType> _desiredChunkTypes = new()
        {
            PngChunkType.IHDR,
            PngChunkType.PLTE,
            PngChunkType.tRNS,
            PngChunkType.cHRM,
            PngChunkType.sRGB,
            PngChunkType.gAMA,
            PngChunkType.iCCP,
            PngChunkType.bKGD,
            PngChunkType.tEXt,
            PngChunkType.zTXt,
            PngChunkType.iTXt,
            PngChunkType.tIME,
            PngChunkType.pHYs,
            PngChunkType.sBIT,
            PngChunkType.eXIf
        };

        public static async Task<DirectoryList> ReadMetadataAsync(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                directories.AddRange(await ReadMetadataAsync(stream));

            directories.Add(FileMetadataReader.Read(filePath));

            return directories;
        }

        public static async Task<DirectoryList> ReadMetadataAsync(Stream stream)
        {
            List<Directory>? directories = null;

            var chunks = PngChunkReader.ExtractAsync(new SequentialStreamReader(stream), _desiredChunkTypes);

            await foreach (var chunk in chunks)
            {
                directories ??= new List<Directory>();

                try
                {
                    await foreach (var c in ProcessChunkAsync(chunk))
                    {
                        directories.Add(c);
                    }
                }
                catch (Exception ex)
                {
                    directories.Add(new ErrorDirectory("Exception reading PNG chunk: " + ex.Message));
                }
            }

            return directories ?? Directory.EmptyList;
        }

        private static readonly Encoding _latin1Encoding = Encoding.GetEncoding("ISO-8859-1");
        private static readonly Encoding _utf8Encoding = Encoding.UTF8;

        private static async IAsyncEnumerable<Directory> ProcessChunkAsync(PngChunk chunk)
        {
            var chunkType = chunk.ChunkType;
            var bytes = chunk.Bytes;

            if (chunkType == PngChunkType.IHDR)
            {
                var header = new PngHeader(bytes);
                var directory = new PngDirectory(PngChunkType.IHDR);
                directory.Set(PngDirectory.TagImageWidth, header.ImageWidth);
                directory.Set(PngDirectory.TagImageHeight, header.ImageHeight);
                directory.Set(PngDirectory.TagBitsPerSample, header.BitsPerSample);
                directory.Set(PngDirectory.TagColorType, header.ColorType.NumericValue);
                directory.Set(PngDirectory.TagCompressionType, header.CompressionType);
                directory.Set(PngDirectory.TagFilterMethod, header.FilterMethod);
                directory.Set(PngDirectory.TagInterlaceMethod, header.InterlaceMethod);
                yield return directory;
            }
            else if (chunkType == PngChunkType.PLTE)
            {
                var directory = new PngDirectory(PngChunkType.PLTE);
                directory.Set(PngDirectory.TagPaletteSize, bytes.Length / 3);
                yield return directory;
            }
            else if (chunkType == PngChunkType.tRNS)
            {
                var directory = new PngDirectory(PngChunkType.tRNS);
                directory.Set(PngDirectory.TagPaletteHasTransparency, 1);
                yield return directory;
            }
            else if (chunkType == PngChunkType.sRGB)
            {
                int srgbRenderingIntent = unchecked((sbyte)bytes[0]);
                var directory = new PngDirectory(PngChunkType.sRGB);
                directory.Set(PngDirectory.TagSrgbRenderingIntent, srgbRenderingIntent);
                yield return directory;
            }
            else if (chunkType == PngChunkType.cHRM)
            {
                var chromaticities = new PngChromaticities(bytes);
                var directory = new PngChromaticitiesDirectory();
                directory.Set(PngChromaticitiesDirectory.TagWhitePointX, chromaticities.WhitePointX);
                directory.Set(PngChromaticitiesDirectory.TagWhitePointY, chromaticities.WhitePointY);
                directory.Set(PngChromaticitiesDirectory.TagRedX, chromaticities.RedX);
                directory.Set(PngChromaticitiesDirectory.TagRedY, chromaticities.RedY);
                directory.Set(PngChromaticitiesDirectory.TagGreenX, chromaticities.GreenX);
                directory.Set(PngChromaticitiesDirectory.TagGreenY, chromaticities.GreenY);
                directory.Set(PngChromaticitiesDirectory.TagBlueX, chromaticities.BlueX);
                directory.Set(PngChromaticitiesDirectory.TagBlueY, chromaticities.BlueY);
                yield return directory;
            }
            else if (chunkType == PngChunkType.gAMA)
            {
                var gammaInt = ByteConvert.ToInt32BigEndian(bytes);
                var directory = new PngDirectory(PngChunkType.gAMA);
                directory.Set(PngDirectory.TagGamma, gammaInt / 100000.0);
                yield return directory;
            }
            else if (chunkType == PngChunkType.iCCP)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var profileName = await reader.GetNullTerminatedStringValueAsync(maxLengthBytes: 79);
                var directory = new PngDirectory(PngChunkType.iCCP);
                directory.Set(PngDirectory.TagIccProfileName, profileName);
                var compressionMethod = await reader.GetSByteAsync();
                if (compressionMethod == 0)
                {
                    var bytesLeft = bytes.Length - profileName.Bytes.Length - 2;

                    reader.Skip(2);
                    bytesLeft -= 2;

                    var compressedProfile = await reader.GetBytesAsync(bytesLeft);

                    IccDirectory? iccDirectory = null;
                    Exception? ex = null;
                    try
                    {
                        using var inflaterStream = new DeflateStream(new MemoryStream(compressedProfile), CompressionMode.Decompress);
                        iccDirectory = IccReader.Extract(new IndexedCapturingReader(inflaterStream));
                        iccDirectory.Parent = directory;
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }

                    if (iccDirectory is not null)
                        yield return iccDirectory;
                    else if (ex is not null)
                        directory.AddError($"Exception decompressing PNG {nameof(PngChunkType.iCCP)} chunk: {ex.Message}");
                }
                else
                {
                    directory.AddError("Invalid compression method value");
                }
                yield return directory;
            }
            else if (chunkType == PngChunkType.bKGD)
            {
                var directory = new PngDirectory(PngChunkType.bKGD);
                directory.Set(PngDirectory.TagBackgroundColor, bytes);
                yield return directory;
            }
            else if (chunkType == PngChunkType.tEXt)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var keyword = (await reader.GetNullTerminatedStringValueAsync(maxLengthBytes: 79)).ToString(_latin1Encoding);
                var bytesLeft = bytes.Length - keyword.Length - 1;
                var value = await reader.GetNullTerminatedStringValueAsync(bytesLeft, _latin1Encoding);

                var textPairs = new KeyValuePair<string, StringValue>(keyword, value);
                var directory = new PngDirectory(PngChunkType.tEXt);
                directory.Set(PngDirectory.TagTextualData, textPairs);
                yield return directory;
            }
            else if (chunkType == PngChunkType.zTXt)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var keyword = (await reader.GetNullTerminatedStringValueAsync(maxLengthBytes: 79)).ToString(_latin1Encoding);
                var compressionMethod = await reader.GetSByteAsync();

                var bytesLeft = bytes.Length - keyword.Length - 1 - 1 - 1 - 1;
                byte[]? textBytes = null;
                if (compressionMethod == 0)
                {
                    if (!TryDeflate(bytes, bytesLeft, out textBytes, out string? errorMessage))
                    {
                        var directory = new PngDirectory(PngChunkType.zTXt);
                        directory.AddError($"Exception decompressing PNG {nameof(PngChunkType.zTXt)} chunk with keyword \"{keyword}\": {errorMessage}");
                        yield return directory;
                    }
                }
                else
                {
                    var directory = new PngDirectory(PngChunkType.zTXt);
                    directory.AddError("Invalid compression method value");
                    yield return directory;
                }

                if (textBytes is not null)
                {
                    await foreach (var directory in ProcessTextChunkAsync(keyword, textBytes))
                    {
                        yield return directory;
                    }
                }
            }
            else if (chunkType == PngChunkType.iTXt)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var keywordStringValue = await reader.GetNullTerminatedStringValueAsync(maxLengthBytes: 79);
                var keyword = keywordStringValue.ToString(_utf8Encoding);
                var compressionFlag = await reader.GetSByteAsync();
                var compressionMethod = await reader.GetSByteAsync();

                var languageTagBytes = await reader.GetNullTerminatedBytesAsync(bytes.Length);
                var translatedKeywordBytes = await reader.GetNullTerminatedBytesAsync(bytes.Length);

                var bytesLeft = bytes.Length - keywordStringValue.Bytes.Length - 1 - 1 - 1 - languageTagBytes.Length - 1 - translatedKeywordBytes.Length - 1;
                byte[]? textBytes = null;
                if (compressionFlag == 0)
                {
                    textBytes = await reader.GetNullTerminatedBytesAsync(bytesLeft);
                }
                else if (compressionFlag == 1)
                {
                    if (compressionMethod == 0)
                    {
                        if (!TryDeflate(bytes, bytesLeft, out textBytes, out string? errorMessage))
                        {
                            var directory = new PngDirectory(PngChunkType.iTXt);
                            directory.AddError($"Exception decompressing PNG {nameof(PngChunkType.iTXt)} chunk with keyword \"{keyword}\": {errorMessage}");
                            yield return directory;
                        }
                    }
                    else
                    {
                        var directory = new PngDirectory(PngChunkType.iTXt);
                        directory.AddError("Invalid compression method value");
                        yield return directory;
                    }
                }
                else
                {
                    var directory = new PngDirectory(PngChunkType.iTXt);
                    directory.AddError("Invalid compression flag value");
                    yield return directory;
                }

                if (textBytes is not null)
                {
                    await foreach (var directory in ProcessTextChunkAsync(keyword, textBytes))
                    {
                        yield return directory;
                    }
                }
            }
            else if (chunkType == PngChunkType.tIME)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var year = await reader.GetUInt16Async();
                var month = await reader.GetByteAsync();
                int day = await reader.GetByteAsync();
                int hour = await reader.GetByteAsync();
                int minute = await reader.GetByteAsync();
                int second = await reader.GetByteAsync();
                var directory = new PngDirectory(PngChunkType.tIME);
                if (DateUtil.IsValidDate(year, month, day) && DateUtil.IsValidTime(hour, minute, second))
                {
                    var time = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
                    directory.Set(PngDirectory.TagLastModificationTime, time);
                }
                else
                {
                    directory.AddError($"PNG tIME data describes an invalid date/time: year={year} month={month} day={day} hour={hour} minute={minute} second={second}");
                }
                yield return directory;
            }
            else if (chunkType == PngChunkType.pHYs)
            {
                var reader = new SequentialByteArrayReader(bytes);
                var pixelsPerUnitX = await reader.GetInt32Async();
                var pixelsPerUnitY = await reader.GetInt32Async();
                var unitSpecifier = await reader.GetSByteAsync();
                var directory = new PngDirectory(PngChunkType.pHYs);
                directory.Set(PngDirectory.TagPixelsPerUnitX, pixelsPerUnitX);
                directory.Set(PngDirectory.TagPixelsPerUnitY, pixelsPerUnitY);
                directory.Set(PngDirectory.TagUnitSpecifier, unitSpecifier);
                yield return directory;
            }
            else if (chunkType.Equals(PngChunkType.sBIT))
            {
                var directory = new PngDirectory(PngChunkType.sBIT);
                directory.Set(PngDirectory.TagSignificantBits, bytes);
                yield return directory;
            }
            else if (chunkType.Equals(PngChunkType.eXIf))
            {
                var directories = new List<Directory>();
                try
                {
                    await TiffReader.ProcessTiffAsync(
                        new ByteArrayReader(bytes),
                        new ExifTiffHandler(directories, exifStartOffset: 0));
                }
                catch (Exception ex)
                {
                    var directory = new PngDirectory(PngChunkType.eXIf);
                    directory.AddError(ex.Message);
                    directories.Add(directory);
                }

                foreach (var directory in directories)
                {
                    yield return directory;
                }
            }

            yield break;

            async IAsyncEnumerable<Directory> ProcessTextChunkAsync(string keyword, byte[] textBytes)
            {
                if (keyword == "XML:com.adobe.xmp")
                {
                    yield return XmpReader.Extract(textBytes);
                }
                else if (keyword == "Raw profile type xmp")
                {
                    if (TryProcessRawProfile(out int byteCount))
                    {
                        yield return XmpReader.Extract(textBytes, 0, byteCount);
                    }
                    else
                    {
                        yield return ReadTextDirectory(keyword, textBytes, chunkType);
                    }
                }
                else if (keyword == "Raw profile type exif" || keyword == "Raw profile type APP1")
                {
                    if (TryProcessRawProfile(out _))
                    {
                        int offset = 0;
                        if (ExifReader.StartsWithJpegExifPreamble(textBytes))
                            offset = ExifReader.JpegSegmentPreambleLength;

                        await foreach (var exifDirectory in ExifReader.ExtractAsync(new ByteArrayReader(textBytes, offset), exifStartOffset: 0))
                            yield return exifDirectory;
                    }
                    else
                    {
                        yield return ReadTextDirectory(keyword, textBytes, chunkType);
                    }
                }
                else if (keyword == "Raw profile type icc" || keyword == "Raw profile type icm")
                {
                    if (TryProcessRawProfile(out _))
                    {
                        yield return IccReader.Extract(new ByteArrayReader(textBytes));
                    }
                    else
                    {
                        yield return ReadTextDirectory(keyword, textBytes, chunkType);
                    }
                }
                else if (keyword == "Raw profile type iptc")
                {
                    if (TryProcessRawProfile(out int byteCount))
                    {
                        if (byteCount > 0 && textBytes[0] == IptcReader.IptcMarkerByte)
                        {
                            await foreach (var directory in IptcReader.ExtractAsync(new SequentialByteArrayReader(textBytes), byteCount))
                            {
                                yield return directory;
                            }
                        }
                        else
                        {
                            await foreach (var psDirectory in Photoshop.PhotoshopReader.ExtractAsync(new SequentialByteArrayReader(textBytes), byteCount))
                                yield return psDirectory;
                        }
                    }
                    else
                    {
                        yield return ReadTextDirectory(keyword, textBytes, chunkType);
                    }
                }
                else
                {
                    yield return ReadTextDirectory(keyword, textBytes, chunkType);
                }

                static PngDirectory ReadTextDirectory(string keyword, byte[] textBytes, PngChunkType pngChunkType)
                {
                    var encoding = _latin1Encoding;

                    if (pngChunkType == PngChunkType.iTXt)
                    {
                        encoding = _utf8Encoding;
                    }

                    var textPairs = new KeyValuePair<string, StringValue>(keyword, new StringValue(textBytes, encoding));
                    var directory = new PngDirectory(pngChunkType);
                    directory.Set(PngDirectory.TagTextualData, textPairs);
                    return directory;
                }

                bool TryProcessRawProfile(out int byteCount)
                {
                    if (textBytes.Length == 0 || textBytes[0] != '\n')
                    {
                        byteCount = default;
                        return false;
                    }

                    var i = 1;

                    while (i < textBytes.Length && textBytes[i] != '\n')
                        i++;

                    if (i == textBytes.Length)
                    {
                        byteCount = default;
                        return false;
                    }

                    int length = 0;
                    while (true)
                    {
                        i++;
                        var c = (char)textBytes[i];

                        if (c == ' ')
                            continue;
                        if (c == '\n')
                            break;

                        if (c is >= '0' and <= '9')
                        {
                            length *= 10;
                            length += c - '0';
                        }
                        else
                        {
                            byteCount = default;
                            return false;
                        }
                    }

                    i++;

                    const int RowCharCount = 72;
                    int charsInRow = RowCharCount;

                    for (int j = i; j < length + i; j++)
                    {
                        byte c = textBytes[j];

                        if (charsInRow-- == 0)
                        {
                            if (c != '\n')
                            {
                                byteCount = default;
                                return false;
                            }

                            charsInRow = RowCharCount;
                            continue;
                        }

                        if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
                        {
                            byteCount = default;
                            return false;
                        }
                    }

                    byteCount = length;
                    var writeIndex = 0;
                    charsInRow = RowCharCount;
                    while (length > 0)
                    {
                        var c1 = textBytes[i++];

                        if (charsInRow-- == 0)
                        {
                            charsInRow = RowCharCount;
                            continue;
                        }

                        var c2 = textBytes[i++];

                        charsInRow--;

                        var n1 = ParseHexNibble(c1);
                        var n2 = ParseHexNibble(c2);

                        length--;
                        textBytes[writeIndex++] = (byte)((n1 << 4) | n2);
                    }

                    return writeIndex == byteCount;

                    static int ParseHexNibble(int h)
                    {
                        if (h is >= '0' and <= '9')
                        {
                            return h - '0';
                        }

                        if (h is >= 'a' and <= 'f')
                        {
                            return 10 + (h - 'a');
                        }

                        if (h is >= 'A' and <= 'F')
                        {
                            return 10 + (h - 'A');
                        }

                        throw new InvalidOperationException();
                    }
                }
            }
        }

        private static bool TryDeflate(
            byte[] bytes,
            int bytesLeft,
            [NotNullWhen(returnValue: true)] out byte[]? textBytes,
            [NotNullWhen(returnValue: false)] out string? errorMessage)
        {
            using var inflaterStream = new DeflateStream(new MemoryStream(bytes, bytes.Length - bytesLeft, bytesLeft), CompressionMode.Decompress);
            try
            {
                var ms = new MemoryStream();

                inflaterStream.CopyTo(ms);

                textBytes = ms.ToArray();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                textBytes = default;
                return false;
            }

            errorMessage = default;
            return true;
        }
    }
}
