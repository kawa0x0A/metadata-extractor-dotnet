namespace MetadataExtractor.Formats.Png
{
    public class PngProcessingException : ImageProcessingException
    {
        public PngProcessingException(string message) : base(message)
        {
        }

        public PngProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PngProcessingException(Exception innerException) : base(innerException)
        {
        }
    }
}
