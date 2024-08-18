using System.Text;

namespace MetadataExtractor.IO
{
    public abstract class IndexedReader(bool isMotorolaByteOrder)
    {
        public bool IsMotorolaByteOrder { get; } = isMotorolaByteOrder;

        public abstract IndexedReader WithByteOrder(bool isMotorolaByteOrder);

        public abstract IndexedReader WithShiftedBaseOffset(int shift);

        public abstract int ToUnshiftedOffset(int localOffset);

        protected abstract byte GetByteInternal(int index);

        public abstract byte[] GetBytes(int index, int count);

        protected abstract void ValidateIndex(int index, int bytesRequested);

        protected abstract bool IsValidIndex(int index, int bytesRequested);

        public abstract long Length { get; }

        public bool GetBit(int index)
        {
            var byteIndex = index / 8;
            var bitIndex = index % 8;
            ValidateIndex(byteIndex, 1);
            var b = GetByteInternal(byteIndex);
            return ((b >> bitIndex) & 1) == 1;
        }

        public byte GetByte(int index)
        {
            ValidateIndex(index, 1);
            return GetByteInternal(index);
        }

        public sbyte GetSByte(int index)
        {
            ValidateIndex(index, 1);
            return unchecked((sbyte)GetByteInternal(index));
        }

#pragma warning disable format

        public ushort GetUInt16(int index)
        {
            ValidateIndex(index, 2);
            if (IsMotorolaByteOrder)
            {
                return (ushort)
                    (GetByteInternal(index    ) << 8 |
                     GetByteInternal(index + 1));
            }
            return (ushort)
                (GetByteInternal(index + 1) << 8 |
                 GetByteInternal(index    ));
        }

        public short GetInt16(int index)
        {
            ValidateIndex(index, 2);
            if (IsMotorolaByteOrder)
            {
                return (short)
                    (GetByteInternal(index    ) << 8 |
                     GetByteInternal(index + 1));
            }
            return (short)
                (GetByteInternal(index + 1) << 8 |
                 GetByteInternal(index));
        }

        public int GetInt24(int index)
        {
            ValidateIndex(index, 3);
            if (IsMotorolaByteOrder)
            {
                return
                    GetByteInternal(index    ) << 16 |
                    GetByteInternal(index + 1)  << 8 |
                    GetByteInternal(index + 2);
            }
            return
                GetByteInternal(index + 2) << 16 |
                GetByteInternal(index + 1) <<  8 |
                GetByteInternal(index    );
        }

        public uint GetUInt32(int index)
        {
            ValidateIndex(index, 4);
            if (IsMotorolaByteOrder)
            {
                return (uint)
                    (GetByteInternal(index    ) << 24 |
                     GetByteInternal(index + 1) << 16 |
                     GetByteInternal(index + 2) <<  8 |
                     GetByteInternal(index + 3));
            }
            return (uint)
                (GetByteInternal(index + 3) << 24 |
                 GetByteInternal(index + 2) << 16 |
                 GetByteInternal(index + 1) <<  8 |
                 GetByteInternal(index    ));
        }

        public int GetInt32(int index)
        {
            ValidateIndex(index, 4);
            if (IsMotorolaByteOrder)
            {
                return
                    GetByteInternal(index    ) << 24 |
                    GetByteInternal(index + 1) << 16 |
                    GetByteInternal(index + 2) <<  8 |
                    GetByteInternal(index + 3);
            }
            return
                GetByteInternal(index + 3) << 24 |
                GetByteInternal(index + 2) << 16 |
                GetByteInternal(index + 1) <<  8 |
                GetByteInternal(index    );
        }

        public long GetInt64(int index)
        {
            ValidateIndex(index, 8);
            if (IsMotorolaByteOrder)
            {
                // Motorola - MSB first
                return
                    (long)GetByteInternal(index    ) << 56 |
                    (long)GetByteInternal(index + 1) << 48 |
                    (long)GetByteInternal(index + 2) << 40 |
                    (long)GetByteInternal(index + 3) << 32 |
                    (long)GetByteInternal(index + 4) << 24 |
                    (long)GetByteInternal(index + 5) << 16 |
                    (long)GetByteInternal(index + 6) <<  8 |
                          GetByteInternal(index + 7);
            }
            return
                (long)GetByteInternal(index + 7) << 56 |
                (long)GetByteInternal(index + 6) << 48 |
                (long)GetByteInternal(index + 5) << 40 |
                (long)GetByteInternal(index + 4) << 32 |
                (long)GetByteInternal(index + 3) << 24 |
                (long)GetByteInternal(index + 2) << 16 |
                (long)GetByteInternal(index + 1) <<  8 |
                      GetByteInternal(index    );
        }

        public ulong GetUInt64(int index)
        {
            ValidateIndex(index, 8);
            if (IsMotorolaByteOrder)
            {
                return
                    (ulong)GetByteInternal(index    ) << 56 |
                    (ulong)GetByteInternal(index + 1) << 48 |
                    (ulong)GetByteInternal(index + 2) << 40 |
                    (ulong)GetByteInternal(index + 3) << 32 |
                    (ulong)GetByteInternal(index + 4) << 24 |
                    (ulong)GetByteInternal(index + 5) << 16 |
                    (ulong)GetByteInternal(index + 6) <<  8 |
                          GetByteInternal(index + 7);
            }
            return
                (ulong)GetByteInternal(index + 7) << 56 |
                (ulong)GetByteInternal(index + 6) << 48 |
                (ulong)GetByteInternal(index + 5) << 40 |
                (ulong)GetByteInternal(index + 4) << 32 |
                (ulong)GetByteInternal(index + 3) << 24 |
                (ulong)GetByteInternal(index + 2) << 16 |
                (ulong)GetByteInternal(index + 1) <<  8 |
                      GetByteInternal(index    );
        }

#pragma warning restore format

        public float GetS15Fixed16(int index)
        {
            ValidateIndex(index, 4);
            if (IsMotorolaByteOrder)
            {
                float res = GetByteInternal(index) << 8 | GetByteInternal(index + 1);
                var d = GetByteInternal(index + 2) << 8 | GetByteInternal(index + 3);
                return (float)(res + d / 65536.0);
            }
            else
            {
                var d = GetByteInternal(index + 1) << 8 | GetByteInternal(index);
                float res = GetByteInternal(index + 3) << 8 | GetByteInternal(index + 2);
                return (float)(res + d / 65536.0);
            }
        }

        public float GetFloat32(int index) => BitConverter.ToSingle(BitConverter.GetBytes(GetInt32(index)), 0);

        public double GetDouble64(int index) => BitConverter.Int64BitsToDouble(GetInt64(index));

        public string GetString(int index, int bytesRequested, Encoding encoding)
        {
            var bytes = GetBytes(index, bytesRequested);
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public string GetNullTerminatedString(int index, int maxLengthBytes, Encoding? encoding = null)
        {
            var bytes = GetNullTerminatedBytes(index, maxLengthBytes);

            return (encoding ?? Encoding.UTF8).GetString(bytes, 0, bytes.Length);
        }

        public StringValue GetNullTerminatedStringValue(int index, int maxLengthBytes, Encoding? encoding = null)
        {
            var bytes = GetNullTerminatedBytes(index, maxLengthBytes);

            return new StringValue(bytes, encoding);
        }

        public byte[] GetNullTerminatedBytes(int index, int maxLengthBytes)
        {
            var buffer = GetBytes(index, maxLengthBytes);

            var length = 0;
            while (length < buffer.Length && buffer[length] != 0)
                length++;

            if (length == maxLengthBytes)
                return buffer;

            var bytes = new byte[length];
            if (length > 0)
                Array.Copy(buffer, 0, bytes, 0, length);
            return bytes;
        }
    }
}
