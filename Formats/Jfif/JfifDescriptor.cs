namespace MetadataExtractor.Formats.Jfif
{
    public sealed class JfifDescriptor(JfifDirectory directory) : TagDescriptor<JfifDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                JfifDirectory.TagResX => GetImageResXDescription(),
                JfifDirectory.TagResY => GetImageResYDescription(),
                JfifDirectory.TagVersion => GetImageVersionDescription(),
                JfifDirectory.TagUnits => GetImageResUnitsDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetImageVersionDescription()
        {
            if (!Directory.TryGetInt(JfifDirectory.TagVersion, out int value))
                return null;
            return $"{(value & 0xFF00) >> 8}.{value & 0x00FF}";
        }

        public string? GetImageResYDescription()
        {
            if (!Directory.TryGetInt(JfifDirectory.TagResY, out int value))
                return null;
            return $"{value} dot{(value == 1 ? string.Empty : "s")}";
        }

        public string? GetImageResXDescription()
        {
            if (!Directory.TryGetInt(JfifDirectory.TagResX, out int value))
                return null;
            return $"{value} dot{(value == 1 ? string.Empty : "s")}";
        }

        public string? GetImageResUnitsDescription()
        {
            if (!Directory.TryGetInt(JfifDirectory.TagUnits, out int value))
                return null;
            return value switch
            {
                0 => "none",
                1 => "inch",
                2 => "centimetre",
                _ => "unit",
            };
        }
    }
}
