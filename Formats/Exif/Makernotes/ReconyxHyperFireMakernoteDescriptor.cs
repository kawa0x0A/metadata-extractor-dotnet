namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class ReconyxHyperFireMakernoteDescriptor(ReconyxHyperFireMakernoteDirectory directory) : TagDescriptor<ReconyxHyperFireMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            switch (tagType)
            {
                case ReconyxHyperFireMakernoteDirectory.TagMakernoteVersion:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFireMakernoteDirectory.TagFirmwareVersion:
                    return Directory.GetString(tagType);
                case ReconyxHyperFireMakernoteDirectory.TagTriggerMode:
                    return Directory.GetString(tagType);
                case ReconyxHyperFireMakernoteDirectory.TagSequence:
                    var sequence = Directory.GetArray<int[]>(tagType);
                    return sequence is not null ? $"{sequence[0]}/{sequence[1]}" : base.GetDescription(tagType);
                case ReconyxHyperFireMakernoteDirectory.TagEventNumber:
                    return Directory.GetUint(tagType).ToString();
                case ReconyxHyperFireMakernoteDirectory.TagMotionSensitivity:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFireMakernoteDirectory.TagBatteryVoltage:
                    return Directory.GetDouble(tagType).ToString("0.000");
                case ReconyxHyperFireMakernoteDirectory.TagDateTimeOriginal:
                    return Directory.GetDateTime(tagType).ToString("yyyy:MM:dd HH:mm:ss");
                case ReconyxHyperFireMakernoteDirectory.TagMoonPhase:
                    return GetIndexedDescription(tagType, "New", "Waxing Crescent", "First Quarter", "Waxing Gibbous", "Full", "Waning Gibbous", "Last Quarter", "Waning Crescent");
                case ReconyxHyperFireMakernoteDirectory.TagAmbientTemperatureFahrenheit:
                case ReconyxHyperFireMakernoteDirectory.TagAmbientTemperature:
                    return Directory.GetShort(tagType).ToString();
                case ReconyxHyperFireMakernoteDirectory.TagSerialNumber:
                    return Directory.GetString(tagType);
                case ReconyxHyperFireMakernoteDirectory.TagContrast:
                case ReconyxHyperFireMakernoteDirectory.TagBrightness:
                case ReconyxHyperFireMakernoteDirectory.TagSharpness:
                case ReconyxHyperFireMakernoteDirectory.TagSaturation:
                    return Directory.GetUshort(tagType).ToString();
                case ReconyxHyperFireMakernoteDirectory.TagInfraredIlluminator:
                    return GetIndexedDescription(tagType, "Off", "On");
                case ReconyxHyperFireMakernoteDirectory.TagUserLabel:
                    return Directory.GetString(tagType);
                default:
                    return base.GetDescription(tagType);
            }
        }
    }
}
