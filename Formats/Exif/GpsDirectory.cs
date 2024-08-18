namespace MetadataExtractor.Formats.Exif
{
    public sealed class GpsDirectory : ExifDirectoryBase
    {
        public const int TagVersionId = 0x0000;

        public const int TagLatitudeRef = 0x0001;

        public const int TagLatitude = 0x0002;

        public const int TagLongitudeRef = 0x0003;

        public const int TagLongitude = 0x0004;

        public const int TagAltitudeRef = 0x0005;

        public const int TagAltitude = 0x0006;

        public const int TagTimeStamp = 0x0007;

        public const int TagSatellites = 0x0008;

        public const int TagStatus = 0x0009;

        public const int TagMeasureMode = 0x000A;

        public const int TagDop = 0x000B;

        public const int TagSpeedRef = 0x000C;

        public const int TagSpeed = 0x000D;

        public const int TagTrackRef = 0x000E;

        public const int TagTrack = 0x000F;

        public const int TagImgDirectionRef = 0x0010;

        public const int TagImgDirection = 0x0011;

        public const int TagMapDatum = 0x0012;

        public const int TagDestLatitudeRef = 0x0013;

        public const int TagDestLatitude = 0x0014;

        public const int TagDestLongitudeRef = 0x0015;

        public const int TagDestLongitude = 0x0016;

        public const int TagDestBearingRef = 0x0017;

        public const int TagDestBearing = 0x0018;

        public const int TagDestDistanceRef = 0x0019;

        public const int TagDestDistance = 0x001A;

        public const int TagProcessingMethod = 0x001B;

        public const int TagAreaInformation = 0x001C;

        public const int TagDateStamp = 0x001D;

        public const int TagDifferential = 0x001E;

        public const int TagHPositioningError = 0x001F;

        private static readonly Dictionary<int, string> _tagNameMap = new();

        static GpsDirectory()
        {
            AddExifTagNames(_tagNameMap);

            _tagNameMap[TagVersionId] = "GPS Version ID";
            _tagNameMap[TagLatitudeRef] = "GPS Latitude Ref";
            _tagNameMap[TagLatitude] = "GPS Latitude";
            _tagNameMap[TagLongitudeRef] = "GPS Longitude Ref";
            _tagNameMap[TagLongitude] = "GPS Longitude";
            _tagNameMap[TagAltitudeRef] = "GPS Altitude Ref";
            _tagNameMap[TagAltitude] = "GPS Altitude";
            _tagNameMap[TagTimeStamp] = "GPS Time-Stamp";
            _tagNameMap[TagSatellites] = "GPS Satellites";
            _tagNameMap[TagStatus] = "GPS Status";
            _tagNameMap[TagMeasureMode] = "GPS Measure Mode";
            _tagNameMap[TagDop] = "GPS DOP";
            _tagNameMap[TagSpeedRef] = "GPS Speed Ref";
            _tagNameMap[TagSpeed] = "GPS Speed";
            _tagNameMap[TagTrackRef] = "GPS Track Ref";
            _tagNameMap[TagTrack] = "GPS Track";
            _tagNameMap[TagImgDirectionRef] = "GPS Img Direction Ref";
            _tagNameMap[TagImgDirection] = "GPS Img Direction";
            _tagNameMap[TagMapDatum] = "GPS Map Datum";
            _tagNameMap[TagDestLatitudeRef] = "GPS Dest Latitude Ref";
            _tagNameMap[TagDestLatitude] = "GPS Dest Latitude";
            _tagNameMap[TagDestLongitudeRef] = "GPS Dest Longitude Ref";
            _tagNameMap[TagDestLongitude] = "GPS Dest Longitude";
            _tagNameMap[TagDestBearingRef] = "GPS Dest Bearing Ref";
            _tagNameMap[TagDestBearing] = "GPS Dest Bearing";
            _tagNameMap[TagDestDistanceRef] = "GPS Dest Distance Ref";
            _tagNameMap[TagDestDistance] = "GPS Dest Distance";
            _tagNameMap[TagProcessingMethod] = "GPS Processing Method";
            _tagNameMap[TagAreaInformation] = "GPS Area Information";
            _tagNameMap[TagDateStamp] = "GPS Date Stamp";
            _tagNameMap[TagDifferential] = "GPS Differential";
            _tagNameMap[TagHPositioningError] = "GPS Horizontal Positioning Error";
        }

        public GpsDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new GpsDescriptor(this));
        }

        public override string Name => "GPS";

        public GeoLocation? GetGeoLocation()
        {
            var latitudes = this.GetArray<Rational[]>(TagLatitude);
            var longitudes = this.GetArray<Rational[]>(TagLongitude);
            var latitudeRef = this.GetString(TagLatitudeRef);
            var longitudeRef = this.GetString(TagLongitudeRef);

            if (latitudes is null || latitudes.Length != 3)
                return null;
            if (longitudes is null || longitudes.Length != 3)
                return null;
            if (latitudeRef is null || longitudeRef is null)
                return null;

#pragma warning disable format
            var lat = GeoLocation.DegreesMinutesSecondsToDecimal(latitudes[0],  latitudes[1],  latitudes[2],  latitudeRef.Equals("S", StringComparison.OrdinalIgnoreCase));
            var lon = GeoLocation.DegreesMinutesSecondsToDecimal(longitudes[0], longitudes[1], longitudes[2], longitudeRef.Equals("W", StringComparison.OrdinalIgnoreCase));
#pragma warning restore format

            if (lat == null || lon == null)
                return null;

            return new GeoLocation((double)lat, (double)lon);
        }

        public bool TryGetGpsDate(out DateTime date)
        {
            if (!this.TryGetDateTime(TagDateStamp, out date))
                return false;

            var timeComponents = GetArray<Rational[]>(TagTimeStamp);

            if (timeComponents is null || timeComponents.Length != 3)
                return false;

            date = date
                .AddHours(timeComponents[0].ToDouble())
                .AddMinutes(timeComponents[1].ToDouble())
                .AddSeconds(timeComponents[2].ToDouble());

            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return true;
        }
    }
}
