namespace MetadataExtractor.IO
{
    public sealed class BitReader
    {
        private readonly SequentialReader _source;

        private byte _mask;
        private byte _currentByte;

        public BitReader(SequentialReader source)
        {
            _source = source;
        }

        public async Task<ulong> GetUInt64Async(int bits)
        {
            if (bits > 64)
                throw new ArgumentOutOfRangeException(nameof(bits), bits, "Must be less than or equal to 64.");
            ulong ret = 0;
            for (int i = 0; i < bits; i++)
            {
                ret <<= 1;
                if (await GetBitAsync())
                {
                    ret |= 1;
                }
            }

            return ret;
        }

        public async Task<uint> GetUInt32Async(int bits)
        {
            if (bits > 32)
                throw new ArgumentOutOfRangeException(nameof(bits), bits, "Must be less than or equal to 32.");
            return (uint)await GetUInt64Async(bits);
        }

        public async Task<ushort> GetUInt16Async(int bits)
        {
            if (bits > 16)
                throw new ArgumentOutOfRangeException(nameof(bits), bits, "Must be less than or equal to 16.");
            return (ushort)await GetUInt64Async(bits);
        }

        public async Task<byte> GetByteAsync(int bits)
        {
            if (bits > 8)
                throw new ArgumentOutOfRangeException(nameof(bits), bits, "Must be less than or equal to 8.");
            return (byte)await GetUInt64Async(bits);
        }

        public async Task<bool> GetBitAsync()
        {
            if (_mask == 0)
                await ReadWholeByteFromSourceAsync();
            var ret = (_mask & _currentByte) == _mask;
            _mask >>= 1;
            return ret;
        }

        private async Task ReadWholeByteFromSourceAsync()
        {
            _currentByte = await _source.GetByteAsync();
            _mask = 0b1000_0000;
        }
    }
}
