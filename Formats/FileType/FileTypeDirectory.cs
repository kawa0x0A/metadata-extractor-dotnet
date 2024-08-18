using MetadataExtractor.Util;

namespace MetadataExtractor.Formats.FileType
{
    public class FileTypeDirectory : Directory
    {
        public const int TagDetectedFileTypeName = 1;
        public const int TagDetectedFileTypeLongName = 2;
        public const int TagDetectedFileMimeType = 3;
        public const int TagExpectedFileNameExtension = 4;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagDetectedFileTypeName, "Detected File Type Name" },
            { TagDetectedFileTypeLongName, "Detected File Type Long Name" },
            { TagDetectedFileMimeType, "Detected MIME Type" },
            { TagExpectedFileNameExtension, "Expected File Name Extension" },
        };

        public FileTypeDirectory(Util.FileType fileType) : base(_tagNameMap)
        {
            SetDescriptor(new FileTypeDescriptor(this));

            var name = fileType.GetName();

            Set(TagDetectedFileTypeName, name);
            Set(TagDetectedFileTypeLongName, fileType.GetLongName());

            var mimeType = fileType.GetMimeType();
            if (mimeType is not null)
                Set(TagDetectedFileMimeType, mimeType);

            var extension = fileType.GetCommonExtension();
            if (extension is not null)
                Set(TagExpectedFileNameExtension, extension);
        }

        public override string Name => "File Type";
    }
}
