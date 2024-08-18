namespace MetadataExtractor.Util
{
    public enum FileType
    {
        Unknown,

        Jpeg,

        Png,

        WebP,
    }

    public static class FileTypeExtensions
    {
        private static readonly string[] _shortNames =
        [
            "Unknown",
            "JPEG",
            "PNG",
            "WebP",
        ];

        private static readonly string[] _longNames =
        [
            "Unknown",
            "Joint Photographic Experts Group",
            "Portable Network Graphics",
            "WebP",
        ];

        private static readonly string?[] _mimeTypes =
        [
            null,
            "image/jpeg",
            "image/png",
            "image/webp",
        ];

        private static readonly string[]?[] _extensions =
        [
            null,
            ["jpg", "jpeg", "jpe"],
            ["png"],
            ["webp"],
        ];

        public static string GetName(this FileType fileType)
        {
            var i = (int)fileType;
            if (i < 0 || i >= _shortNames.Length)
                throw new ArgumentException($"Invalid {nameof(FileType)} enum member.", nameof(fileType));
            return _shortNames[i];
        }

        public static string GetLongName(this FileType fileType)
        {
            var i = (int)fileType;
            if (i < 0 || i >= _longNames.Length)
                throw new ArgumentException($"Invalid {nameof(FileType)} enum member.", nameof(fileType));
            return _longNames[i];
        }

        public static string? GetMimeType(this FileType fileType)
        {
            var i = (int)fileType;
            if (i < 0 || i >= _mimeTypes.Length)
                throw new ArgumentException($"Invalid {nameof(FileType)} enum member.", nameof(fileType));
            return _mimeTypes[i];
        }

        public static string? GetCommonExtension(this FileType fileType)
        {
            var i = (int)fileType;
            if (i < 0 || i >= _extensions.Length)
                throw new ArgumentException($"Invalid {nameof(FileType)} enum member.", nameof(fileType));
            return _extensions[i]?.FirstOrDefault();
        }

        public static IEnumerable<string?>? GetAllExtensions(this FileType fileType)
        {
            var i = (int)fileType;
            if (i < 0 || i >= _extensions.Length)
                throw new ArgumentException($"Invalid {nameof(FileType)} enum member.", nameof(fileType));
            return _extensions[i];
        }
    }
}
