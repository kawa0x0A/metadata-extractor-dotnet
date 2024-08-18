using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class PanasonicMakernoteDirectory : Directory
    {
        public const int TagQualityMode = 0x0001;

        public const int TagFirmwareVersion = 0x0002;

        public const int TagWhiteBalance = 0x0003;

        public const int TagFocusMode = 0x0007;

        public const int TagAfAreaMode = 0x000f;

        public const int TagImageStabilization = 0x001a;

        public const int TagMacroMode = 0x001C;

        public const int TagRecordMode = 0x001F;

        public const int TagAudio = 0x0020;

        public const int TagUnknownDataDump = 0x0021;

        public const int TagEasyMode = 0x0022;

        public const int TagWhiteBalanceBias = 0x0023;

        public const int TagFlashBias = 0x0024;

        public const int TagInternalSerialNumber = 0x0025;

        public const int TagExifVersion = 0x0026;

        public const int TagColorEffect = 0x0028;

        public const int TagUptime = 0x0029;

        public const int TagBurstMode = 0x002a;

        public const int TagSequenceNumber = 0x002b;

        public const int TagContrastMode = 0x002c;

        public const int TagNoiseReduction = 0x002d;

        public const int TagSelfTimer = 0x002e;

        public const int TagRotation = 0x0030;

        public const int TagAfAssistLamp = 0x0031;

        public const int TagColorMode = 0x0032;

        public const int TagBabyAge = 0x0033;

        public const int TagOpticalZoomMode = 0x0034;

        public const int TagConversionLens = 0x0035;

        public const int TagTravelDay = 0x0036;

        public const int TagContrast = 0x0039;

        public const int TagWorldTimeLocation = 0x003a;

        public const int TagTextStamp = 0x003b;

        public const int TagProgramIso = 0x003c;

        public const int TagAdvancedSceneMode = 0x003d;

        public const int TagTextStamp1 = 0x003e;

        public const int TagFacesDetected = 0x003f;
        public const int TagSaturation = 0x0040;
        public const int TagSharpness = 0x0041;
        public const int TagFilmMode = 0x0042;

        public const int TagColorTempKelvin = 0x0044;
        public const int TagBracketSettings = 0x0045;

        public const int TagWbAdjustAb = 0x0046;

        public const int TagWbAdjustGm = 0x0047;

        public const int TagFlashCurtain = 0x0048;
        public const int TagLongExposureNoiseReduction = 0x0049;

        public const int TagPanasonicImageWidth = 0x004b;
        public const int TagPanasonicImageHeight = 0x004c;
        public const int TagAfPointPosition = 0x004d;

        public const int TagFaceDetectionInfo = 0x004e;

        public const int TagLensType = 0x0051;
        public const int TagLensSerialNumber = 0x0052;
        public const int TagAccessoryType = 0x0053;
        public const int TagAccessorySerialNumber = 0x0054;

        public const int TagTransform = 0x0059;

        public const int TagIntelligentExposure = 0x005d;

        public const int TagLensFirmwareVersion = 0x0060;
        public const int TagBurstSpeed = 0x0077;
        public const int TagIntelligentDRange = 0x0079;
        public const int TagClearRetouch = 0x007c;
        public const int TagCity2 = 0x0080;
        public const int TagPhotoStyle = 0x0089;
        public const int TagShadingCompensation = 0x008a;

        public const int TagAccelerometerZ = 0x008c;
        public const int TagAccelerometerX = 0x008d;
        public const int TagAccelerometerY = 0x008e;
        public const int TagCameraOrientation = 0x008f;
        public const int TagRollAngle = 0x0090;
        public const int TagPitchAngle = 0x0091;
        public const int TagSweepPanoramaDirection = 0x0093;
        public const int TagSweepPanoramaFieldOfView = 0x0094;
        public const int TagTimerRecording = 0x0096;

        public const int TagInternalNDFilter = 0x009d;
        public const int TagHdr = 0x009e;
        public const int TagShutterType = 0x009f;

        public const int TagClearRetouchValue = 0x00a3;
        public const int TagTouchAe = 0x00ab;

        public const int TagPrintImageMatchingInfo = 0x0E00;

        public const int TagFaceRecognitionInfo = 0x0061;

        public const int TagFlashWarning = 0x0062;

        public const int TagRecognizedFaceFlags = 0x0063;
        public const int TagTitle = 0x0065;
        public const int TagBabyName = 0x0066;
        public const int TagLocation = 0x0067;
        public const int TagCountry = 0x0069;
        public const int TagState = 0x006b;
        public const int TagCity = 0x006d;
        public const int TagLandmark = 0x006f;

        public const int TagIntelligentResolution = 0x0070;

        public const int TagMakernoteVersion = 0x8000;
        public const int TagSceneMode = 0x8001;
        public const int TagWbRedLevel = 0x8004;
        public const int TagWbGreenLevel = 0x8005;
        public const int TagWbBlueLevel = 0x8006;
        public const int TagFlashFired = 0x8007;
        public const int TagTextStamp2 = 0x8008;
        public const int TagTextStamp3 = 0x8009;
        public const int TagBabyAge1 = 0x8010;

        public const int TagTransform1 = 0x8012;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagQualityMode, "Quality Mode" },
            { TagFirmwareVersion, "Version" },
            { TagWhiteBalance, "White Balance" },
            { TagFocusMode, "Focus Mode" },
            { TagAfAreaMode, "AF Area Mode" },
            { TagImageStabilization, "Image Stabilization" },
            { TagMacroMode, "Macro Mode" },
            { TagRecordMode, "Record Mode" },
            { TagAudio, "Audio" },
            { TagInternalSerialNumber, "Internal Serial Number" },
            { TagUnknownDataDump, "Unknown Data Dump" },
            { TagEasyMode, "Easy Mode" },
            { TagWhiteBalanceBias, "White Balance Bias" },
            { TagFlashBias, "Flash Bias" },
            { TagExifVersion, "Exif Version" },
            { TagColorEffect, "Color Effect" },
            { TagUptime, "Camera Uptime" },
            { TagBurstMode, "Burst Mode" },
            { TagSequenceNumber, "Sequence Number" },
            { TagContrastMode, "Contrast Mode" },
            { TagNoiseReduction, "Noise Reduction" },
            { TagSelfTimer, "Self Timer" },
            { TagRotation, "Rotation" },
            { TagAfAssistLamp, "AF Assist Lamp" },
            { TagColorMode, "Color Mode" },
            { TagBabyAge, "Baby Age" },
            { TagOpticalZoomMode, "Optical Zoom Mode" },
            { TagConversionLens, "Conversion Lens" },
            { TagTravelDay, "Travel Day" },
            { TagContrast, "Contrast" },
            { TagWorldTimeLocation, "World Time Location" },
            { TagTextStamp, "Text Stamp" },
            { TagProgramIso, "Program ISO" },
            { TagAdvancedSceneMode, "Advanced Scene Mode" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" },
            { TagFacesDetected, "Number of Detected Faces" },
            { TagSaturation, "Saturation" },
            { TagSharpness, "Sharpness" },
            { TagFilmMode, "Film Mode" },
            { TagColorTempKelvin, "Color Temp Kelvin" },
            { TagBracketSettings, "Bracket Settings" },
            { TagWbAdjustAb, "White Balance Adjust (AB)" },
            { TagWbAdjustGm, "White Balance Adjust (GM)" },

            { TagFlashCurtain, "Flash Curtain" },
            { TagLongExposureNoiseReduction, "Long Exposure Noise Reduction" },
            { TagPanasonicImageWidth, "Panasonic Image Width" },
            { TagPanasonicImageHeight, "Panasonic Image Height" },

            { TagAfPointPosition, "Af Point Position" },
            { TagFaceDetectionInfo, "Face Detection Info" },
            { TagLensType, "Lens Type" },
            { TagLensSerialNumber, "Lens Serial Number" },
            { TagAccessoryType, "Accessory Type" },
            { TagAccessorySerialNumber, "Accessory Serial Number" },
            { TagTransform, "Transform" },
            { TagIntelligentExposure, "Intelligent Exposure" },
            { TagLensFirmwareVersion, "Lens Firmware Version" },
            { TagFaceRecognitionInfo, "Face Recognition Info" },
            { TagFlashWarning, "Flash Warning" },
            { TagRecognizedFaceFlags, "Recognized Face Flags" },
            { TagTitle, "Title" },
            { TagBabyName, "Baby Name" },
            { TagLocation, "Location" },
            { TagCountry, "Country" },
            { TagState, "State" },
            { TagCity, "City" },
            { TagLandmark, "Landmark" },
            { TagIntelligentResolution, "Intelligent Resolution" },
            { TagBurstSpeed, "Burst Speed" },
            { TagIntelligentDRange, "Intelligent D-Range" },
            { TagClearRetouch, "Clear Retouch" },
            { TagCity2, "City 2" },
            { TagPhotoStyle, "Photo Style" },
            { TagShadingCompensation, "Shading Compensation" },

            { TagAccelerometerZ, "Accelerometer Z" },
            { TagAccelerometerX, "Accelerometer X" },
            { TagAccelerometerY, "Accelerometer Y" },
            { TagCameraOrientation, "Camera Orientation" },
            { TagRollAngle, "Roll Angle" },
            { TagPitchAngle, "Pitch Angle" },
            { TagSweepPanoramaDirection, "Sweep Panorama Direction" },
            { TagSweepPanoramaFieldOfView, "Sweep Panorama Field Of View" },
            { TagTimerRecording, "Timer Recording" },

            { TagInternalNDFilter, "Internal ND Filter" },
            { TagHdr, "HDR" },
            { TagShutterType, "Shutter Type" },
            { TagClearRetouchValue, "Clear Retouch Value" },
            { TagTouchAe, "Touch AE" },

            { TagMakernoteVersion, "Makernote Version" },
            { TagSceneMode, "Scene Mode" },
            { TagWbRedLevel, "White Balance (Red)" },
            { TagWbGreenLevel, "White Balance (Green)" },
            { TagWbBlueLevel, "White Balance (Blue)" },
            { TagFlashFired, "Flash Fired" },
            { TagTextStamp1, "Text Stamp 1" },
            { TagTextStamp2, "Text Stamp 2" },
            { TagTextStamp3, "Text Stamp 3" },
            { TagBabyAge1, "Baby Age 1" },
            { TagTransform1, "Transform 1" }
        };

        public PanasonicMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new PanasonicMakernoteDescriptor(this));
        }

        public override string Name => "Panasonic Makernote";

        public IEnumerable<Face> GetDetectedFaces()
        {
            return ParseFaces(this.GetArray<byte[]>(TagFaceDetectionInfo), 2, 0, 8);
        }

        public IEnumerable<Face> GetRecognizedFaces()
        {
            return ParseFaces(this.GetArray<byte[]>(TagFaceRecognitionInfo), 4, 20, 44);
        }

        private static IEnumerable<Face> ParseFaces(byte[]? bytes, int firstRecordOffset, int posOffset, int recordLength)
        {
            if (bytes is null)
                yield break;

            var reader = new ByteArrayReader(bytes, isMotorolaByteOrder: false);

            int faceCount = reader.GetUInt16(0);

            if (faceCount == 0 || bytes.Length < firstRecordOffset + faceCount * recordLength)
                yield break;

            posOffset += firstRecordOffset;

            for (int i = 0, recordOffset = firstRecordOffset; i < faceCount; i++, recordOffset += recordLength, posOffset += recordLength)
            {
                yield return new Face(
                    x: reader.GetUInt16(posOffset),
                    y: reader.GetUInt16(posOffset + 2),
                    width: reader.GetUInt16(posOffset + 4),
                    height: reader.GetUInt16(posOffset + 6),
                    name: recordLength == 44 ? reader.GetString(recordOffset, 20, Encoding.UTF8).Trim(' ', '\0') : null,
                    age: recordLength == 44 ? Age.FromPanasonicString(reader.GetString(recordOffset + 28, 20, Encoding.UTF8).Trim(' ', '\0')) : null);
            }
        }

        public Age? GetAge(int tag)
        {
            var ageString = this.GetString(tag);
            return ageString is null ? null : Age.FromPanasonicString(ageString);
        }
    }
}
