namespace MetadataExtractor
{
    public class ImageProcessingException : Exception
    {
        public ImageProcessingException(string message) : base(message)
        {
        }

        public ImageProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ImageProcessingException(Exception innerException) : base(null, innerException)
        {
        }
    }
}
