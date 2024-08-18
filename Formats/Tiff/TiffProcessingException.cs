namespace MetadataExtractor.Formats.Tiff
{
    public class TiffProcessingException : ImageProcessingException
    {
        public TiffProcessingException(string message) : base(message)
        {
        }

        public TiffProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TiffProcessingException(Exception innerException) : base(innerException)
        {
        }
    }
}
