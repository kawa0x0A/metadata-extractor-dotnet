using System.Text;

namespace MetadataExtractor.Formats.Png
{
    public sealed class PngChunkType
    {
        private static readonly ICollection<string> _identifiersAllowingMultiples = ["IDAT", "sPLT", "iTXt", "tEXt", "zTXt"];

        public static readonly PngChunkType IHDR = new("IHDR");

        public static readonly PngChunkType PLTE = new("PLTE");

        public static readonly PngChunkType IDAT = new("IDAT", true);

        public static readonly PngChunkType IEND = new("IEND");

        public static readonly PngChunkType cHRM = new("cHRM");

        public static readonly PngChunkType gAMA = new("gAMA");

        public static readonly PngChunkType iCCP = new("iCCP");

        public static readonly PngChunkType sBIT = new("sBIT");

        public static readonly PngChunkType sRGB = new("sRGB");

        public static readonly PngChunkType bKGD = new("bKGD");

        public static readonly PngChunkType hIST = new("hIST");

        public static readonly PngChunkType tRNS = new("tRNS");

        public static readonly PngChunkType pHYs = new("pHYs");

        public static readonly PngChunkType sPLT = new("sPLT", true);

        public static readonly PngChunkType tIME = new("tIME");

        public static readonly PngChunkType iTXt = new("iTXt", true);

        public static readonly PngChunkType tEXt = new("tEXt", true);

        public static readonly PngChunkType zTXt = new("zTXt", true);

        public static readonly PngChunkType eXIf = new("eXIf");

        private readonly byte[] _bytes;

        public PngChunkType(string identifier, bool multipleAllowed = false)
        {
            AreMultipleAllowed = multipleAllowed;
            var bytes = Encoding.UTF8.GetBytes(identifier);
            ValidateBytes(bytes);
            _bytes = bytes;
        }

        public PngChunkType(byte[] bytes)
        {
            ValidateBytes(bytes);
            _bytes = bytes;
            AreMultipleAllowed = _identifiersAllowingMultiples.Contains(Identifier);
        }

        private static void ValidateBytes(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new ArgumentException("PNG chunk type identifier must be four bytes in length");
            if (!bytes.All(IsValidByte))
                throw new ArgumentException("PNG chunk type identifier may only contain alphabet characters");
        }

        public bool IsCritical => IsUpperCase(_bytes[0]);

        public bool IsAncillary => !IsCritical;

        public bool IsPrivate => IsUpperCase(_bytes[1]);

        public bool IsSafeToCopy => IsLowerCase(_bytes[3]);

        public bool AreMultipleAllowed { get; }

        private static bool IsLowerCase(byte b) => (b & (1 << 5)) != 0;

        private static bool IsUpperCase(byte b) => (b & (1 << 5)) == 0;

        private static bool IsValidByte(byte b) => b is >= 65 and <= 90 or >= 97 and <= 122;

        public string Identifier => Encoding.UTF8.GetString(_bytes, 0, _bytes.Length);

        public override string ToString() => Identifier;

        private bool Equals(PngChunkType other) => _bytes.SequenceEqual(other._bytes);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is PngChunkType t && Equals(t);
        }

        public override int GetHashCode() => _bytes[0] << 24 | _bytes[1] << 16 << _bytes[2] << 8 | _bytes[3];

        public static bool operator ==(PngChunkType left, PngChunkType right) => Equals(left, right);
        public static bool operator !=(PngChunkType left, PngChunkType right) => !Equals(left, right);
    }
}
