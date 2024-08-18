using MetadataExtractor.Util;
using System.Text;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusMakernoteDescriptor(OlympusMakernoteDirectory directory) : TagDescriptor<OlympusMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                OlympusMakernoteDirectory.TagMakernoteVersion => GetMakernoteVersionDescription(),
                OlympusMakernoteDirectory.TagColourMode => GetColorModeDescription(),
                OlympusMakernoteDirectory.TagImageQuality1 => GetImageQuality1Description(),
                OlympusMakernoteDirectory.TagImageQuality2 => GetImageQuality2Description(),
                OlympusMakernoteDirectory.TagSpecialMode => GetSpecialModeDescription(),
                OlympusMakernoteDirectory.TagJpegQuality => GetJpegQualityDescription(),
                OlympusMakernoteDirectory.TagMacroMode => GetMacroModeDescription(),
                OlympusMakernoteDirectory.TagBwMode => GetBwModeDescription(),
                OlympusMakernoteDirectory.TagDigitalZoom => GetDigitalZoomDescription(),
                OlympusMakernoteDirectory.TagFocalPlaneDiagonal => GetFocalPlaneDiagonalDescription(),
                OlympusMakernoteDirectory.TagCameraType => GetCameraTypeDescription(),
                OlympusMakernoteDirectory.TagCameraId => GetCameraIdDescription(),
                OlympusMakernoteDirectory.TagOneTouchWb => GetOneTouchWbDescription(),
                OlympusMakernoteDirectory.TagShutterSpeedValue => GetShutterSpeedDescription(),
                OlympusMakernoteDirectory.TagIsoValue => GetIsoValueDescription(),
                OlympusMakernoteDirectory.TagApertureValue => GetApertureValueDescription(),
                OlympusMakernoteDirectory.TagFlashMode => GetFlashModeDescription(),
                OlympusMakernoteDirectory.TagFocusRange => GetFocusRangeDescription(),
                OlympusMakernoteDirectory.TagFocusMode => GetFocusModeDescription(),
                OlympusMakernoteDirectory.TagSharpness => GetSharpnessDescription(),
                OlympusMakernoteDirectory.TagColourMatrix => GetColorMatrixDescription(),
                OlympusMakernoteDirectory.TagWbMode => GetWbModeDescription(),
                OlympusMakernoteDirectory.TagRedBalance => GetRedBalanceDescription(),
                OlympusMakernoteDirectory.TagBlueBalance => GetBlueBalanceDescription(),
                OlympusMakernoteDirectory.TagContrast => GetContrastDescription(),
                OlympusMakernoteDirectory.TagPreviewImageValid => GetPreviewImageValidDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagExposureMode => GetExposureModeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFlashMode => GetFlashModeCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagWhiteBalance => GetWhiteBalanceDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagImageSize => GetImageSizeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagImageQuality => GetImageQualityDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagShootingMode => GetShootingModeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagMeteringMode => GetMeteringModeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagApexFilmSpeedValue => GetApexFilmSpeedDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagApexShutterSpeedTimeValue => GetApexShutterSpeedTimeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagApexApertureValue => GetApexApertureDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagMacroMode => GetMacroModeCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagDigitalZoom => GetDigitalZoomCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagExposureCompensation => GetExposureCompensationDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagBracketStep => GetBracketStepDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagIntervalLength => GetIntervalLengthDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagIntervalNumber => GetIntervalNumberDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFocalLength => GetFocalLengthDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFocusDistance => GetFocusDistanceDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFlashFired => GetFlashFiredDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagDate => GetDateDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagTime => GetTimeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagMaxApertureAtFocalLength => GetMaxApertureAtFocalLengthDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFileNumberMemory => GetFileNumberMemoryDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagLastFileNumber => GetLastFileNumberDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceRed => GetWhiteBalanceRedDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceGreen => GetWhiteBalanceGreenDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceBlue => GetWhiteBalanceBlueDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagSaturation => GetSaturationDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagContrast => GetContrastCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagSharpness => GetSharpnessCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagSubjectProgram => GetSubjectProgramDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFlashCompensation => GetFlashCompensationDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagIsoSetting => GetIsoSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagCameraModel => GetCameraModelDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagIntervalMode => GetIntervalModeDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFolderName => GetFolderNameDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagColorMode => GetColorModeCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagColorFilter => GetColorFilterDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagBlackAndWhiteFilter => GetBlackAndWhiteFilterDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagInternalFlash => GetInternalFlashDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagApexBrightnessValue => GetApexBrightnessDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagSpotFocusPointXCoordinate => GetSpotFocusPointXCoordinateDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagSpotFocusPointYCoordinate => GetSpotFocusPointYCoordinateDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagWideFocusZone => GetWideFocusZoneDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFocusMode => GetFocusModeCameraSettingDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagFocusArea => GetFocusAreaDescription(),
                OlympusMakernoteDirectory.CameraSettings.TagDecSwitchPosition => GetDecSwitchPositionDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetExposureModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagExposureMode,
                "P", "A", "S", "M");
        }

        public string? GetFlashModeCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFlashMode,
                "Normal", "Red-eye reduction", "Rear flash sync", "Wireless");
        }

        public string? GetWhiteBalanceDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagWhiteBalance,
                "Auto", "Daylight", "Cloudy", "Tungsten", null, "Custom", null, "Fluorescent", "Fluorescent 2", null, null, "Custom 2", "Custom 3");
        }

        public string? GetImageSizeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagImageSize,
                "2560 x 1920", "1600 x 1200", "1280 x 960", "640 x 480");
        }

        public string? GetImageQualityDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagImageQuality,
                "Raw", "Super Fine", "Fine", "Standard", "Economy", "Extra Fine");
        }

        public string? GetShootingModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagShootingMode,
                "Single", "Continuous", "Self Timer", null, "Bracketing", "Interval", "UHS Continuous", "HS Continuous");
        }

        public string? GetMeteringModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagMeteringMode,
                "Multi-Segment", "Centre Weighted", "Spot");
        }

        public string? GetApexFilmSpeedDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagApexFilmSpeedValue, out long value))
                return null;
            var iso = Math.Pow(value / 8d - 1, 2) * 3.125;
            return iso.ToString("0.##");
        }

        public string? GetApexShutterSpeedTimeDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagApexShutterSpeedTimeValue, out long value))
                return null;
            var shutterSpeed = Math.Pow((49 - value) / 8d, 2);
            return $"{shutterSpeed:0.###} sec";
        }

        public string? GetApexApertureDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagApexApertureValue, out long value))
                return null;
            var fStop = Math.Pow(value / 16d - 0.5, 2);
            return GetFStopDescription(fStop);
        }

        public string? GetMacroModeCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagMacroMode,
                "Off", "On");
        }

        public string? GetDigitalZoomCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagDigitalZoom,
                "Off", "Electronic magnification", "Digital zoom 2x");
        }

        public string? GetExposureCompensationDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagExposureCompensation, out long value)
                ? $"{value / 3d - 2:0.##} EV"
                : null;
        }

        public string? GetBracketStepDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagBracketStep,
                "1/3 EV", "2/3 EV", "1 EV");
        }

        public string? GetIntervalLengthDescription()
        {
            if (!Directory.IsIntervalMode())
                return "N/A";

            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagIntervalLength, out long value)
                ? value + " min"
                : null;
        }

        public string? GetIntervalNumberDescription()
        {
            if (!Directory.IsIntervalMode())
                return "N/A";

            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagIntervalNumber, out long value)
                ? value.ToString()
                : null;
        }

        public string? GetFocalLengthDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagFocalLength, out long value)
                ? GetFocalLengthDescription(value / 256d)
                : null;
        }

        public string? GetFocusDistanceDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagFocusDistance, out long value)
                ? value == 0 ? "Infinity" : value + " mm"
                : null;
        }

        public string? GetFlashFiredDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFlashFired,
                "No", "Yes");
        }

        public string? GetDateDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagDate, out long value))
                return null;

            var day = (int)(value & 0xFF);
            var month = (int)(value >> 16) & 0xFF;
            var year = ((int)(value >> 8) & 0xFF) + 1970;

            if (!DateUtil.IsValidDate(year, month, day))
                return "Invalid date";

            return $"{year:0000}-{month + 1:00}-{day:00}";
        }

        public string? GetTimeDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagTime, out long value))
                return null;

            var hours = (int)((value >> 8) & 0xFF);
            var minutes = (int)((value >> 16) & 0xFF);
            var seconds = (int)(value & 0xFF);

            if (!DateUtil.IsValidTime(hours, minutes, seconds))
                return "Invalid date";

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        public string? GetMaxApertureAtFocalLengthDescription()
        {
            if (!Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagTime, out long value))
                return null;
            var fStop = Math.Pow(value / 16d - 0.5, 2);
            return GetFStopDescription(fStop);
        }

        public string? GetFileNumberMemoryDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFileNumberMemory,
                "Off", "On");
        }

        public string? GetLastFileNumberDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagLastFileNumber, out long value)
                ? value == 0
                    ? "File Number Memory Off"
                    : value.ToString()
                : null;
        }

        public string? GetWhiteBalanceRedDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceRed, out long value)
                ? (value / 256d).ToString("0.##")
                : null;
        }

        public string? GetWhiteBalanceGreenDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceGreen, out long value)
                ? (value / 256d).ToString("0.##")
                : null;
        }

        public string? GetWhiteBalanceBlueDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagWhiteBalanceBlue, out long value)
                ? (value / 256d).ToString("0.##")
                : null;
        }

        public string? GetSaturationDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagSaturation, out long value)
                ? (value - 3).ToString()
                : null;
        }

        public string? GetContrastCameraSettingDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagContrast, out long value)
                ? (value - 3).ToString()
                : null;
        }

        public string? GetSharpnessCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagSharpness,
                "Hard", "Normal", "Soft");
        }

        public string? GetSubjectProgramDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagSubjectProgram,
                "None", "Portrait", "Text", "Night Portrait", "Sunset", "Sports Action");
        }

        public string? GetFlashCompensationDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagFlashCompensation, out long value)
                ? $"{(value - 6) / 3d:0.##} EV"
                : null;
        }

        public string? GetIsoSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagIsoSetting,
                "100", "200", "400", "800", "Auto", "64");
        }

        public string? GetCameraModelDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagCameraModel,
                "DiMAGE 7", "DiMAGE 5", "DiMAGE S304", "DiMAGE S404", "DiMAGE 7i", "DiMAGE 7Hi", "DiMAGE A1", "DiMAGE S414");
        }

        public string? GetIntervalModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagIntervalMode,
                "Still Image", "Time Lapse Movie");
        }

        public string? GetFolderNameDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFolderName,
                "Standard Form", "Data Form");
        }

        public string? GetColorModeCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagColorMode,
                "Natural Color", "Black & White", "Vivid Color", "Solarization", "AdobeRGB");
        }

        public string? GetColorFilterDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagColorFilter, out long value)
                ? (value - 3).ToString()
                : null;
        }

        public string? GetBlackAndWhiteFilterDescription()
        {
            return base.GetDescription(OlympusMakernoteDirectory.CameraSettings.TagBlackAndWhiteFilter);
        }

        public string? GetInternalFlashDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagInternalFlash,
                "Did Not Fire", "Fired");
        }

        public string? GetApexBrightnessDescription()
        {
            return Directory.TryGetLong(OlympusMakernoteDirectory.CameraSettings.TagApexBrightnessValue, out long value)
                ? (value / 8d - 6).ToString()
                : null;
        }

        public string? GetSpotFocusPointXCoordinateDescription()
        {
            return base.GetDescription(OlympusMakernoteDirectory.CameraSettings.TagSpotFocusPointXCoordinate);
        }

        public string? GetSpotFocusPointYCoordinateDescription()
        {
            return base.GetDescription(OlympusMakernoteDirectory.CameraSettings.TagSpotFocusPointYCoordinate);
        }

        public string? GetWideFocusZoneDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagWideFocusZone,
                "No Zone or AF Failed", "Center Zone (Horizontal Orientation)", "Center Zone (Vertical Orientation)", "Left Zone", "Right Zone");
        }

        public string? GetFocusModeCameraSettingDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFocusMode,
                "Auto Focus", "Manual Focus");
        }

        public string? GetFocusAreaDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagFocusArea,
                "Wide Focus (Normal)", "Spot Focus");
        }

        public string? GetDecSwitchPositionDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.CameraSettings.TagDecSwitchPosition,
                "Exposure", "Contrast", "Saturation", "Filter");
        }

        public string? GetMakernoteVersionDescription()
        {
            return GetVersionBytesDescription(OlympusMakernoteDirectory.TagMakernoteVersion, 2);
        }

        public string? GetImageQuality2Description()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagImageQuality2,
                "Raw", "Super Fine", "Fine", "Standard", "Extra Fine");
        }

        public string? GetImageQuality1Description()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagImageQuality1,
                "Raw", "Super Fine", "Fine", "Standard", "Extra Fine");
        }

        public string? GetColorModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagColourMode,
                "Natural Colour", "Black & White", "Vivid Colour", "Solarization", "AdobeRGB");
        }

        public string? GetSharpnessDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagSharpness,
                "Normal", "Hard", "Soft");
        }

        public string? GetColorMatrixDescription()
        {
            var values = Directory.GetArray<short[]>(OlympusMakernoteDirectory.TagColourMatrix);

            if (values is null)
                return null;

            return string.Join(" ", values.Select(b => b.ToString()).ToArray());
        }

        public string? GetWbModeDescription()
        {
            var values = Directory.GetArray<short[]>(OlympusMakernoteDirectory.TagWbMode);

            if (values is null)
                return null;

            return $"{values[0]} {values[1]}".Trim() switch
            {
                "1" or "1 0" => "Auto",
                "1 2" => "Auto (2)",
                "1 4" => "Auto (4)",
                "2 2" => "3000 Kelvin",
                "2 3" => "3700 Kelvin",
                "2 4" => "4000 Kelvin",
                "2 5" => "4500 Kelvin",
                "2 6" => "5500 Kelvin",
                "2 7" => "6500 Kelvin",
                "2 8" => "7500 Kelvin",
                "3 0" => "One-touch",
                _ => $"Unknown ({values[0]} {values[1]})",
            };
        }

        public string? GetRedBalanceDescription()
        {
            var values = Directory.GetArray<ushort[]>(OlympusMakernoteDirectory.TagRedBalance);

            if (values is null || values.Length < 2)
                return null;

            return (values[0] / 256.0d).ToString();
        }

        public string? GetBlueBalanceDescription()
        {
            var values = Directory.GetArray<ushort[]>(OlympusMakernoteDirectory.TagBlueBalance);

            if (values is null || values.Length < 2)
                return null;

            return (values[0] / 256.0d).ToString();
        }

        public string? GetContrastDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagContrast,
                "High", "Normal", "Low");
        }

        public string? GetPreviewImageValidDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagPreviewImageValid,
                "No", "Yes");
        }

        public string? GetFocusModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagFocusMode,
                "Auto", "Manual");
        }

        public string? GetFocusRangeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagFocusRange,
                "Normal", "Macro");
        }

        public string? GetFlashModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagFlashMode,
                null, null, "On", "Off");
        }

        public string? GetDigitalZoomDescription()
        {
            if (!Directory.TryGetRational(OlympusMakernoteDirectory.TagDigitalZoom, out Rational value))
                return null;
            return value.ToSimpleString(allowDecimal: false);
        }

        public string? GetFocalPlaneDiagonalDescription()
        {
            if (!Directory.TryGetRational(OlympusMakernoteDirectory.TagFocalPlaneDiagonal, out Rational value))
                return null;
            return value.ToDouble().ToString("0.###") + " mm";
        }

        public string? GetCameraTypeDescription()
        {
            var cameratype = Directory.GetString(OlympusMakernoteDirectory.TagCameraType);
            if (cameratype is null)
                return null;

            if (OlympusMakernoteDirectory.OlympusCameraTypes.TryGetValue(cameratype, out var mapped))
                return mapped;

            return cameratype;
        }

        public string? GetCameraIdDescription()
        {
            var bytes = Directory.GetArray<byte[]>(OlympusMakernoteDirectory.TagCameraId);
            if (bytes is null)
                return null;

            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public string? GetOneTouchWbDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagOneTouchWb,
                "Off", "On", "On (Preset)");
        }

        public string? GetShutterSpeedDescription()
        {
            return GetShutterSpeedDescription(OlympusMakernoteDirectory.TagShutterSpeedValue);
        }

        public string? GetIsoValueDescription()
        {
            if (!Directory.TryGetRational(OlympusMakernoteDirectory.TagIsoValue, out Rational value))
                return null;

            return Math.Round(Math.Pow(2, value.ToDouble() - 5) * 100, 0).ToString();
        }

        public string? GetApertureValueDescription()
        {
            if (!Directory.TryGetDouble(OlympusMakernoteDirectory.TagApertureValue, out double aperture))
                return null;
            return GetFStopDescription(PhotographicConversions.ApertureToFStop(aperture));
        }

        public string? GetMacroModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagMacroMode,
                "Normal (no macro)", "Macro");
        }

        public string? GetBwModeDescription()
        {
            return GetIndexedDescription(OlympusMakernoteDirectory.TagBwMode,
                "Off", "On");
        }

        public string? GetJpegQualityDescription()
        {
            var cameratype = Directory.GetString(OlympusMakernoteDirectory.TagCameraType);

            if (cameratype is not null)
            {
                if (!Directory.TryGetInt(OlympusMakernoteDirectory.TagJpegQuality, out int value))
                    return null;

                if ((cameratype.StartsWith("SX", StringComparison.OrdinalIgnoreCase) && !cameratype.StartsWith("SX151", StringComparison.OrdinalIgnoreCase))
                    || cameratype.StartsWith("D4322", StringComparison.OrdinalIgnoreCase))
                {
                    return value switch
                    {
                        0 => "Standard Quality (Low)",
                        1 => "High Quality (Normal)",
                        2 => "Super High Quality (Fine)",
                        6 => "RAW",
                        _ => "Unknown (" + value + ")",
                    };
                }
                else
                {
                    return value switch
                    {
                        0 => "Standard Quality (Low)",
                        1 => "High Quality (Normal)",
                        2 => "Super High Quality (Fine)",
                        4 => "RAW",
                        5 => "Medium-Fine",
                        6 => "Small-Fine",
                        33 => "Uncompressed",
                        _ => "Unknown (" + value + ")",
                    };
                }
            }
            else
                return GetIndexedDescription(OlympusMakernoteDirectory.TagJpegQuality,
                    1,
                    "Standard Quality", "High Quality", "Super High Quality");
        }

        public string? GetSpecialModeDescription()
        {
            var values = Directory.GetArray<uint[]>(OlympusMakernoteDirectory.TagSpecialMode);

            if (values is null)
                return null;
            if (values.Length < 1)
                return string.Empty;

            var description = new StringBuilder();

            switch (values[0])
            {
                case 0:
                    description.Append("Normal picture taking mode");
                    break;
                case 1:
                    description.Append("Unknown picture taking mode");
                    break;
                case 2:
                    description.Append("Fast picture taking mode");
                    break;
                case 3:
                    description.Append("Panorama picture taking mode");
                    break;
                default:
                    // TODO return at this point, as future values are not likely to be any good
                    description.Append("Unknown picture taking mode");
                    break;
            }

            if (values.Length >= 2)
            {
                switch (values[1])
                {
                    case 0:
                        break;
                    case 1:
                        description.Append(" / 1st in a sequence");
                        break;
                    case 2:
                        description.Append(" / 2nd in a sequence");
                        break;
                    case 3:
                        description.Append(" / 3rd in a sequence");
                        break;
                    default:
                        description.AppendFormat(" / {0}th in a sequence", values[1]);
                        break;
                }
            }

            if (values.Length >= 3)
            {
                switch (values[2])
                {
                    case 1:
                        description.Append(" / Left to right panorama direction");
                        break;
                    case 2:
                        description.Append(" / Right to left panorama direction");
                        break;
                    case 3:
                        description.Append(" / Bottom to top panorama direction");
                        break;
                    case 4:
                        description.Append(" / Top to bottom panorama direction");
                        break;
                }
            }

            return description.ToString();
        }
    }
}
