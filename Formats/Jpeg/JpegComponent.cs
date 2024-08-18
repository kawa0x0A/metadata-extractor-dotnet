namespace MetadataExtractor.Formats.Jpeg
{
    [Serializable]
    public sealed class JpegComponent
    {
        private readonly byte _samplingFactorByte;

        public JpegComponent(byte componentId, byte samplingFactorByte, byte quantizationTableNumber)
        {
            Id = componentId;
            _samplingFactorByte = samplingFactorByte;
            QuantizationTableNumber = quantizationTableNumber;
        }

        public byte Id { get; }

        public byte QuantizationTableNumber { get; }

        public string Name
        {
            get
            {
                return Id switch
                {
                    1 => "Y",
                    2 => "Cb",
                    3 => "Cr",
                    4 => "I",
                    5 => "Q",
                    _ => $"Unknown ({Id})",
                };
            }
        }

        public int HorizontalSamplingFactor => (_samplingFactorByte >> 4) & 0x0F;

        public int VerticalSamplingFactor => _samplingFactorByte & 0x0F;

        public override string ToString()
            => $"Quantization table {QuantizationTableNumber}, Sampling factors {HorizontalSamplingFactor} horiz/{VerticalSamplingFactor} vert";
    }
}
