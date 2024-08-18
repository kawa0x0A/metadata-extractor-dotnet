namespace MetadataExtractor
{
    public class MetadataException : Exception
    {
        public MetadataException(string msg) : base(msg)
        {
        }

        public MetadataException(Exception innerException) : base(null, innerException)
        {
        }

        public MetadataException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}
