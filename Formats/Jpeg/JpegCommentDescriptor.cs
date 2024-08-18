namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class JpegCommentDescriptor(JpegCommentDirectory directory) : TagDescriptor<JpegCommentDirectory>(directory)
    {
        public string? GetJpegCommentDescription()
        {
            return Directory.GetString(JpegCommentDirectory.TagComment);
        }
    }
}
