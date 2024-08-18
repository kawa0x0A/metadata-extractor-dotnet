namespace MetadataExtractor.Util
{
    internal static class ByteArrayExtensions
    {
        public static bool RegionEquals(this byte[] bytes, int offset, int count, byte[] comparand)
        {
            if (offset < 0 ||
                count < 0 ||
                bytes.Length < offset + count)
                return false;

            for (int i = 0, j = offset; i < count; i++, j++)
            {
                if (bytes[j] != comparand[i])
                    return false;
            }

            return true;
        }
    }
}
