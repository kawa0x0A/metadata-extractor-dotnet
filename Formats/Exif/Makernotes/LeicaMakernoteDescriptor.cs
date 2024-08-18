namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class LeicaMakernoteDescriptor(LeicaMakernoteDirectory directory) : TagDescriptor<LeicaMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                LeicaMakernoteDirectory.TagQuality => GetQualityDescription(),
                LeicaMakernoteDirectory.TagUserProfile => GetUserProfileDescription(),
                LeicaMakernoteDirectory.TagWhiteBalance => GetWhiteBalanceDescription(),
                LeicaMakernoteDirectory.TagExternalSensorBrightnessValue => GetExternalSensorBrightnessValueDescription(),
                LeicaMakernoteDirectory.TagMeasuredLV => GetMeasuredLVDescription(),
                LeicaMakernoteDirectory.TagApproximateFNumber => GetApproximateFNumberDescription(),
                LeicaMakernoteDirectory.TagCameraTemperature => GetCameraTemperatureDescription(),
                LeicaMakernoteDirectory.TagWBRedLevel or LeicaMakernoteDirectory.TagWBBlueLevel or LeicaMakernoteDirectory.TagWBGreenLevel => GetSimpleRational(tagType),
                _ => base.GetDescription(tagType),
            };
        }

        private string? GetCameraTemperatureDescription()
        {
            return GetFormattedInt(LeicaMakernoteDirectory.TagCameraTemperature, "{0} C");
        }

        private string? GetApproximateFNumberDescription()
        {
            return GetSimpleRational(LeicaMakernoteDirectory.TagApproximateFNumber);
        }

        private string? GetMeasuredLVDescription()
        {
            return GetSimpleRational(LeicaMakernoteDirectory.TagMeasuredLV);
        }

        private string? GetExternalSensorBrightnessValueDescription()
        {
            return GetSimpleRational(LeicaMakernoteDirectory.TagExternalSensorBrightnessValue);
        }

        private string? GetWhiteBalanceDescription()
        {
            return GetIndexedDescription(LeicaMakernoteDirectory.TagWhiteBalance,
                "Auto or Manual", "Daylight", "Fluorescent", "Tungsten", "Flash", "Cloudy", "Shadow");
        }

        private string? GetUserProfileDescription()
        {
            return GetIndexedDescription(LeicaMakernoteDirectory.TagQuality,
                1,
                "User Profile 1", "User Profile 2", "User Profile 3", "User Profile 0 (Dynamic)");
        }

        private string? GetQualityDescription()
        {
            return GetIndexedDescription(LeicaMakernoteDirectory.TagQuality,
                1,
                "Fine", "Basic");
        }
    }
}
