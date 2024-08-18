namespace MetadataExtractor.Formats.Exif.Makernotes
{
    public class DjiMakernoteDirectory : Directory
    {
        public const int TagMake = 0x0001;
        public const int TagSpeedX = 0x0003;
        public const int TagSpeedY = 0x0004;
        public const int TagSpeedZ = 0x0005;
        public const int TagAircraftPitch = 0x0006;
        public const int TagAircraftYaw = 0x0007;
        public const int TagAircraftRoll = 0x0008;
        public const int TagCameraPitch = 0x0009;
        public const int TagCameraYaw = 0x000a;
        public const int TagCameraRoll = 0x000b;

        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagMake, "Make" },
            { TagSpeedX, "Aircraft X Speed" },
            { TagSpeedY, "Aircraft Y Speed" },
            { TagSpeedZ, "Aircraft Z Speed" },
            { TagAircraftPitch, "Aircraft Pitch" },
            { TagAircraftYaw, "Aircraft Yaw" },
            { TagAircraftRoll, "Aircraft Roll" },
            { TagCameraPitch, "Camera Pitch" },
            { TagCameraYaw, "Camera Yaw" },
            { TagCameraRoll, "Camera Roll" }
        };

        public DjiMakernoteDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new DjiMakernoteDescriptor(this));
        }

        public override string Name => "DJI Makernote";

        public float? GetAircraftSpeedX() => this.TryGetFloat(TagSpeedX, out var value) ? value : null;

        public float? GetAircraftSpeedY() => this.TryGetFloat(TagSpeedY, out var value) ? value : null;

        public float? GetAircraftSpeedZ() => this.TryGetFloat(TagSpeedZ, out var value) ? value : null;

        public float? GetAircraftPitch() => this.TryGetFloat(TagAircraftPitch, out var value) ? value : null;

        public float? GetAircraftYaw() => this.TryGetFloat(TagAircraftYaw, out var value) ? value : null;

        public float? GetAircraftRoll() => this.TryGetFloat(TagAircraftRoll, out var value) ? value : null;

        public float? GetCameraPitch() => this.TryGetFloat(TagCameraPitch, out var value) ? value : null;

        public float? GetCameraYaw() => this.TryGetFloat(TagCameraYaw, out var value) ? value : null;

        public float? GetCameraRoll() => this.TryGetFloat(TagCameraRoll, out var value) ? value : null;
    }
}
