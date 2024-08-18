namespace MetadataExtractor.Formats.Exif
{
    public sealed class GpsDescriptor(GpsDirectory directory) : TagDescriptor<GpsDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                GpsDirectory.TagVersionId => GetGpsVersionIdDescription(),
                GpsDirectory.TagAltitude => GetGpsAltitudeDescription(),
                GpsDirectory.TagAltitudeRef => GetGpsAltitudeRefDescription(),
                GpsDirectory.TagStatus => GetGpsStatusDescription(),
                GpsDirectory.TagMeasureMode => GetGpsMeasureModeDescription(),
                GpsDirectory.TagDop => GetGpsDopDescription(),
                GpsDirectory.TagSpeedRef => GetGpsSpeedRefDescription(),
                GpsDirectory.TagSpeed => GetGpsSpeedDescription(),
                GpsDirectory.TagTrackRef or GpsDirectory.TagImgDirectionRef or GpsDirectory.TagDestBearingRef => GetGpsDirectionReferenceDescription(tagType),
                GpsDirectory.TagTrack or GpsDirectory.TagImgDirection or GpsDirectory.TagDestBearing => GetGpsDirectionDescription(tagType),
                GpsDirectory.TagDestLatitude => GetGpsDestLatitudeDescription(),
                GpsDirectory.TagDestLongitude => GetGpsDestLongitudeDescription(),
                GpsDirectory.TagDestDistanceRef => GetGpsDestinationReferenceDescription(),
                GpsDirectory.TagDestDistance => GetGpsDestDistanceDescription(),
                GpsDirectory.TagTimeStamp => GetGpsTimeStampDescription(),
                GpsDirectory.TagLongitude => GetGpsLongitudeDescription(),// three rational numbers -- displayed in HH"MM"SS.ss
                GpsDirectory.TagLatitude => GetGpsLatitudeDescription(),// three rational numbers -- displayed in HH"MM"SS.ss
                GpsDirectory.TagProcessingMethod => GetGpsProcessingMethodDescription(),
                GpsDirectory.TagAreaInformation => GetGpsAreaInformationDescription(),
                GpsDirectory.TagDifferential => GetGpsDifferentialDescription(),
                GpsDirectory.TagHPositioningError => GetGpsHPositioningErrorDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        private string? GetGpsVersionIdDescription()
        {
            return GetVersionBytesDescription(GpsDirectory.TagVersionId, 1);
        }

        public string? GetGpsLatitudeDescription()
        {
            var location = Directory.GetGeoLocation();
            return location is null ? null : GeoLocation.DecimalToDegreesMinutesSecondsString(location.Latitude);
        }

        public string? GetGpsLongitudeDescription()
        {
            var location = Directory.GetGeoLocation();
            return location is null ? null : GeoLocation.DecimalToDegreesMinutesSecondsString(location.Longitude);
        }

        public string? GetGpsTimeStampDescription()
        {
            var timeComponents = Directory.GetArray<Rational[]>(GpsDirectory.TagTimeStamp);
            return timeComponents is null
                ? null
                : $"{timeComponents[0].ToInt32():D2}:{timeComponents[1].ToInt32():D2}:{timeComponents[2].ToDouble():00.000} UTC";
        }

        public string? GetGpsDestLatitudeDescription()
        {
            return GetGeoLocationDimension(GpsDirectory.TagDestLatitude, GpsDirectory.TagDestLatitudeRef, "S");
        }

        public string? GetGpsDestLongitudeDescription()
        {
            return GetGeoLocationDimension(GpsDirectory.TagDestLongitude, GpsDirectory.TagDestLongitudeRef, "W");
        }

        private string? GetGeoLocationDimension(int tagValue, int tagRef, string positiveRef)
        {
            var values = Directory.GetArray<Rational[]>(tagValue);
            var @ref = Directory.GetString(tagRef);

            if (values is null || values.Length != 3 || @ref is null)
                return null;

            var dec = GeoLocation.DegreesMinutesSecondsToDecimal(
                values[0], values[1], values[2], @ref.Equals(positiveRef, StringComparison.OrdinalIgnoreCase));

            return dec == null ? null : GeoLocation.DecimalToDegreesMinutesSecondsString((double)dec);
        }

        public string? GetGpsDestinationReferenceDescription()
        {
            var value = Directory.GetString(GpsDirectory.TagDestDistanceRef);
            if (value is null)
                return null;

            return (value.Trim().ToUpper()) switch
            {
                "K" => "kilometers",
                "M" => "miles",
                "N" => "knots",

                _ => "Unknown (" + value.Trim() + ")",
            };
        }

        public string? GetGpsDestDistanceDescription()
        {
            if (!Directory.TryGetRational(GpsDirectory.TagDestDistance, out Rational value))
                return null;

            var unit = GetGpsDestinationReferenceDescription();
            return string.Format("{0} {1}", value.ToDouble().ToString("0.##"), unit is null ? "unit" : unit.ToLower());
        }

        public string? GetGpsDirectionDescription(int tagType)
        {
            if (!Directory.TryGetRational(tagType, out Rational angle))
                return null;

            return angle.ToDouble().ToString("0.##") + " degrees";
        }

        public string? GetGpsDirectionReferenceDescription(int tagType)
        {
            var value = Directory.GetString(tagType);
            if (value is null)
                return null;

            return (value.Trim().ToUpper()) switch
            {
                "T" => "True direction",
                "M" => "Magnetic direction",

                _ => "Unknown (" + value.Trim() + ")",
            };
        }

        public string? GetGpsDopDescription()
        {
            if (!Directory.TryGetRational(GpsDirectory.TagDop, out Rational value))
                return null;
            return $"{value.ToDouble():0.##}";
        }

        public string? GetGpsSpeedRefDescription()
        {
            var value = Directory.GetString(GpsDirectory.TagSpeedRef);
            if (value is null)
                return null;

            return (value.Trim().ToUpper()) switch
            {
                "K" => "km/h",
                "M" => "mph",
                "N" => "knots",

                _ => "Unknown (" + value.Trim() + ")",
            };
        }

        public string? GetGpsSpeedDescription()
        {
            if (!Directory.TryGetRational(GpsDirectory.TagSpeed, out Rational value))
                return null;

            var unit = GetGpsSpeedRefDescription();

            return string.Format("{0} {1}", value.ToDouble().ToString("0.##"), unit is null ? "unit" : unit.ToLower());
        }

        public string? GetGpsMeasureModeDescription()
        {
            var value = Directory.GetString(GpsDirectory.TagMeasureMode);
            if (value is null)
                return null;

            return (value.Trim()) switch
            {
                "2" => "2-dimensional measurement",
                "3" => "3-dimensional measurement",
                _ => "Unknown (" + value.Trim() + ")",
            };
        }


        public string? GetGpsStatusDescription()
        {
            var value = Directory.GetString(GpsDirectory.TagStatus);
            if (value is null)
                return null;

            return (value.Trim().ToUpper()) switch
            {
                "A" => "Active (Measurement in progress)",
                "V" => "Void (Measurement Interoperability)",

                _ => "Unknown (" + value.Trim() + ")",
            };
        }

        public string? GetGpsAltitudeRefDescription()
        {
            return GetIndexedDescription(GpsDirectory.TagAltitudeRef,
                "Sea level", "Below sea level");
        }

        public string? GetGpsAltitudeDescription()
        {
            if (!Directory.TryGetRational(GpsDirectory.TagAltitude, out Rational value))
                return null;
            return $"{value.ToDouble():0.##} metres";
        }

        public string? GetGpsProcessingMethodDescription()
        {
            return GetEncodedTextDescription(GpsDirectory.TagProcessingMethod);
        }

        public string? GetGpsAreaInformationDescription()
        {
            return GetEncodedTextDescription(GpsDirectory.TagAreaInformation);
        }

        public string? GetGpsDifferentialDescription()
        {
            return GetIndexedDescription(GpsDirectory.TagDifferential,
                "No Correction", "Differential Corrected");
        }

        public string? GetGpsHPositioningErrorDescription()
        {
            if (!Directory.TryGetRational(GpsDirectory.TagHPositioningError, out Rational value))
                return null;
            return $"{value.ToDouble():0.##} metres";
        }

        public string? GetDegreesMinutesSecondsDescription()
        {
            var location = Directory.GetGeoLocation();
            return location?.ToDmsString();
        }
    }
}
