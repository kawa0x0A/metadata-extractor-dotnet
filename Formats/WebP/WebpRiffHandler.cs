using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Riff;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.WebP
{
    public sealed class WebPRiffHandler(List<Directory> directories) : IRiffHandler
    {
        public bool ShouldAcceptRiffIdentifier(string identifier) => identifier == "WEBP";

        public bool ShouldAcceptChunk(string fourCc) => fourCc == "VP8X" ||
                                                        fourCc == "VP8L" ||
                                                        fourCc == "VP8 " ||
                                                        fourCc == "EXIF" ||
                                                        fourCc == "ICCP" ||
                                                        fourCc == "XMP ";

        public bool ShouldAcceptList(string fourCc) => false;

        public void ProcessChunk(string fourCc, byte[] payload)
        {
            switch (fourCc)
            {
                case "EXIF":
                    {
                        var reader = ExifReader.StartsWithJpegExifPreamble(payload)
                            ? new ByteArrayReader(payload, ExifReader.JpegSegmentPreambleLength)
                            : new ByteArrayReader(payload);
                        directories.AddRange(ExifReader.ExtractAsync(reader, exifStartOffset: 0).ToBlockingEnumerable());
                        break;
                    }
                case "ICCP":
                    {
                        directories.Add(IccReader.Extract(new ByteArrayReader(payload)));
                        break;
                    }
                case "XMP ":
                    {
                        directories.Add(XmpReader.Extract(payload));
                        break;
                    }
                case "VP8X":
                    {
                        if (payload.Length != 10)
                            break;

                        string? error = null;
                        var reader = new ByteArrayReader(payload, isMotorolaByteOrder: false);
                        var isAnimation = false;
                        var hasAlpha = false;
                        var widthMinusOne = -1;
                        var heightMinusOne = -1;
                        try
                        {
                            isAnimation = reader.GetBit(1);
                            hasAlpha = reader.GetBit(4);

                            // Image size
                            widthMinusOne = reader.GetInt24(4);
                            heightMinusOne = reader.GetInt24(7);
                        }
                        catch (IOException e)
                        {
                            error = "Exception reading WebpRiff chunk 'VP8X' : " + e.Message;
                        }

                        var directory = new WebPDirectory();
                        if (error is null)
                        {
                            directory.Set(WebPDirectory.TagImageWidth, widthMinusOne + 1);
                            directory.Set(WebPDirectory.TagImageHeight, heightMinusOne + 1);
                            directory.Set(WebPDirectory.TagHasAlpha, hasAlpha);
                            directory.Set(WebPDirectory.TagIsAnimation, isAnimation);
                        }
                        else
                            directory.AddError(error);
                        directories.Add(directory);
                        break;
                    }
                case "VP8L":
                    {
                        if (payload.Length < 5)
                            break;

                        var reader = new ByteArrayReader(payload, isMotorolaByteOrder: false);

                        string? error = null;
                        var widthMinusOne = -1;
                        var heightMinusOne = -1;
                        try
                        {
                            if (reader.GetByte(0) != 0x2F)
                                break;
                            var b1 = reader.GetByte(1);
                            var b2 = reader.GetByte(2);
                            var b3 = reader.GetByte(3);
                            var b4 = reader.GetByte(4);

                            widthMinusOne = (b2 & 0x3F) << 8 | b1;
                            heightMinusOne = (b4 & 0x0F) << 10 | b3 << 2 | (b2 & 0xC0) >> 6;
                        }
                        catch (IOException e)
                        {
                            error = "Exception reading WebpRiff chunk 'VP8L' : " + e.Message;
                        }

                        var directory = new WebPDirectory();
                        if (error is null)
                        {
                            directory.Set(WebPDirectory.TagImageWidth, widthMinusOne + 1);
                            directory.Set(WebPDirectory.TagImageHeight, heightMinusOne + 1);
                        }
                        else
                            directory.AddError(error);
                        directories.Add(directory);
                        break;
                    }
                case "VP8 ":
                    {
                        if (payload.Length < 10)
                            break;

                        var reader = new ByteArrayReader(payload, isMotorolaByteOrder: false);

                        string? error = null;
                        var width = 0;
                        var height = 0;
                        try
                        {
                            if (reader.GetByte(3) != 0x9D ||
                                reader.GetByte(4) != 0x01 ||
                                reader.GetByte(5) != 0x2A)
                                break;
                            width = reader.GetUInt16(6);
                            height = reader.GetUInt16(8);
                        }
                        catch (IOException e)
                        {
                            error = "Exception reading WebpRiff chunk 'VP8' : " + e.Message;
                        }

                        var directory = new WebPDirectory();
                        if (error is null)
                        {
                            directory.Set(WebPDirectory.TagImageWidth, width);
                            directory.Set(WebPDirectory.TagImageHeight, height);
                        }
                        else
                            directory.AddError(error);
                        directories.Add(directory);
                        break;
                    }
            }
        }

        public void AddError(string errorMessage)
        {
            directories.Add(new ErrorDirectory(errorMessage));
        }
    }
}
