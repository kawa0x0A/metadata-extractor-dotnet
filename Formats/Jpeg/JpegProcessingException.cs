namespace MetadataExtractor.Formats.Jpeg
{
    public class JpegProcessingException : ImageProcessingException
    {
        public JpegProcessingException(string message) : base(message)
        {
        }

        public JpegProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public JpegProcessingException(Exception innerException) : base(innerException)
        {
        }
    }
}
