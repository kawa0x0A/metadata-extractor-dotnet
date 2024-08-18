namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class NikonType2MakernoteDirectory : Directory
    {
        public const int TagFirmwareVersion = 0x0001;

        public const int TagIso1 = 0x0002;

        public const int TagColorMode = 0x0003;

        public const int TagQualityAndFileFormat = 0x0004;

        public const int TagCameraWhiteBalance = 0x0005;

        public const int TagCameraSharpening = 0x0006;

        public const int TagAfType = 0x0007;

        public const int TagFlashSyncMode = 0x0008;

        public const int TagAutoFlashMode = 0x0009;

        public const int TagUnknown34 = 0x000A;

        public const int TagCameraWhiteBalanceFine = 0x000B;

        public const int TagCameraWhiteBalanceRbCoeff = 0x000C;

        public const int TagProgramShift = 0x000D;

        public const int TagExposureDifference = 0x000E;

        public const int TagIsoMode = 0x000F;

        public const int TagDataDump = 0x0010;

        public const int TagPreviewIfd = 0x0011;

        public const int TagAutoFlashCompensation = 0x0012;

        public const int TagIsoRequested = 0x0013;

        public const int TagImageBoundary = 0x0016;

        public const int TagFlashExposureCompensation = 0x0017;

        public const int TagFlashBracketCompensation = 0x0018;

        public const int TagAeBracketCompensation = 0x0019;

        public const int TagFlashMode = 0x001a;

        public const int TagCropHighSpeed = 0x001b;

        public const int TagExposureTuning = 0x001c;

        public const int TagCameraSerialNumber = 0x001d;

        public const int TagColorSpace = 0x001e;

        public const int TagVrInfo = 0x001f;

        public const int TagImageAuthentication = 0x0020;

        public const int TagFaceDetect = 0x0021;

        public const int TagActiveDLighting = 0x0022;

        public const int TagPictureControl = 0x0023;

        public const int TagWorldTime = 0x0024;

        public const int TagIsoInfo = 0x0025;

        public const int TagUnknown36 = 0x0026;

        public const int TagUnknown37 = 0x0027;

        public const int TagUnknown38 = 0x0028;

        public const int TagUnknown39 = 0x0029;

        public const int TagVignetteControl = 0x002a;

        public const int TagDistortInfo = 0x002b;

        public const int TagUnknown41 = 0x002c;

        public const int TagUnknown42 = 0x002d;

        public const int TagUnknown43 = 0x002e;

        public const int TagUnknown44 = 0x002f;

        public const int TagUnknown45 = 0x0030;

        public const int TagUnknown46 = 0x0031;

        public const int TagImageAdjustment = 0x0080;

        public const int TagCameraToneCompensation = 0x0081;

        public const int TagAdapter = 0x0082;

        public const int TagLensType = 0x0083;

        public const int TagLens = 0x0084;

        public const int TagManualFocusDistance = 0x0085;

        public const int TagDigitalZoom = 0x0086;

        public const int TagFlashUsed = 0x0087;

        public const int TagAfFocusPosition = 0x0088;

        public const int TagShootingMode = 0x0089;

        public const int TagUnknown20 = 0x008A;

        public const int TagLensStops = 0x008B;

        public const int TagContrastCurve = 0x008C;

        public const int TagCameraColorMode = 0x008D;

        public const int TagUnknown47 = 0x008E;

        public const int TagSceneMode = 0x008F;

        public const int TagLightSource = 0x0090;

        public const int TagShotInfo = 0x0091;

        public const int TagCameraHueAdjustment = 0x0092;

        public const int TagNefCompression = 0x0093;

        public const int TagSaturation = 0x0094;

        public const int TagNoiseReduction = 0x0095;

        public const int TagLinearizationTable = 0x0096;

        public const int TagColorBalance = 0x0097;

        public const int TagLensData = 0x0098;

        public const int TagNefThumbnailSize = 0x0099;

        public const int TagSensorPixelSize = 0x009A;

        public const int TagUnknown10 = 0x009B;

        public const int TagSceneAssist = 0x009C;

        public const int TagDateStampMode = 0x009D;

        public const int TagRetouchHistory = 0x009E;

        public const int TagUnknown12 = 0x009F;

        public const int TagCameraSerialNumber2 = 0x00A0;

        public const int TagImageDataSize = 0x00A2;

        public const int TagUnknown27 = 0x00A3;

        public const int TagUnknown28 = 0x00A4;

        public const int TagImageCount = 0x00A5;

        public const int TagDeletedImageCount = 0x00A6;

        public const int TagExposureSequenceNumber = 0x00A7;

        public const int TagFlashInfo = 0x00A8;

        public const int TagImageOptimisation = 0x00A9;

        public const int TagSaturation2 = 0x00AA;

        public const int TagDigitalVariProgram = 0x00AB;

        public const int TagImageStabilisation = 0x00AC;

        public const int TagAfResponse = 0x00AD;

        public const int TagUnknown29 = 0x00AE;

        public const int TagUnknown30 = 0x00AF;

        public const int TagMultiExposure = 0x00B0;

        public const int TagHighIsoNoiseReduction = 0x00B1;

        public const int TagUnknown31 = 0x00B2;

        public const int TagToningEffect = 0x00B3;

        public const int TagUnknown33 = 0x00B4;

        public const int TagUnknown48 = 0x00B5;

        public const int TagPowerUpTime = 0x00B6;

        public const int TagAfInfo2 = 0x00B7;

        public const int TagFileInfo = 0x00B8;

        public const int TagAfTune = 0x00B9;

        public const int TagRetouchInfo = 0x00BB;

        public const int TagPictureControl2 = 0x00BD;

        public const int TagUnknown51 = 0x0103;

        public const int TagPrintImageMatchingInfo = 0x0E00;

        public const int TagNikonCaptureData = 0x0E01;

        public const int TagUnknown52 = 0x0E05;

        public const int TagUnknown53 = 0x0E08;

        public const int TagNikonCaptureVersion = 0x0E09;

        public const int TagNikonCaptureOffsets = 0x0E0E;

        public const int TagNikonScan = 0x0E10;

        public const int TagUnknown54 = 0x0E19;

        public const int TagNefBitDepth = 0x0E22;

        public const int TagUnknown55 = 0x0E23;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagFirmwareVersion, "Firmware Version" },
            { TagIso1, "ISO" },
            { TagQualityAndFileFormat, "Quality & File Format" },
            { TagCameraWhiteBalance, "White Balance" },
            { TagCameraSharpening, "Sharpening" },
            { TagAfType, "AF Type" },
            { TagCameraWhiteBalanceFine, "White Balance Fine" },
            { TagCameraWhiteBalanceRbCoeff, "White Balance RB Coefficients" },
            { TagIsoRequested, "ISO" },
            { TagIsoMode, "ISO Mode" },
            { TagDataDump, "Data Dump" },
            { TagProgramShift, "Program Shift" },
            { TagExposureDifference, "Exposure Difference" },
            { TagPreviewIfd, "Preview IFD" },
            { TagLensType, "Lens Type" },
            { TagFlashUsed, "Flash Used" },
            { TagAfFocusPosition, "AF Focus Position" },
            { TagShootingMode, "Shooting Mode" },
            { TagLensStops, "Lens Stops" },
            { TagContrastCurve, "Contrast Curve" },
            { TagLightSource, "Light source" },
            { TagShotInfo, "Shot Info" },
            { TagColorBalance, "Color Balance" },
            { TagLensData, "Lens Data" },
            { TagNefThumbnailSize, "NEF Thumbnail Size" },
            { TagSensorPixelSize, "Sensor Pixel Size" },
            { TagUnknown10, "Unknown 10" },
            { TagSceneAssist, "Scene Assist" },
            { TagDateStampMode, "Date Stamp Mode" },
            { TagRetouchHistory, "Retouch History" },
            { TagUnknown12, "Unknown 12" },
            { TagFlashSyncMode, "Flash Sync Mode" },
            { TagAutoFlashMode, "Auto Flash Mode" },
            { TagAutoFlashCompensation, "Auto Flash Compensation" },
            { TagExposureSequenceNumber, "Exposure Sequence Number" },
            { TagColorMode, "Color Mode" },
            { TagUnknown20, "Unknown 20" },
            { TagImageBoundary, "Image Boundary" },
            { TagFlashExposureCompensation, "Flash Exposure Compensation" },
            { TagFlashBracketCompensation, "Flash Bracket Compensation" },
            { TagAeBracketCompensation, "AE Bracket Compensation" },
            { TagFlashMode, "Flash Mode" },
            { TagCropHighSpeed, "Crop High Speed" },
            { TagExposureTuning, "Exposure Tuning" },
            { TagCameraSerialNumber, "Camera Serial Number" },
            { TagColorSpace, "Color Space" },
            { TagVrInfo, "VR Info" },
            { TagImageAuthentication, "Image Authentication" },
            { TagFaceDetect, "Face Detect" },
            { TagActiveDLighting, "Active D-Lighting" },
            { TagPictureControl, "Picture Control" },
            { TagWorldTime, "World Time" },
            { TagIsoInfo, "ISO Info" },
            { TagUnknown36, "Unknown 36" },
            { TagUnknown37, "Unknown 37" },
            { TagUnknown38, "Unknown 38" },
            { TagUnknown39, "Unknown 39" },
            { TagVignetteControl, "Vignette Control" },
            { TagDistortInfo, "Distort Info" },
            { TagUnknown41, "Unknown 41" },
            { TagUnknown42, "Unknown 42" },
            { TagUnknown43, "Unknown 43" },
            { TagUnknown44, "Unknown 44" },
            { TagUnknown45, "Unknown 45" },
            { TagUnknown46, "Unknown 46" },
            { TagUnknown47, "Unknown 47" },
            { TagSceneMode, "Scene Mode" },
            { TagCameraSerialNumber2, "Camera Serial Number" },
            { TagImageDataSize, "Image Data Size" },
            { TagUnknown27, "Unknown 27" },
            { TagUnknown28, "Unknown 28" },
            { TagImageCount, "Image Count" },
            { TagDeletedImageCount, "Deleted Image Count" },
            { TagSaturation2, "Saturation" },
            { TagDigitalVariProgram, "Digital Vari Program" },
            { TagImageStabilisation, "Image Stabilisation" },
            { TagAfResponse, "AF Response" },
            { TagUnknown29, "Unknown 29" },
            { TagUnknown30, "Unknown 30" },
            { TagMultiExposure, "Multi Exposure" },
            { TagHighIsoNoiseReduction, "High ISO Noise Reduction" },
            { TagUnknown31, "Unknown 31" },
            { TagToningEffect, "Toning Effect" },
            { TagUnknown33, "Unknown 33" },
            { TagUnknown48, "Unknown 48" },
            { TagPowerUpTime, "Power Up Time" },
            { TagAfInfo2, "AF Info 2" },
            { TagFileInfo, "File Info" },
            { TagAfTune, "AF Tune" },
            { TagFlashInfo, "Flash Info" },
            { TagImageOptimisation, "Image Optimisation" },
            { TagImageAdjustment, "Image Adjustment" },
            { TagCameraToneCompensation, "Tone Compensation" },
            { TagAdapter, "Adapter" },
            { TagLens, "Lens" },
            { TagManualFocusDistance, "Manual Focus Distance" },
            { TagDigitalZoom, "Digital Zoom" },
            { TagCameraColorMode, "Colour Mode" },
            { TagCameraHueAdjustment, "Camera Hue Adjustment" },
            { TagNefCompression, "NEF Compression" },
            { TagSaturation, "Saturation" },
            { TagNoiseReduction, "Noise Reduction" },
            { TagLinearizationTable, "Linearization Table" },
            { TagNikonCaptureData, "Nikon Capture Data" },
            { TagRetouchInfo, "Retouch Info" },
            { TagPictureControl2, "Picture Control 2" },
            { TagUnknown51, "Unknown 51" },
            { TagPrintImageMatchingInfo, "Print Image Matching (PIM) Info" },
            { TagUnknown52, "Unknown 52" },
            { TagUnknown53, "Unknown 53" },
            { TagNikonCaptureVersion, "Nikon Capture Version" },
            { TagNikonCaptureOffsets, "Nikon Capture Offsets" },
            { TagNikonScan, "Nikon Scan" },
            { TagUnknown54, "Unknown 54" },
            { TagNefBitDepth, "NEF Bit Depth" },
            { TagUnknown55, "Unknown 55" }
        };

        public NikonType2MakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new NikonType2MakernoteDescriptor(this));
        }

        public override string Name => "Nikon Makernote";
    }
}
