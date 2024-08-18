namespace MetadataExtractor.Formats.Riff
{
    public class RiffProcessingException : ImageProcessingException
    {
        public RiffProcessingException(string message) : base(message)
        {
        }

        public RiffProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RiffProcessingException(Exception innerException) : base(innerException)
        {
        }
    }
}
