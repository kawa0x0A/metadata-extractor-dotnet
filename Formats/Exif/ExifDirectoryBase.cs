namespace MetadataExtractor.Formats.Exif
{
    public abstract class ExifDirectoryBase(Dictionary<int, string> tagNameMap) : Directory(tagNameMap)
    {
        public const int TagInteropIndex = 0x0001;

        public const int TagInteropVersion = 0x0002;

        public const int TagNewSubfileType = 0x00FE;

        public const int TagSubfileType = 0x00FF;

        public const int TagImageWidth = 0x0100;

        public const int TagImageHeight = 0x0101;

        public const int TagBitsPerSample = 0x0102;

        public const int TagCompression = 0x0103;

        public const int TagPhotometricInterpretation = 0x0106;

        public const int TagThresholding = 0x0107;

        public const int TagFillOrder = 0x010A;

        public const int TagDocumentName = 0x010D;

        public const int TagImageDescription = 0x010E;

        public const int TagMake = 0x010F;

        public const int TagModel = 0x0110;

        public const int TagStripOffsets = 0x0111;

        public const int TagOrientation = 0x0112;

        public const int TagSamplesPerPixel = 0x0115;

        public const int TagRowsPerStrip = 0x0116;

        public const int TagStripByteCounts = 0x0117;

        public const int TagMinSampleValue = 0x0118;

        public const int TagMaxSampleValue = 0x0119;

        public const int TagXResolution = 0x011A;

        public const int TagYResolution = 0x011B;

        public const int TagPlanarConfiguration = 0x011C;

        public const int TagPageName = 0x011D;

        public const int TagResolutionUnit = 0x0128;
        public const int TagPageNumber = 0x0129;

        public const int TagTransferFunction = 0x012D;

        public const int TagSoftware = 0x0131;

        public const int TagDateTime = 0x0132;

        public const int TagArtist = 0x013B;

        public const int TagHostComputer = 0x013C;

        public const int TagPredictor = 0x013D;

        public const int TagWhitePoint = 0x013E;

        public const int TagPrimaryChromaticities = 0x013F;

        public const int TagTileWidth = 0x0142;

        public const int TagTileLength = 0x0143;

        public const int TagTileOffsets = 0x0144;

        public const int TagTileByteCounts = 0x0145;

        public const int TagSubIfdOffset = 0x014a;

        public const int TagExtraSamples = 0x0152;
        public const int TagSampleFormat = 0x0153;

        public const int TagTransferRange = 0x0156;

        public const int TagJpegTables = 0x015B;

        public const int TagJpegProc = 0x0200;

        public const int TagJpegRestartInterval = 0x0203;
        public const int TagJpegLosslessPredictors = 0x0205;
        public const int TagJpegPointTransforms = 0x0206;
        public const int TagJpegQTables = 0x0207;
        public const int TagJpegDcTables = 0x0208;
        public const int TagJpegAcTables = 0x0209;

        public const int TagYCbCrCoefficients = 0x0211;

        public const int TagYCbCrSubsampling = 0x0212;

        public const int TagYCbCrPositioning = 0x0213;

        public const int TagReferenceBlackWhite = 0x0214;

        public const int TagStripRowCounts = 0x022F;

        public const int TagApplicationNotes = 0x02BC;

        public const int TagRelatedImageFileFormat = 0x1000;

        public const int TagRelatedImageWidth = 0x1001;

        public const int TagRelatedImageHeight = 0x1002;

        public const int TagRating = 0x4746;

        public const int TagRatingPercent = 0x4749;

        public const int TagCfaRepeatPatternDim = 0x828D;

        public const int TagCfaPattern2 = 0x828E;

        public const int TagBatteryLevel = 0x828F;

        public const int TagCopyright = 0x8298;

        public const int TagExposureTime = 0x829A;

        public const int TagFNumber = 0x829D;

        public const int TagPixelScale = 0x830E;

        public const int TagIptcNaa = 0x83BB;

        public const int TagModelTiePoint = 0x8482;

        public const int TagPhotoshopSettings = 0x8649;

        public const int TagInterColorProfile = 0x8773;

        public const int TagGeoTiffGeoKeys = 0x87af;

        public const int TagGeoTiffGeoDoubleParams = 0x87b0;

        public const int TagGeoTiffGeoAsciiParams = 0x87b1;

        public const int TagExposureProgram = 0x8822;

        public const int TagSpectralSensitivity = 0x8824;

        public const int TagIsoEquivalent = 0x8827;

        public const int TagOptoElectricConversionFunction = 0x8828;

        public const int TagInterlace = 0x8829;

        [Obsolete("Use TagTimeZoneOffset instead.")]
        public const int TagTimeZoneOffsetTiffEp = 0x882A;

        [Obsolete("Use TagSelfTimerMode instead.")]
        public const int TagSelfTimerModeTiffEp = 0x882B;

        public const int TagTimeZoneOffset = 0x882A;

        public const int TagSelfTimerMode = 0x882B;

        public const int TagSensitivityType = 0x8830;

        public const int TagStandardOutputSensitivity = 0x8831;

        public const int TagRecommendedExposureIndex = 0x8832;

        public const int TagIsoSpeed = 0x8833;

        public const int TagIsoSpeedLatitudeYYY = 0x8834;

        public const int TagIsoSpeedLatitudeZZZ = 0x8835;

        public const int TagExifVersion = 0x9000;

        public const int TagDateTimeOriginal = 0x9003;

        public const int TagDateTimeDigitized = 0x9004;

        public const int TagTimeZone = 0x9010;

        public const int TagTimeZoneOriginal = 0x9011;

        public const int TagTimeZoneDigitized = 0x9012;

        public const int TagComponentsConfiguration = 0x9101;

        public const int TagCompressedAverageBitsPerPixel = 0x9102;

        public const int TagShutterSpeed = 0x9201;

        public const int TagAperture = 0x9202;

        public const int TagBrightnessValue = 0x9203;

        public const int TagExposureBias = 0x9204;

        public const int TagMaxAperture = 0x9205;

        public const int TagSubjectDistance = 0x9206;

        public const int TagMeteringMode = 0x9207;

        public const int TagWhiteBalance = 0x9208;

        public const int TagFlash = 0x9209;

        public const int TagFocalLength = 0x920A;

        public const int TagFlashEnergyTiffEp = 0x920B;

        public const int TagSpatialFreqResponseTiffEp = 0x920C;

        public const int TagNoise = 0x920D;

        public const int TagFocalPlaneXResolutionTiffEp = 0x920E;

        public const int TagFocalPlaneYResolutionTiffEp = 0x920F;

        public const int TagImageNumber = 0x9211;

        public const int TagSecurityClassification = 0x9212;

        public const int TagImageHistory = 0x9213;

        public const int TagSubjectLocationTiffEp = 0x9214;

        public const int TagExposureIndexTiffEp = 0x9215;

        public const int TagStandardIdTiffEp = 0x9216;

        public const int TagMakernote = 0x927C;

        public const int TagUserComment = 0x9286;

        public const int TagSubsecondTime = 0x9290;

        public const int TagSubsecondTimeOriginal = 0x9291;

        public const int TagSubsecondTimeDigitized = 0x9292;

        public const int TagWinTitle = 0x9C9B;

        public const int TagWinComment = 0x9C9C;

        public const int TagWinAuthor = 0x9C9D;

        public const int TagWinKeywords = 0x9C9E;

        public const int TagWinSubject = 0x9C9F;

        public const int TagFlashpixVersion = 0xA000;

        public const int TagColorSpace = 0xA001;

        public const int TagExifImageWidth = 0xA002;

        public const int TagExifImageHeight = 0xA003;

        public const int TagRelatedSoundFile = 0xA004;

        public const int TagFlashEnergy = 0xA20B;

        public const int TagSpatialFreqResponse = 0xA20C;

        public const int TagFocalPlaneXResolution = 0xA20E;

        public const int TagFocalPlaneYResolution = 0xA20F;

        public const int TagFocalPlaneResolutionUnit = 0xA210;

        public const int TagSubjectLocation = 0xA214;

        public const int TagExposureIndex = 0xA215;

        public const int TagSensingMethod = 0xA217;

        public const int TagFileSource = 0xA300;

        public const int TagSceneType = 0xA301;

        public const int TagCfaPattern = 0xA302;

        public const int TagCustomRendered = 0xA401;

        public const int TagExposureMode = 0xA402;

        public const int TagWhiteBalanceMode = 0xA403;

        public const int TagDigitalZoomRatio = 0xA404;

        public const int Tag35MMFilmEquivFocalLength = 0xA405;

        public const int TagSceneCaptureType = 0xA406;

        public const int TagGainControl = 0xA407;

        public const int TagContrast = 0xA408;

        public const int TagSaturation = 0xA409;

        public const int TagSharpness = 0xA40A;

        public const int TagDeviceSettingDescription = 0xA40B;

        public const int TagSubjectDistanceRange = 0xA40C;

        public const int TagImageUniqueId = 0xA420;

        public const int TagCameraOwnerName = 0xA430;

        public const int TagBodySerialNumber = 0xA431;

        public const int TagLensSpecification = 0xA432;

        public const int TagLensMake = 0xA433;

        public const int TagLensModel = 0xA434;

        public const int TagLensSerialNumber = 0xA435;

        public const int TagGdalMetadata = 0xA480;
        public const int TagGdalNoData = 0xA481;

        public const int TagGamma = 0xA500;

        public const int TagPrintImageMatchingInfo = 0xC4A5;

        public const int TagPanasonicTitle = 0xC6D2;

        public const int TagPanasonicTitle2 = 0xC6D3;

        public const int TagPadding = 0xEA1C;

        public const int TagLens = 0xFDEA;

        protected static void AddExifTagNames(Dictionary<int, string> map)
        {
            map[TagInteropIndex] = "Interoperability Index";
            map[TagInteropVersion] = "Interoperability Version";
            map[TagNewSubfileType] = "New Subfile Type";
            map[TagSubfileType] = "Subfile Type";
            map[TagImageWidth] = "Image Width";
            map[TagImageHeight] = "Image Height";
            map[TagBitsPerSample] = "Bits Per Sample";
            map[TagCompression] = "Compression";
            map[TagPhotometricInterpretation] = "Photometric Interpretation";
            map[TagThresholding] = "Thresholding";
            map[TagFillOrder] = "Fill Order";
            map[TagDocumentName] = "Document Name";
            map[TagImageDescription] = "Image Description";
            map[TagMake] = "Make";
            map[TagModel] = "Model";
            map[TagStripOffsets] = "Strip Offsets";
            map[TagOrientation] = "Orientation";
            map[TagSamplesPerPixel] = "Samples Per Pixel";
            map[TagRowsPerStrip] = "Rows Per Strip";
            map[TagStripByteCounts] = "Strip Byte Counts";
            map[TagMinSampleValue] = "Minimum Sample Value";
            map[TagMaxSampleValue] = "Maximum Sample Value";
            map[TagXResolution] = "X Resolution";
            map[TagYResolution] = "Y Resolution";
            map[TagPlanarConfiguration] = "Planar Configuration";
            map[TagPageName] = "Page Name";
            map[TagResolutionUnit] = "Resolution Unit";
            map[TagPageNumber] = "Page Number";
            map[TagTransferFunction] = "Transfer Function";
            map[TagSoftware] = "Software";
            map[TagDateTime] = "Date/Time";
            map[TagArtist] = "Artist";
            map[TagPredictor] = "Predictor";
            map[TagHostComputer] = "Host Computer";
            map[TagWhitePoint] = "White Point";
            map[TagPrimaryChromaticities] = "Primary Chromaticities";
            map[TagTileWidth] = "Tile Width";
            map[TagTileLength] = "Tile Length";
            map[TagTileOffsets] = "Tile Offsets";
            map[TagTileByteCounts] = "Tile Byte Counts";
            map[TagSubIfdOffset] = "Sub IFD Pointer(s)";
            map[TagExtraSamples] = "Extra Samples";
            map[TagSampleFormat] = "Sample Format";
            map[TagTransferRange] = "Transfer Range";
            map[TagJpegTables] = "JPEG Tables";
            map[TagJpegProc] = "JPEG Proc";

            map[TagJpegRestartInterval] = "JPEG Restart Interval";
            map[TagJpegLosslessPredictors] = "JPEG Lossless Predictors";
            map[TagJpegPointTransforms] = "JPEG Point Transforms";
            map[TagJpegQTables] = "JPEGQ Tables";
            map[TagJpegDcTables] = "JPEGDC Tables";
            map[TagJpegAcTables] = "JPEGAC Tables";

            map[TagYCbCrCoefficients] = "YCbCr Coefficients";
            map[TagYCbCrSubsampling] = "YCbCr Sub-Sampling";
            map[TagYCbCrPositioning] = "YCbCr Positioning";
            map[TagReferenceBlackWhite] = "Reference Black/White";
            map[TagStripRowCounts] = "Strip Row Counts";
            map[TagApplicationNotes] = "Application Notes";
            map[TagRelatedImageFileFormat] = "Related Image File Format";
            map[TagRelatedImageWidth] = "Related Image Width";
            map[TagRelatedImageHeight] = "Related Image Height";
            map[TagRating] = "Rating";
            map[TagRatingPercent] = "Rating Percent";
            map[TagCfaRepeatPatternDim] = "CFA Repeat Pattern Dim";
            map[TagCfaPattern2] = "CFA Pattern";
            map[TagBatteryLevel] = "Battery Level";
            map[TagCopyright] = "Copyright";
            map[TagExposureTime] = "Exposure Time";
            map[TagFNumber] = "F-Number";
            map[TagPixelScale] = "Pixel Scale";
            map[TagIptcNaa] = "IPTC/NAA";
            map[TagModelTiePoint] = "Model Tie Point";
            map[TagPhotoshopSettings] = "Photoshop Settings";
            map[TagInterColorProfile] = "Inter Color Profile";
            map[TagExposureProgram] = "Exposure Program";
            map[TagSpectralSensitivity] = "Spectral Sensitivity";
            map[TagIsoEquivalent] = "ISO Speed Ratings";
            map[TagOptoElectricConversionFunction] = "Opto-electric Conversion Function (OECF)";
            map[TagInterlace] = "Interlace";
            map[TagTimeZoneOffset] = "Time Zone Offset";
            map[TagSelfTimerMode] = "Self Timer Mode";
            map[TagSensitivityType] = "Sensitivity Type";
            map[TagStandardOutputSensitivity] = "Standard Output Sensitivity";
            map[TagRecommendedExposureIndex] = "Recommended Exposure Index";
            map[TagIsoSpeed] = "ISO Speed";
            map[TagIsoSpeedLatitudeYYY] = "ISO Speed Latitude yyy";
            map[TagIsoSpeedLatitudeZZZ] = "ISO Speed Latitude zzz";
            map[TagExifVersion] = "Exif Version";
            map[TagDateTimeOriginal] = "Date/Time Original";
            map[TagDateTimeDigitized] = "Date/Time Digitized";
            map[TagTimeZone] = "Time Zone";
            map[TagTimeZoneOriginal] = "Time Zone Original";
            map[TagTimeZoneDigitized] = "Time Zone Digitized";
            map[TagComponentsConfiguration] = "Components Configuration";
            map[TagCompressedAverageBitsPerPixel] = "Compressed Bits Per Pixel";
            map[TagShutterSpeed] = "Shutter Speed Value";
            map[TagAperture] = "Aperture Value";
            map[TagBrightnessValue] = "Brightness Value";
            map[TagExposureBias] = "Exposure Bias Value";
            map[TagMaxAperture] = "Max Aperture Value";
            map[TagSubjectDistance] = "Subject Distance";
            map[TagMeteringMode] = "Metering Mode";
            map[TagWhiteBalance] = "White Balance";
            map[TagFlash] = "Flash";
            map[TagFocalLength] = "Focal Length";
            map[TagFlashEnergyTiffEp] = "Flash Energy";
            map[TagSpatialFreqResponseTiffEp] = "Spatial Frequency Response";
            map[TagNoise] = "Noise";
            map[TagFocalPlaneXResolutionTiffEp] = "Focal Plane X Resolution";
            map[TagFocalPlaneYResolutionTiffEp] = "Focal Plane Y Resolution";
            map[TagImageNumber] = "Image Number";
            map[TagSecurityClassification] = "Security Classification";
            map[TagImageHistory] = "Image History";
            map[TagSubjectLocationTiffEp] = "Subject Location";
            map[TagExposureIndexTiffEp] = "Exposure Index";
            map[TagStandardIdTiffEp] = "TIFF/EP Standard ID";
            map[TagMakernote] = "Makernote";
            map[TagUserComment] = "User Comment";
            map[TagSubsecondTime] = "Sub-Sec Time";
            map[TagSubsecondTimeOriginal] = "Sub-Sec Time Original";
            map[TagSubsecondTimeDigitized] = "Sub-Sec Time Digitized";
            map[TagWinTitle] = "Windows XP Title";
            map[TagWinComment] = "Windows XP Comment";
            map[TagWinAuthor] = "Windows XP Author";
            map[TagWinKeywords] = "Windows XP Keywords";
            map[TagWinSubject] = "Windows XP Subject";
            map[TagFlashpixVersion] = "FlashPix Version";
            map[TagColorSpace] = "Color Space";
            map[TagExifImageWidth] = "Exif Image Width";
            map[TagExifImageHeight] = "Exif Image Height";
            map[TagRelatedSoundFile] = "Related Sound File";
            map[TagFlashEnergy] = "Flash Energy";
            map[TagSpatialFreqResponse] = "Spatial Frequency Response";
            map[TagFocalPlaneXResolution] = "Focal Plane X Resolution";
            map[TagFocalPlaneYResolution] = "Focal Plane Y Resolution";
            map[TagFocalPlaneResolutionUnit] = "Focal Plane Resolution Unit";
            map[TagSubjectLocation] = "Subject Location";
            map[TagExposureIndex] = "Exposure Index";
            map[TagSensingMethod] = "Sensing Method";
            map[TagFileSource] = "File Source";
            map[TagSceneType] = "Scene Type";
            map[TagCfaPattern] = "CFA Pattern";
            map[TagCustomRendered] = "Custom Rendered";
            map[TagExposureMode] = "Exposure Mode";
            map[TagWhiteBalanceMode] = "White Balance Mode";
            map[TagDigitalZoomRatio] = "Digital Zoom Ratio";
            map[Tag35MMFilmEquivFocalLength] = "Focal Length 35";
            map[TagSceneCaptureType] = "Scene Capture Type";
            map[TagGainControl] = "Gain Control";
            map[TagContrast] = "Contrast";
            map[TagSaturation] = "Saturation";
            map[TagSharpness] = "Sharpness";
            map[TagDeviceSettingDescription] = "Device Setting Description";
            map[TagSubjectDistanceRange] = "Subject Distance Range";
            map[TagImageUniqueId] = "Unique Image ID";
            map[TagCameraOwnerName] = "Camera Owner Name";
            map[TagBodySerialNumber] = "Body Serial Number";
            map[TagLensSpecification] = "Lens Specification";
            map[TagLensMake] = "Lens Make";
            map[TagLensModel] = "Lens Model";
            map[TagLensSerialNumber] = "Lens Serial Number";
            map[TagGdalMetadata] = "GDAL Metadata";
            map[TagGdalNoData] = "GDAL NoData";
            map[TagGamma] = "Gamma";
            map[TagPrintImageMatchingInfo] = "Print Image Matching (PIM) Info";
            map[TagPanasonicTitle] = "Panasonic Title";
            map[TagPanasonicTitle2] = "Panasonic Title (2)";
            map[TagPadding] = "Padding";
            map[TagLens] = "Lens";
        }
    }
}
