using System.Globalization;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class ReconyxHyperFire2MakernoteDescriptor(ReconyxHyperFire2MakernoteDirectory directory) : TagDescriptor<ReconyxHyperFire2MakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            switch (tagType)
            {
                case ReconyxHyperFire2MakernoteDirectory.TagFileNumber:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagDirectoryNumber:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagFirmwareVersion:
                    var version = Directory.GetVersion(tagType);
                    if (version is not null)
                        return $"{version.Major}.{version.Minor}{(char)version.Build}";
                    else
                        return string.Empty;
                case ReconyxHyperFire2MakernoteDirectory.TagFirmwareDate:
                    return Directory.GetDateTime(tagType).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                case ReconyxHyperFire2MakernoteDirectory.TagTriggerMode:
                    return Directory.GetString(tagType) switch
                    {
                        "M" => "Motion Detection",
                        "P" => "Point and Shoot",
                        "T" => "Time Lapse",
                        _ => "Unknown trigger mode",
                    };
                case ReconyxHyperFire2MakernoteDirectory.TagSequence:
                    var sequence = Directory.GetArray<int[]>(tagType);
                    return sequence is { Length: > 1 } ? $"{sequence[0]}/{sequence[1]}" : base.GetDescription(tagType);
                case ReconyxHyperFire2MakernoteDirectory.TagEventNumber:
                    return Directory.GetUint(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagDateTimeOriginal:
                    return Directory.GetDateTime(tagType).ToString("yyyy:MM:dd HH:mm:ss");
                case ReconyxHyperFire2MakernoteDirectory.TagDayOfWeek:
                    return GetIndexedDescription(tagType, CultureInfo.CurrentCulture.DateTimeFormat.DayNames);
                case ReconyxHyperFire2MakernoteDirectory.TagMoonPhase:
                    return GetIndexedDescription(tagType, "New", "Waxing Crescent", "First Quarter", "Waxing Gibbous", "Full", "Waning Gibbous", "Last Quarter", "Waning Crescent");
                case ReconyxHyperFire2MakernoteDirectory.TagAmbientTemperatureFahrenheit:
                    return $"{Directory.GetShort(tagType)}°F";
                case ReconyxHyperFire2MakernoteDirectory.TagAmbientTemperatureCelcius:
                    return $"{Directory.GetShort(tagType)}°C";
                case ReconyxHyperFire2MakernoteDirectory.TagContrast:
                case ReconyxHyperFire2MakernoteDirectory.TagBrightness:
                case ReconyxHyperFire2MakernoteDirectory.TagSharpness:
                case ReconyxHyperFire2MakernoteDirectory.TagSaturation:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagFlash:
                    return GetIndexedDescription(tagType, "Off", "On");
                case ReconyxHyperFire2MakernoteDirectory.TagAmbientInfrared:
                case ReconyxHyperFire2MakernoteDirectory.TagAmbientLight:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagMotionSensitivity:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagBatteryVoltage:
                case ReconyxHyperFire2MakernoteDirectory.TagBatteryVoltageAvg:
                    return Directory.GetDouble(tagType).ToString("0.000");
                case ReconyxHyperFire2MakernoteDirectory.TagBatteryType:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFire2MakernoteDirectory.TagUserLabel:
                case ReconyxHyperFire2MakernoteDirectory.TagSerialNumber:
                    return Directory.GetString(tagType);
                default:
                    return base.GetDescription(tagType);
            }
        }
    }
}
