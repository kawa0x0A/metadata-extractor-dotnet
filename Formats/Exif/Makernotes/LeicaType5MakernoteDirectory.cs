namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class LeicaType5MakernoteDirectory : Directory
    {
        public const int TagLensModel = 0x0303;
        public const int TagOriginalFileName = 0x0407;
        public const int TagOriginalDirectory = 0x0408;
        public const int TagExposureMode = 0x040d;
        public const int TagShotInfo = 0x0410;
        public const int TagFilmMode = 0x0412;
        public const int TagWbRgbLevels = 0x0413;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagLensModel, "Lens Model" },
            { TagOriginalFileName, "Original File Name" },
            { TagOriginalDirectory, "Original Directory" },
            { TagExposureMode, "Exposure Mode" },
            { TagShotInfo, "Shot Info" },
            { TagFilmMode, "Film Mode" },
            { TagWbRgbLevels, "WB RGB Levels" }
        };

        public LeicaType5MakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new LeicaType5MakernoteDescriptor(this));
        }

        public override string Name => "Leica Makernote";
    }
}
