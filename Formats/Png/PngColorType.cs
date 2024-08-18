namespace MetadataExtractor.Formats.Png
{
    [Serializable]
    public sealed class PngColorType
    {
        public static readonly PngColorType Greyscale = new(0, "Greyscale", 1, 2, 4, 8, 16);

        public static readonly PngColorType TrueColor = new(2, "True Color", 8, 16);

        public static readonly PngColorType IndexedColor = new(3, "Indexed Color", 1, 2, 4, 8);

        public static readonly PngColorType GreyscaleWithAlpha = new(4, "Greyscale with Alpha", 8, 16);

        public static readonly PngColorType TrueColorWithAlpha = new(6, "True Color with Alpha", 8, 16);

        public static PngColorType FromNumericValue(int numericValue)
        {
            var colorTypes = new[] { Greyscale, TrueColor, IndexedColor, GreyscaleWithAlpha, TrueColorWithAlpha };
            return colorTypes.FirstOrDefault(colorType => colorType.NumericValue == numericValue)
                ?? new PngColorType(numericValue, $"Unknown ({numericValue})");
        }

        public int NumericValue { get; }

        public string Description { get; }

        public int[] AllowedBitDepths { get; }

        private PngColorType(int numericValue, string description, params int[] allowedBitDepths)
        {
            NumericValue = numericValue;
            Description = description;
            AllowedBitDepths = allowedBitDepths;
        }
    }
}
