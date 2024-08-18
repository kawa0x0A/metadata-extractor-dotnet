namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class ReconyxHyperFireMakernoteDirectory : Directory
    {
        public static readonly ushort MakernoteVersion = 61697;

        public const int TagMakernoteVersion = 0;
        public const int TagFirmwareVersion = 2;
        public const int TagTriggerMode = 12;
        public const int TagSequence = 14;
        public const int TagEventNumber = 18;
        public const int TagDateTimeOriginal = 22;
        public const int TagMoonPhase = 36;
        public const int TagAmbientTemperatureFahrenheit = 38;
        public const int TagAmbientTemperature = 40;
        public const int TagSerialNumber = 42;
        public const int TagContrast = 72;
        public const int TagBrightness = 74;
        public const int TagSharpness = 76;
        public const int TagSaturation = 78;
        public const int TagInfraredIlluminator = 80;
        public const int TagMotionSensitivity = 82;
        public const int TagBatteryVoltage = 84;
        public const int TagUserLabel = 86;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
             { TagMakernoteVersion, "Makernote Version" },
             { TagFirmwareVersion, "Firmware Version" },
             { TagTriggerMode, "Trigger Mode" },
             { TagSequence, "Sequence" },
             { TagEventNumber, "Event Number" },
             { TagDateTimeOriginal, "Date/Time Original" },
             { TagMoonPhase, "Moon Phase" },
             { TagAmbientTemperatureFahrenheit, "Ambient Temperature Fahrenheit" },
             { TagAmbientTemperature, "Ambient Temperature" },
             { TagSerialNumber, "Serial Number" },
             { TagContrast, "Contrast" },
             { TagBrightness, "Brightness" },
             { TagSharpness, "Sharpness" },
             { TagSaturation, "Saturation" },
             { TagInfraredIlluminator, "Infrared Illuminator" },
             { TagMotionSensitivity, "Motion Sensitivity" },
             { TagBatteryVoltage, "Battery Voltage" },
             { TagUserLabel, "User Label" }
        };

        public ReconyxHyperFireMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new ReconyxHyperFireMakernoteDescriptor(this));
        }

        public override string Name => "Reconyx HyperFire Makernote";
    }
}
