using System.Text;

namespace MetadataExtractor
{
    public sealed class Face(int x, int y, int width, int height, string? name = "", Age? age = null)
    {
        public int X { get; } = x;

        public int Y { get; } = y;

        public int Width { get; } = width;

        public int Height { get; } = height;

        public string? Name { get; } = name;

        public Age? Age { get; } = age;

        private bool Equals(Face other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height && string.Equals(Name, other.Name) && Equals(Age, other.Age);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is Face face && Equals(face);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Age?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append("x: ").Append(X);
            result.Append(" y: ").Append(Y);
            result.Append(" width: ").Append(Width);
            result.Append(" height: ").Append(Height);

            if (Name is not null)
                result.Append(" name: ").Append(Name);

            if (Age is not null)
                result.Append(" age: ").Append(Age.ToFriendlyString());

            return result.ToString();
        }
    }
}
