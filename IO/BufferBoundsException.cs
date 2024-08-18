namespace MetadataExtractor.IO
{
    [Serializable]
    public class BufferBoundsException : IOException
    {
        public BufferBoundsException(int index, int bytesRequested, long bufferLength) : base(GetMessage(index, bytesRequested, bufferLength))
        {
        }

        public BufferBoundsException(string message) : base(message)
        {
        }

        public BufferBoundsException()
        {
        }

        private static string GetMessage(int index, int bytesRequested, long bufferLength)
        {
            if (index < 0)
                return $"Attempt to read from buffer using a negative index ({index})";

            if (bytesRequested < 0)
                return $"Number of requested bytes cannot be negative ({bytesRequested})";

            if (index + (long)bytesRequested - 1L > int.MaxValue)
                return $"Number of requested bytes summed with starting index exceed maximum range of signed 32 bit integers (requested index: {index}, requested count: {bytesRequested})";

            return $"Attempt to read from beyond end of underlying data source (requested index: {index}, requested count: {bytesRequested}, max index: {bufferLength - 1})";
        }
    }
}
