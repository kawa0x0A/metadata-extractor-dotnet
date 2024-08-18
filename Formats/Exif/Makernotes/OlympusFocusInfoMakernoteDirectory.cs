namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusFocusInfoMakernoteDirectory : Directory
    {
        public const int TagFocusInfoVersion = 0x0000;
        public const int TagAutoFocus = 0x0209;
        public const int TagSceneDetect = 0x0210;
        public const int TagSceneArea = 0x0211;
        public const int TagSceneDetectData = 0x0212;

        public const int TagZoomStepCount = 0x0300;
        public const int TagFocusStepCount = 0x0301;
        public const int TagFocusStepInfinity = 0x0303;
        public const int TagFocusStepNear = 0x0304;
        public const int TagFocusDistance = 0x0305;
        public const int TagAfPoint = 0x0308;

        public const int TagAfInfo = 0x0328;

        public const int TagExternalFlash = 0x1201;
        public const int TagExternalFlashGuideNumber = 0x1203;
        public const int TagExternalFlashBounce = 0x1204;
        public const int TagExternalFlashZoom = 0x1205;
        public const int TagInternalFlash = 0x1208;
        public const int TagManualFlash = 0x1209;
        public const int TagMacroLed = 0x120A;

        public const int TagSensorTemperature = 0x1500;

        public const int TagImageStabilization = 0x1600;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagFocusInfoVersion, "Focus Info Version" },
            { TagAutoFocus, "Auto Focus" },
            { TagSceneDetect, "Scene Detect" },
            { TagSceneArea, "Scene Area" },
            { TagSceneDetectData, "Scene Detect Data" },
            { TagZoomStepCount, "Zoom Step Count" },
            { TagFocusStepCount, "Focus Step Count" },
            { TagFocusStepInfinity, "Focus Step Infinity" },
            { TagFocusStepNear, "Focus Step Near" },
            { TagFocusDistance, "Focus Distance" },
            { TagAfPoint, "AF Point" },
            { TagAfInfo, "AF Info" },
            { TagExternalFlash, "External Flash" },
            { TagExternalFlashGuideNumber, "External Flash Guide Number" },
            { TagExternalFlashBounce, "External Flash Bounce" },
            { TagExternalFlashZoom, "External Flash Zoom" },
            { TagInternalFlash, "Internal Flash" },
            { TagManualFlash, "Manual Flash" },
            { TagMacroLed, "Macro LED" },
            { TagSensorTemperature, "Sensor Temperature" },
            { TagImageStabilization, "Image Stabilization" }
        };

        public OlympusFocusInfoMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new OlympusFocusInfoMakernoteDescriptor(this));
        }

        public override string Name => "Olympus Focus Info";
    }
}
