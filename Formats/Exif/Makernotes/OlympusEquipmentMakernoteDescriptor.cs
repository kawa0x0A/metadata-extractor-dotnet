namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class OlympusEquipmentMakernoteDescriptor(OlympusEquipmentMakernoteDirectory directory) : TagDescriptor<OlympusEquipmentMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                OlympusEquipmentMakernoteDirectory.TagEquipmentVersion => GetEquipmentVersionDescription(),
                OlympusEquipmentMakernoteDirectory.TagCameraType2 => GetCameraType2Description(),
                OlympusEquipmentMakernoteDirectory.TagFocalPlaneDiagonal => GetFocalPlaneDiagonalDescription(),
                OlympusEquipmentMakernoteDirectory.TagBodyFirmwareVersion => GetBodyFirmwareVersionDescription(),
                OlympusEquipmentMakernoteDirectory.TagLensType => GetLensTypeDescription(),
                OlympusEquipmentMakernoteDirectory.TagLensFirmwareVersion => GetLensFirmwareVersionDescription(),
                OlympusEquipmentMakernoteDirectory.TagMaxApertureAtMinFocal => GetMaxApertureAtMinFocalDescription(),
                OlympusEquipmentMakernoteDirectory.TagMaxApertureAtMaxFocal => GetMaxApertureAtMaxFocalDescription(),
                OlympusEquipmentMakernoteDirectory.TagMaxAperture => GetMaxApertureDescription(),
                OlympusEquipmentMakernoteDirectory.TagLensProperties => GetLensPropertiesDescription(),
                OlympusEquipmentMakernoteDirectory.TagExtender => GetExtenderDescription(),
                OlympusEquipmentMakernoteDirectory.TagFlashType => GetFlashTypeDescription(),
                OlympusEquipmentMakernoteDirectory.TagFlashModel => GetFlashModelDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetEquipmentVersionDescription()
        {
            return GetVersionBytesDescription(OlympusEquipmentMakernoteDirectory.TagEquipmentVersion, 4);
        }

        public string? GetCameraType2Description()
        {
            var cameratype = Directory.GetString(OlympusEquipmentMakernoteDirectory.TagCameraType2);
            if (cameratype is null)
                return null;

            if (OlympusMakernoteDirectory.OlympusCameraTypes.TryGetValue(cameratype, out var mapped))
                return mapped;

            return cameratype;
        }

        public string GetFocalPlaneDiagonalDescription()
        {
            return Directory.GetString(OlympusEquipmentMakernoteDirectory.TagFocalPlaneDiagonal) + " mm";
        }

        public string? GetBodyFirmwareVersionDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagBodyFirmwareVersion, out int value))
                return null;

            var hex = ((uint)value).ToString("X4");
            return string.Concat(hex.AsSpan(0, hex.Length - 3), ".", hex.AsSpan(hex.Length - 3));
        }

        public string? GetLensTypeDescription()
        {
            var str = Directory.GetString(OlympusEquipmentMakernoteDirectory.TagLensType);

            if (str is null)
                return null;

            var values = str.Split(' ');

            if (values.Length < 6)
                return null;


            return int.TryParse(values[0], out int num1) &&
                   int.TryParse(values[2], out int num2) &&
                   int.TryParse(values[3], out int num3) &&
                   _olympusLensTypes.TryGetValue($"{num1:X} {num2:X2} {num3:X2}", out string? lensType)
                       ? lensType
                       : null;
        }

        public string? GetLensFirmwareVersionDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagLensFirmwareVersion, out int value))
                return null;

            var hexstring = ((uint)value).ToString("X4");
            return hexstring.Insert(hexstring.Length - 3, ".");
        }

        public string? GetMaxApertureAtMinFocalDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagMaxApertureAtMinFocal, out int value))
                return null;

            return CalcMaxAperture((ushort)value).ToString("0.#");
        }

        public string? GetMaxApertureAtMaxFocalDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagMaxApertureAtMaxFocal, out int value))
                return null;

            return CalcMaxAperture((ushort)value).ToString("0.#");
        }

        public string? GetMaxApertureDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagMaxAperture, out int value))
                return null;

            return CalcMaxAperture((ushort)value).ToString("0.#");
        }

        private static double CalcMaxAperture(ushort value)
        {
            return Math.Pow(Math.Sqrt(2.00), value / 256.0);
        }

        public string? GetLensPropertiesDescription()
        {
            if (!Directory.TryGetInt(OlympusEquipmentMakernoteDirectory.TagLensProperties, out int value))
                return null;

            return $"0x{value:X4}";
        }

        public string? GetExtenderDescription()
        {
            var str = Directory.GetString(OlympusEquipmentMakernoteDirectory.TagExtender);

            if (str is null)
                return null;

            var values = str.Split(' ');

            if (values.Length < 6)
                return null;


            return int.TryParse(values[0], out int num1) &&
                   int.TryParse(values[2], out int num2) &&
                   _olympusExtenderTypes.TryGetValue($"{num1:X} {num2:X2}", out string? extenderType)
                       ? extenderType
                       : null;
        }

        public string? GetFlashTypeDescription()
        {
            return GetIndexedDescription(OlympusEquipmentMakernoteDirectory.TagFlashType,
                "None", null, "Simple E-System", "E-System");
        }

        public string? GetFlashModelDescription()
        {
            return GetIndexedDescription(OlympusEquipmentMakernoteDirectory.TagFlashModel,
                "None", "FL-20", "FL-50", "RF-11", "TF-22", "FL-36", "FL-50R", "FL-36R");
        }

        private static readonly Dictionary<string, string> _olympusLensTypes = new()
        {
            { "0 00 00", "None" },

            { "0 01 00", "Olympus Zuiko Digital ED 50mm F2.0 Macro" },
            { "0 01 01", "Olympus Zuiko Digital 40-150mm F3.5-4.5" },
            { "0 01 10", "Olympus M.Zuiko Digital ED 14-42mm F3.5-5.6" },
            { "0 02 00", "Olympus Zuiko Digital ED 150mm F2.0" },
            { "0 02 10", "Olympus M.Zuiko Digital 17mm F2.8 Pancake" },
            { "0 03 00", "Olympus Zuiko Digital ED 300mm F2.8" },
            { "0 03 10", "Olympus M.Zuiko Digital ED 14-150mm F4.0-5.6 [II]" },
            { "0 04 10", "Olympus M.Zuiko Digital ED 9-18mm F4.0-5.6" },
            { "0 05 00", "Olympus Zuiko Digital 14-54mm F2.8-3.5" },
            { "0 05 01", "Olympus Zuiko Digital Pro ED 90-250mm F2.8" },
            { "0 05 10", "Olympus M.Zuiko Digital ED 14-42mm F3.5-5.6 L" },
            { "0 06 00", "Olympus Zuiko Digital ED 50-200mm F2.8-3.5" },
            { "0 06 01", "Olympus Zuiko Digital ED 8mm F3.5 Fisheye" },
            { "0 06 10", "Olympus M.Zuiko Digital ED 40-150mm F4.0-5.6" },
            { "0 07 00", "Olympus Zuiko Digital 11-22mm F2.8-3.5" },
            { "0 07 01", "Olympus Zuiko Digital 18-180mm F3.5-6.3" },
            { "0 07 10", "Olympus M.Zuiko Digital ED 12mm F2.0" },
            { "0 08 01", "Olympus Zuiko Digital 70-300mm F4.0-5.6" },
            { "0 08 10", "Olympus M.Zuiko Digital ED 75-300mm F4.8-6.7" },
            { "0 09 10", "Olympus M.Zuiko Digital 14-42mm F3.5-5.6 II" },
            { "0 10 01", "Kenko Tokina Reflex 300mm F6.3 MF Macro" },
            { "0 10 10", "Olympus M.Zuiko Digital ED 12-50mm F3.5-6.3 EZ" },
            { "0 11 10", "Olympus M.Zuiko Digital 45mm F1.8" },
            { "0 12 10", "Olympus M.Zuiko Digital ED 60mm F2.8 Macro" },
            { "0 13 10", "Olympus M.Zuiko Digital 14-42mm F3.5-5.6 II R" },
            { "0 14 10", "Olympus M.Zuiko Digital ED 40-150mm F4.0-5.6 R" },

            { "0 15 00", "Olympus Zuiko Digital ED 7-14mm F4.0" },
            { "0 15 10", "Olympus M.Zuiko Digital ED 75mm F1.8" },
            { "0 16 10", "Olympus M.Zuiko Digital 17mm F1.8" },
            { "0 17 00", "Olympus Zuiko Digital Pro ED 35-100mm F2.0" },
            { "0 18 00", "Olympus Zuiko Digital 14-45mm F3.5-5.6" },
            { "0 18 10", "Olympus M.Zuiko Digital ED 75-300mm F4.8-6.7 II" },
            { "0 19 10", "Olympus M.Zuiko Digital ED 12-40mm F2.8 Pro" },
            { "0 20 00", "Olympus Zuiko Digital 35mm F3.5 Macro" },
            { "0 20 10", "Olympus M.Zuiko Digital ED 40-150mm F2.8 Pro" },
            { "0 21 10", "Olympus M.Zuiko Digital ED 14-42mm F3.5-5.6 EZ" },
            { "0 22 00", "Olympus Zuiko Digital 17.5-45mm F3.5-5.6" },
            { "0 22 10", "Olympus M.Zuiko Digital 25mm F1.8" },
            { "0 23 00", "Olympus Zuiko Digital ED 14-42mm F3.5-5.6" },
            { "0 23 10", "Olympus M.Zuiko Digital ED 7-14mm F2.8 Pro" },
            { "0 24 00", "Olympus Zuiko Digital ED 40-150mm F4.0-5.6" },
            { "0 24 10", "Olympus M.Zuiko Digital ED 300mm F4.0 IS Pro" },
            { "0 25 10", "Olympus M.Zuiko Digital ED 8mm F1.8 Fisheye Pro" },
            { "0 30 00", "Olympus Zuiko Digital ED 50-200mm F2.8-3.5 SWD" },
            { "0 31 00", "Olympus Zuiko Digital ED 12-60mm F2.8-4.0 SWD" },
            { "0 32 00", "Olympus Zuiko Digital ED 14-35mm F2.0 SWD" },
            { "0 33 00", "Olympus Zuiko Digital 25mm F2.8" },
            { "0 34 00", "Olympus Zuiko Digital ED 9-18mm F4.0-5.6" },
            { "0 35 00", "Olympus Zuiko Digital 14-54mm F2.8-3.5 II" },

            { "1 01 00", "Sigma 18-50mm F3.5-5.6 DC" },
            { "1 01 10", "Sigma 30mm F2.8 EX DN" },
            { "1 02 00", "Sigma 55-200mm F4.0-5.6 DC" },
            { "1 02 10", "Sigma 19mm F2.8 EX DN" },
            { "1 03 00", "Sigma 18-125mm F3.5-5.6 DC" },
            { "1 03 10", "Sigma 30mm F2.8 DN | A" },
            { "1 04 00", "Sigma 18-125mm F3.5-5.6 DC" },
            { "1 04 10", "Sigma 19mm F2.8 DN | A" },
            { "1 05 00", "Sigma 30mm F1.4 EX DC HSM" },
            { "1 05 10", "Sigma 60mm F2.8 DN | A" },
            { "1 06 00", "Sigma APO 50-500mm F4.0-6.3 EX DG HSM" },
            { "1 07 00", "Sigma Macro 105mm F2.8 EX DG" },
            { "1 08 00", "Sigma APO Macro 150mm F2.8 EX DG HSM" },
            { "1 09 00", "Sigma 18-50mm F2.8 EX DC Macro" },
            { "1 10 00", "Sigma 24mm F1.8 EX DG Aspherical Macro" },
            { "1 11 00", "Sigma APO 135-400mm F4.5-5.6 DG" },
            { "1 12 00", "Sigma APO 300-800mm F5.6 EX DG HSM" },
            { "1 13 00", "Sigma 30mm F1.4 EX DC HSM" },
            { "1 14 00", "Sigma APO 50-500mm F4.0-6.3 EX DG HSM" },
            { "1 15 00", "Sigma 10-20mm F4.0-5.6 EX DC HSM" },
            { "1 16 00", "Sigma APO 70-200mm F2.8 II EX DG Macro HSM" },
            { "1 17 00", "Sigma 50mm F1.4 EX DG HSM" },

            { "2 01 00", "Leica D Vario Elmarit 14-50mm F2.8-3.5 Asph." },
            { "2 01 10", "Lumix G Vario 14-45mm F3.5-5.6 Asph. Mega OIS" },
            { "2 02 00", "Leica D Summilux 25mm F1.4 Asph." },
            { "2 02 10", "Lumix G Vario 45-200mm F4.0-5.6 Mega OIS" },
            { "2 03 00", "Leica D Vario Elmar 14-50mm F3.8-5.6 Asph. Mega OIS" },
            { "2 03 01", "Leica D Vario Elmar 14-50mm F3.8-5.6 Asph." },
            { "2 03 10", "Lumix G Vario HD 14-140mm F4.0-5.8 Asph. Mega OIS" },
            { "2 04 00", "Leica D Vario Elmar 14-150mm F3.5-5.6" },
            { "2 04 10", "Lumix G Vario 7-14mm F4.0 Asph." },
            { "2 05 10", "Lumix G 20mm F1.7 Asph." },
            { "2 06 10", "Leica DG Macro-Elmarit 45mm F2.8 Asph. Mega OIS" },
            { "2 07 10", "Lumix G Vario 14-42mm F3.5-5.6 Asph. Mega OIS" },
            { "2 08 10", "Lumix G Fisheye 8mm F3.5" },
            { "2 09 10", "Lumix G Vario 100-300mm F4.0-5.6 Mega OIS" },
            { "2 10 10", "Lumix G 14mm F2.5 Asph." },
            { "2 11 10", "Lumix G 12.5mm F12 3D" },
            { "2 12 10", "Leica DG Summilux 25mm F1.4 Asph." },
            { "2 13 10", "Lumix G X Vario PZ 45-175mm F4.0-5.6 Asph. Power OIS" },
            { "2 14 10", "Lumix G X Vario PZ 14-42mm F3.5-5.6 Asph. Power OIS" },
            { "2 15 10", "Lumix G X Vario 12-35mm F2.8 Asph. Power OIS" },
            { "2 16 10", "Lumix G Vario 45-150mm F4.0-5.6 Asph. Mega OIS" },
            { "2 17 10", "Lumix G X Vario 35-100mm F2.8 Power OIS" },
            { "2 18 10", "Lumix G Vario 14-42mm F3.5-5.6 II Asph. Mega OIS" },
            { "2 19 10", "Lumix G Vario 14-140mm F3.5-5.6 Asph. Power OIS" },
            { "2 20 10", "Lumix G Vario 12-32mm F3.5-5.6 Asph. Mega OIS" },
            { "2 21 10", "Leica DG Nocticron 42.5mm F1.2 Asph. Power OIS" },
            { "2 22 10", "Leica DG Summilux 15mm F1.7 Asph." },

            { "2 24 10", "Lumix G Macro 30mm F2.8 Asph. Mega OIS" },
            { "2 25 10", "Lumix G 42.5mm F1.7 Asph. Power OIS" },
            { "3 01 00", "Leica D Vario Elmarit 14-50mm F2.8-3.5 Asph." },
            { "3 02 00", "Leica D Summilux 25mm F1.4 Asph." },

            { "5 01 10", "Tamron 14-150mm F3.5-5.8 Di III" }
        };

        private static readonly Dictionary<string, string> _olympusExtenderTypes = new()
        {
            { "0 00", "None" },
            { "0 04", "Olympus Zuiko Digital EC-14 1.4x Teleconverter" },
            { "0 08", "Olympus EX-25 Extension Tube" },
            { "0 10", "Olympus Zuiko Digital EC-20 2.0x Teleconverter" }
        };
    }
}
