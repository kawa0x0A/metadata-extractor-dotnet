namespace MetadataExtractor.Util
{
    internal interface ITypeChecker
    {
        int ByteCount { get; }
        FileType CheckType(byte[] bytes);
    }
}
