using System.Collections;

namespace MetadataExtractor.Util
{
    public sealed class ByteTrie<T> : IEnumerable<T>
    {
        private sealed class ByteTrieNode
        {
            public readonly IDictionary<byte, ByteTrieNode> Children = new Dictionary<byte, ByteTrieNode>();

            public T Value { get; private set; } = default!;
            public bool HasValue { get; private set; }

            public void SetValue(T value)
            {
                Value = value;
                HasValue = true;
            }
        }

        private readonly ByteTrieNode _root = new();

        public int MaxDepth { get; private set; }

        public ByteTrie(T defaultValue) => _root.SetValue(defaultValue);

        public T Find(byte[] bytes) => Find(bytes, 0, bytes.Length);

        public T Find(byte[] bytes, int offset, int count)
        {
            var maxIndex = offset + count;
            if (maxIndex > bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset and length are not in bounds for byte array.");

            var node = _root;
            var value = node.Value;
            for (var i = offset; i < maxIndex; i++)
            {
                var b = bytes[i];
                if (!node.Children.TryGetValue(b, out node))
                    break;
                if (node.HasValue)
                    value = node.Value;
            }

            return value;
        }

        public void Add(T value, params byte[][] parts)
        {
            var depth = 0;
            var node = _root;
            foreach (var part in parts)
            {
                foreach (var b in part)
                {
                    if (!node.Children.TryGetValue(b, out ByteTrieNode? child))
                    {
                        child = new ByteTrieNode();
                        node.Children[b] = child;
                    }
                    node = child;
                    depth++;
                }
            }
            node.SetValue(value);
            MaxDepth = Math.Max(MaxDepth, depth);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }
}
