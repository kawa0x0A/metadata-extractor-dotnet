namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public sealed class FujifilmMakernoteDescriptor(FujifilmMakernoteDirectory directory) : TagDescriptor<FujifilmMakernoteDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                FujifilmMakernoteDirectory.TagMakernoteVersion => GetMakernoteVersionDescription(),
                FujifilmMakernoteDirectory.TagSharpness => GetSharpnessDescription(),
                FujifilmMakernoteDirectory.TagWhiteBalance => GetWhiteBalanceDescription(),
                FujifilmMakernoteDirectory.TagColorSaturation => GetColorSaturationDescription(),
                FujifilmMakernoteDirectory.TagTone => GetToneDescription(),
                FujifilmMakernoteDirectory.TagContrast => GetContrastDescription(),
                FujifilmMakernoteDirectory.TagNoiseReduction => GetNoiseReductionDescription(),
                FujifilmMakernoteDirectory.TagHighIsoNoiseReduction => GetHighIsoNoiseReductionDescription(),
                FujifilmMakernoteDirectory.TagFlashMode => GetFlashModeDescription(),
                FujifilmMakernoteDirectory.TagFlashEv => GetFlashExposureValueDescription(),
                FujifilmMakernoteDirectory.TagMacro => GetMacroDescription(),
                FujifilmMakernoteDirectory.TagFocusMode => GetFocusModeDescription(),
                FujifilmMakernoteDirectory.TagSlowSync => GetSlowSyncDescription(),
                FujifilmMakernoteDirectory.TagPictureMode => GetPictureModeDescription(),
                FujifilmMakernoteDirectory.TagExrAuto => GetExrAutoDescription(),
                FujifilmMakernoteDirectory.TagExrMode => GetExrModeDescription(),
                FujifilmMakernoteDirectory.TagAutoBracketing => GetAutoBracketingDescription(),
                FujifilmMakernoteDirectory.TagFinePixColor => GetFinePixColorDescription(),
                FujifilmMakernoteDirectory.TagBlurWarning => GetBlurWarningDescription(),
                FujifilmMakernoteDirectory.TagFocusWarning => GetFocusWarningDescription(),
                FujifilmMakernoteDirectory.TagAutoExposureWarning => GetAutoExposureWarningDescription(),
                FujifilmMakernoteDirectory.TagDynamicRange => GetDynamicRangeDescription(),
                FujifilmMakernoteDirectory.TagFilmMode => GetFilmModeDescription(),
                FujifilmMakernoteDirectory.TagDynamicRangeSetting => GetDynamicRangeSettingDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetMakernoteVersionDescription()
        {
            return GetVersionBytesDescription(FujifilmMakernoteDirectory.TagMakernoteVersion, 2);
        }

        public string? GetSharpnessDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagSharpness, out int value))
                return null;

            return value switch
            {
                1 => "Softest",
                2 => "Soft",
                3 => "Normal",
                4 => "Hard",
                5 => "Hardest",
                0x82 => "Medium Soft",
                0x84 => "Medium Hard",
                0x8000 => "Film Simulation",
                0xFFFF => "N/A",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetWhiteBalanceDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagWhiteBalance, out int value))
                return null;

            return value switch
            {
                0x000 => "Auto",
                0x100 => "Daylight",
                0x200 => "Cloudy",
                0x300 => "Daylight Fluorescent",
                0x301 => "Day White Fluorescent",
                0x302 => "White Fluorescent",
                0x303 => "Warm White Fluorescent",
                0x304 => "Living Room Warm White Fluorescent",
                0x400 => "Incandescence",
                0x500 => "Flash",
                0xf00 => "Custom White Balance",
                0xf01 => "Custom White Balance 2",
                0xf02 => "Custom White Balance 3",
                0xf03 => "Custom White Balance 4",
                0xf04 => "Custom White Balance 5",
                0xff0 => "Kelvin",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetColorSaturationDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagColorSaturation, out int value))
                return null;

            return value switch
            {
                0x000 => "Normal",
                0x080 => "Medium High",
                0x100 => "High",
                0x180 => "Medium Low",
                0x200 => "Low",
                0x300 => "None (B&W)",
                0x301 => "B&W Green Filter",
                0x302 => "B&W Yellow Filter",
                0x303 => "B&W Blue Filter",
                0x304 => "B&W Sepia",
                0x8000 => "Film Simulation",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetToneDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagTone, out int value))
                return null;

            return value switch
            {
                0x000 => "Normal",
                0x080 => "Medium High",
                0x100 => "High",
                0x180 => "Medium Low",
                0x200 => "Low",
                0x300 => "None (B&W)",
                0x8000 => "Film Simulation",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetContrastDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagContrast, out int value))
                return null;

            return value switch
            {
                0x000 => "Normal",
                0x100 => "High",
                0x300 => "Low",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetNoiseReductionDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagNoiseReduction, out int value))
                return null;
            return value switch
            {
                0x040 => "Low",
                0x080 => "Normal",
                0x100 => "N/A",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetHighIsoNoiseReductionDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagHighIsoNoiseReduction, out int value))
                return null;

            return value switch
            {
                0x000 => "Normal",
                0x100 => "Strong",
                0x200 => "Weak",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetFlashModeDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagFlashMode,
                "Auto", "On", "Off", "Red-eye Reduction", "External");
        }

        public string? GetFlashExposureValueDescription()
        {
            if (!Directory.TryGetRational(FujifilmMakernoteDirectory.TagFlashEv, out Rational value))
                return null;
            return value.ToSimpleString(allowDecimal: false) + " EV (Apex)";
        }

        public string? GetMacroDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagMacro,
                "Off", "On");
        }

        public string? GetFocusModeDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagFocusMode,
                "Auto Focus", "Manual Focus");
        }

        public string? GetSlowSyncDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagSlowSync,
                "Off", "On");
        }

        public string? GetPictureModeDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagPictureMode, out int value))
                return null;

            return value switch
            {
                0x000 => "Auto",
                0x001 => "Portrait scene",
                0x002 => "Landscape scene",
                0x003 => "Macro",
                0x004 => "Sports scene",
                0x005 => "Night scene",
                0x006 => "Program AE",
                0x007 => "Natural Light",
                0x008 => "Anti-blur",
                0x009 => "Beach & Snow",
                0x00a => "Sunset",
                0x00b => "Museum",
                0x00c => "Party",
                0x00d => "Flower",
                0x00e => "Text",
                0x00f => "Natural Light & Flash",
                0x010 => "Beach",
                0x011 => "Snow",
                0x012 => "Fireworks",
                0x013 => "Underwater",
                0x014 => "Portrait with Skin Correction",
                // skip 0x015
                0x016 => "Panorama",
                0x017 => "Night (Tripod)",
                0x018 => "Pro Low-light",
                0x019 => "Pro Focus",
                // skip 0x01a
                0x01b => "Dog Face Detection",
                0x01c => "Cat Face Detection",
                0x100 => "Aperture priority AE",
                0x200 => "Shutter priority AE",
                0x300 => "Manual exposure",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetExrAutoDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagExrAuto,
                "Auto", "Manual");
        }

        public string? GetExrModeDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagExrMode, out int value))
                return null;

            return value switch
            {
                0x100 => "HR (High Resolution)",
                0x200 => "SN (Signal to Noise Priority)",
                0x300 => "DR (Dynamic Range Priority)",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetAutoBracketingDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagAutoBracketing,
                "Off", "On", "No Flash & Flash");
        }

        public string? GetFinePixColorDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagFinePixColor, out int value))
                return null;
            return value switch
            {
                0x00 => "Standard",
                0x10 => "Chrome",
                0x30 => "B&W",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetBlurWarningDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagBlurWarning,
                "No Blur Warning", "Blur warning");
        }

        public string? GetFocusWarningDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagFocusWarning,
                "Good Focus", "Out Of Focus");
        }

        public string? GetAutoExposureWarningDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagAutoExposureWarning,
                "AE Good", "Over Exposed");
        }

        public string? GetDynamicRangeDescription()
        {
            return GetIndexedDescription(FujifilmMakernoteDirectory.TagDynamicRange,
                1,
                "Standard", null, "Wide");
        }

        public string? GetFilmModeDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagFilmMode, out int value))
                return null;

            return value switch
            {
                0x000 => "F0/Standard (Provia) ",
                0x100 => "F1/Studio Portrait",
                0x110 => "F1a/Studio Portrait Enhanced Saturation",
                0x120 => "F1b/Studio Portrait Smooth Skin Tone (Astia)",
                0x130 => "F1c/Studio Portrait Increased Sharpness",
                0x200 => "F2/Fujichrome (Velvia)",
                0x300 => "F3/Studio Portrait Ex",
                0x400 => "F4/Velvia",
                0x500 => "Pro Neg. Std",
                0x501 => "Pro Neg. Hi",
                0x600 => "Classic Chrome",
                0x700 => "Eterna",
                0x800 => "Classic Negative",
                0x900 => "Bleach Bypass",
                0xa00 => "Nostalgic Neg",
                _ => "Unknown (" + value + ")",
            };
        }

        public string? GetDynamicRangeSettingDescription()
        {
            if (!Directory.TryGetInt(FujifilmMakernoteDirectory.TagDynamicRangeSetting, out int value))
                return null;

            return value switch
            {
                0x000 => "Auto (100-400%)",
                0x001 => "Manual",
                0x100 => "Standard (100%)",
                0x200 => "Wide 1 (230%)",
                0x201 => "Wide 2 (400%)",
                0x8000 => "Film Simulation",
                _ => "Unknown (" + value + ")",
            };
        }
    }
}
