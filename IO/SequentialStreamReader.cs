namespace MetadataExtractor.IO
{
    public class SequentialStreamReader : SequentialReader
    {
        private readonly Stream _stream;

        public override long Position => _stream.Position;

        public SequentialStreamReader(Stream stream, bool isMotorolaByteOrder = true) : base(isMotorolaByteOrder)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public override byte GetByte()
        {
            var value = _stream.ReadByte();

            if (value == -1)
                throw new IOException("End of data reached.");

            return unchecked((byte)value);
        }

        public override async Task<byte> GetByteAsync()
        {
            var buffer = new byte[1];

            var value = await _stream.ReadAsync(buffer.AsMemory(0, 1));

            if (value == -1)
                throw new IOException("End of data reached.");

            return unchecked(buffer[0]);
        }

        public override SequentialReader WithByteOrder(bool isMotorolaByteOrder) => isMotorolaByteOrder == IsMotorolaByteOrder ? this : new SequentialStreamReader(_stream, isMotorolaByteOrder);

        public override async Task<byte[]> GetBytesAsync(int count)
        {
            var bytes = new byte[count];
            await GetBytesAsync(bytes, 0, count);
            return bytes;
        }

        public override async Task GetBytesAsync(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;
            while (totalBytesRead != count)
            {
                var bytesRead = await _stream.ReadAsync(buffer.AsMemory(offset + totalBytesRead, count - totalBytesRead));
                if (bytesRead == 0)
                    throw new IOException("End of data reached.");
                totalBytesRead += bytesRead;
            }
        }

        public override void Skip(long n)
        {
            if (n < 0)
                throw new ArgumentException("n must be zero or greater.");

            if (_stream.Position + n > _stream.Length)
                throw new IOException($"Unable to skip. Requested {n} bytes but only {_stream.Length - _stream.Position} remained.");

            _stream.Seek(n, SeekOrigin.Current);
        }

        public override bool TrySkip(long n)
        {
            try
            {
                Skip(n);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public override int Available()
        {
            return (int)(_stream.Length - _stream.Position);
        }

        protected override void Seek(int offset)
        {
            _stream.Seek(offset, SeekOrigin.Current);
        }

        public override bool IsCloserToEnd(long numberOfBytes)
        {
            return _stream.Position + numberOfBytes > _stream.Length;
        }
    }
}
