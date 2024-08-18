using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Photoshop
{
    public sealed class PsdReader
    {
        public static async IAsyncEnumerable<MetadataExtractor.Directory> ExtractAsync(SequentialReader reader)
        {
            var psdHeaderDirectory = new PsdHeaderDirectory();

            try
            {
                var signature = await reader.GetInt32Async();
                var version = await reader.GetUInt16Async();

                if (signature != 0x38425053)
                {
                    psdHeaderDirectory.AddError("Invalid PSD file signature");
                }
                else if (version != 1 && version != 2)
                {
                    psdHeaderDirectory.AddError("Invalid PSD file version (must be 1 or 2)");
                }
                else
                {
                    reader.Skip(6);
                    var channelCount = await reader.GetUInt16Async();
                    psdHeaderDirectory.Set(PsdHeaderDirectory.TagChannelCount, channelCount);
                    var imageHeight = await reader.GetInt32Async();
                    psdHeaderDirectory.Set(PsdHeaderDirectory.TagImageHeight, imageHeight);
                    var imageWidth = await reader.GetInt32Async();
                    psdHeaderDirectory.Set(PsdHeaderDirectory.TagImageWidth, imageWidth);
                    var bitsPerChannel = await reader.GetUInt16Async();
                    psdHeaderDirectory.Set(PsdHeaderDirectory.TagBitsPerChannel, bitsPerChannel);
                    var colorMode = await reader.GetUInt16Async();
                    psdHeaderDirectory.Set(PsdHeaderDirectory.TagColorMode, colorMode);
                }
            }
            catch (IOException)
            {
                psdHeaderDirectory.AddError("Unable to read PSD header");
            }

            if (psdHeaderDirectory.HasError)
            {
                yield return psdHeaderDirectory;
                yield break;
            }

            IEnumerable<Directory>? photoshopDirectories = null;

            try
            {
                var colorModeSectionLength = await reader.GetUInt32Async();
                reader.Skip(colorModeSectionLength);

                var imageResourcesSectionLength = await reader.GetUInt32Async();
                if (imageResourcesSectionLength > int.MaxValue)
                    throw new IOException("Invalid resource section length.");
                photoshopDirectories = PhotoshopReader.ExtractAsync(reader, (int)imageResourcesSectionLength).ToBlockingEnumerable();
            }
            catch (IOException)
            {
                psdHeaderDirectory.AddError("Unable to read PSD image resources");
            }

            var directories = new List<Directory> { psdHeaderDirectory };

            if (photoshopDirectories is not null)
                directories.AddRange(photoshopDirectories);

            foreach (var directory in directories)
            {
                yield return directory;
            }
        }
    }
}
