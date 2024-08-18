namespace MetadataExtractor.Formats.Photoshop
{
    public class Knot
    {
        private readonly double[] _points = new double[6];

        public Knot(string type)
        {
            Type = type;
        }

        public double this[int index]
        {
            get => _points[index];
            set => _points[index] = value;
        }

        public string Type { get; }
    }
}
