namespace MetadataExtractor.Util
{
    public static class ByteArrayUtil
    {
        public static bool StartsWith(this byte[] source, byte[] pattern)
        {
            if (source.Length < pattern.Length)
                return false;

            for (var i = 0; i < pattern.Length; i++)
                if (source[i] != pattern[i])
                    return false;

            return true;
        }

        public static bool EqualTo(this byte[] source, byte[] compare)
        {
            if (source.Length != compare.Length)
                return false;

            if (ReferenceEquals(source, compare))
                return true;

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != compare[i])
                    return false;
            }

            return true;
        }
    }
}
