namespace MetadataExtractor.Formats.Photoshop
{
    public class Subpath
    {
        private readonly List<Knot> _knots = new();

        public Subpath(string type = "")
        {
            Type = type;
        }

        public void Add(Knot knot)
        {
            _knots.Add(knot);
        }

        public int KnotCount
        {
            get { return _knots.Count; }
        }

        public IEnumerable<Knot> Knots
        {
            get { return _knots; }
        }

        public string Type { get; }
    }
}
