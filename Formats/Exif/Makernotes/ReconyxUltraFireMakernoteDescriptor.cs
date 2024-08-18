using System.Globalization;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class ReconyxUltraFireMakernoteDescriptor(ReconyxUltraFireMakernoteDirectory directory) : TagDescriptor<ReconyxUltraFireMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            switch (tagType)
            {
                case ReconyxUltraFireMakernoteDirectory.TagLabel:
                    return Directory.GetString(tagType);
                case ReconyxUltraFireMakernoteDirectory.TagMakernoteId:
                    return "0x" + Directory.GetUint(tagType).ToString("x8");
                case ReconyxUltraFireMakernoteDirectory.TagMakernoteSize:
                    return Directory.GetUint(tagType).ToString();
                case ReconyxUltraFireMakernoteDirectory.TagMakernotePublicId:
                    return "0x" + Directory.GetUint(tagType).ToString("x8");
                case ReconyxUltraFireMakernoteDirectory.TagMakernotePublicSize:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxUltraFireMakernoteDirectory.TagAmbientTemperatureFahrenheit:
                case ReconyxUltraFireMakernoteDirectory.TagAmbientTemperature:
                    return Directory.GetShort(tagType).ToString();
                case ReconyxUltraFireMakernoteDirectory.TagCameraVersion:
                case ReconyxUltraFireMakernoteDirectory.TagUibVersion:
                case ReconyxUltraFireMakernoteDirectory.TagBtlVersion:
                case ReconyxUltraFireMakernoteDirectory.TagPexVersion:
                case ReconyxUltraFireMakernoteDirectory.TagEventType:
                    return Directory.GetString(tagType);
                case ReconyxUltraFireMakernoteDirectory.TagSequence:
                    var sequence = Directory.GetArray<int[]>(tagType);
                    return sequence is not null ? $"{sequence[0]}/{sequence[1]}" : base.GetDescription(tagType);
                case ReconyxUltraFireMakernoteDirectory.TagEventNumber:
                    return Directory.GetUint(tagType).ToString();
                case ReconyxUltraFireMakernoteDirectory.TagDateTimeOriginal:
                    return Directory.GetDateTime(tagType).ToString("yyyy:MM:dd HH:mm:ss");
                case ReconyxUltraFireMakernoteDirectory.TagDayOfWeek:
                    return GetIndexedDescription(tagType, CultureInfo.CurrentCulture.DateTimeFormat.DayNames);
                case ReconyxUltraFireMakernoteDirectory.TagMoonPhase:
                    return GetIndexedDescription(tagType, "New", "Waxing Crescent", "First Quarter", "Waxing Gibbous", "Full", "Waning Gibbous", "Last Quarter", "Waning Crescent");
                case ReconyxUltraFireMakernoteDirectory.TagFlash:
                    return GetIndexedDescription(tagType, "Off", "On");
                case ReconyxUltraFireMakernoteDirectory.TagSerialNumber:
                    return Directory.GetString(tagType);
                case ReconyxUltraFireMakernoteDirectory.TagBatteryVoltage:
                    return Directory.GetDouble(tagType).ToString("0.000");
                case ReconyxUltraFireMakernoteDirectory.TagUserLabel:
                    return Directory.GetString(tagType);
                default:
                    return base.GetDescription(tagType);
            }
        }
    }
}
