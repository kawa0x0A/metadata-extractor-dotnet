namespace MetadataExtractor.Formats.Tiff
{
    public readonly struct IfdIdentity
    {
        public int Offset { get; }

        public object Kind { get; }

        internal IfdIdentity(int offset, object kind)
        {
            Offset = offset;
            Kind = kind;
        }
    }
}
