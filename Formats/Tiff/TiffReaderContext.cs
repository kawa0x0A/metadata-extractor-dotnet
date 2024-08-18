using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Tiff
{
    public readonly struct TiffReaderContext
    {
        public IndexedReader Reader { get; }

        public bool IsMotorolaByteOrder { get; }

        public bool IsBigTiff { get; }

        private readonly HashSet<IfdIdentity> _visitedIfds;

        public TiffReaderContext(IndexedReader reader, bool isMotorolaByteOrder, bool isBigTiff) : this()
        {
            Reader = reader;
            IsMotorolaByteOrder = isMotorolaByteOrder;
            IsBigTiff = isBigTiff;

            _visitedIfds = new();
        }

        public bool TryVisitIfd(int ifdOffset, object kind)
        {
            var globalIfdOffset = Reader.ToUnshiftedOffset(ifdOffset);

            return _visitedIfds.Add(new(globalIfdOffset, kind));
        }

        public TiffReaderContext WithByteOrder(bool isMotorolaByteOrder)
        {
            return new(Reader.WithByteOrder(isMotorolaByteOrder), IsMotorolaByteOrder, IsBigTiff);
        }

        public TiffReaderContext WithShiftedBaseOffset(int baseOffset)
        {
            return new(Reader.WithShiftedBaseOffset(baseOffset), IsMotorolaByteOrder, IsBigTiff);
        }
    }
}
