using System.Text;

namespace MetadataExtractor.IO
{
    public abstract class SequentialReader(bool isMotorolaByteOrder)
    {
        public bool IsMotorolaByteOrder { get; } = isMotorolaByteOrder;

        public abstract long Position { get; }

        public abstract SequentialReader WithByteOrder(bool isMotorolaByteOrder);

        public abstract Task<byte[]> GetBytesAsync(int count);

        public abstract Task GetBytesAsync(byte[] buffer, int offset, int count);

        public abstract void Skip(long n);

        public abstract bool TrySkip(long n);

        public abstract int Available();

        protected abstract void Seek(int offset);

        public abstract byte GetByte();

        public abstract Task<byte> GetByteAsync();

        public async Task<sbyte> GetSByteAsync() => unchecked((sbyte)await GetByteAsync());

#pragma warning disable format

        public ushort GetUInt16()
        {
            if (IsMotorolaByteOrder)
            {
                return (ushort)
                    (GetByte() << 8 |
                     GetByte());
            }
            return (ushort)
                (GetByte() |
                 GetByte() << 8);
        }

        public async Task<ushort> GetUInt16Async()
        {
            if (IsMotorolaByteOrder)
            {
                return (ushort)
                    (await GetByteAsync() << 8 |
                     await GetByteAsync());
            }
            return (ushort)
                (await GetByteAsync() |
                 await GetByteAsync() << 8);
        }

        public short GetInt16()
        {
            if (IsMotorolaByteOrder)
            {
                return (short)
                    (GetByte() << 8 |
                     GetByte());
            }
            return (short)
                (GetByte() |
                 GetByte() << 8);
        }

        public async Task<short> GetInt16Async()
        {
            if (IsMotorolaByteOrder)
            {
                return (short)
                    (await GetByteAsync() << 8 |
                     await GetByteAsync());
            }
            return (short)
                (await GetByteAsync() |
                 await GetByteAsync() << 8);
        }

        public uint GetUInt32()
        {
            if (IsMotorolaByteOrder)
            {
                return (uint)
                    (GetByte() << 24 |
                     GetByte() << 16 |
                     GetByte() << 8  |
                     GetByte());
            }
            return (uint)
                (GetByte()       |
                 GetByte() << 8  |
                 GetByte() << 16 |
                 GetByte() << 24);
        }

        public async Task<uint> GetUInt32Async()
        {
            if (IsMotorolaByteOrder)
            {
                return (uint)
                    (await GetByteAsync() << 24 |
                     await GetByteAsync() << 16 |
                     await GetByteAsync() << 8  |
                     await GetByteAsync());
            }
            return (uint)
                (await GetByteAsync()       |
                 await GetByteAsync() << 8  |
                 await GetByteAsync() << 16 |
                 await GetByteAsync() << 24);
        }

        public int GetInt32()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    GetByte() << 24 |
                    GetByte() << 16 |
                    GetByte() << 8  |
                    GetByte();
            }
            return
                GetByte()       |
                GetByte() <<  8 |
                GetByte() << 16 |
                GetByte() << 24;
        }

        public async Task<int> GetInt32Async()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    await GetByteAsync() << 24 |
                    await GetByteAsync() << 16 |
                    await GetByteAsync() << 8  |
                    await GetByteAsync();
            }
            return
                await GetByteAsync()       |
                await GetByteAsync() <<  8 |
                await GetByteAsync() << 16 |
                await GetByteAsync() << 24;
        }

        public long GetInt64()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    (long)GetByte() << 56 |
                    (long)GetByte() << 48 |
                    (long)GetByte() << 40 |
                    (long)GetByte() << 32 |
                    (long)GetByte() << 24 |
                    (long)GetByte() << 16 |
                    (long)GetByte() << 8  |
                          GetByte();
            }
            return
                      GetByte()       |
                (long)GetByte() << 8  |
                (long)GetByte() << 16 |
                (long)GetByte() << 24 |
                (long)GetByte() << 32 |
                (long)GetByte() << 40 |
                (long)GetByte() << 48 |
                (long)GetByte() << 56;
        }

        public async Task<long> GetInt64Async()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    (long)await GetByteAsync() << 56 |
                    (long)await GetByteAsync() << 48 |
                    (long)await GetByteAsync() << 40 |
                    (long)await GetByteAsync() << 32 |
                    (long)await GetByteAsync() << 24 |
                    (long)await GetByteAsync() << 16 |
                    (long)await GetByteAsync() << 8  |
                          await GetByteAsync();
            }
            return
                      await GetByteAsync()       |
                (long)await GetByteAsync() << 8  |
                (long)await GetByteAsync() << 16 |
                (long)await GetByteAsync() << 24 |
                (long)await GetByteAsync() << 32 |
                (long)await GetByteAsync() << 40 |
                (long)await GetByteAsync() << 48 |
                (long)await GetByteAsync() << 56;
        }

        public ulong GetUInt64()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    (ulong)GetByte() << 56 |
                    (ulong)GetByte() << 48 |
                    (ulong)GetByte() << 40 |
                    (ulong)GetByte() << 32 |
                    (ulong)GetByte() << 24 |
                    (ulong)GetByte() << 16 |
                    (ulong)GetByte() << 8  |
                           GetByte();
            }
            return
                       GetByte()       |
                (ulong)GetByte() << 8  |
                (ulong)GetByte() << 16 |
                (ulong)GetByte() << 24 |
                (ulong)GetByte() << 32 |
                (ulong)GetByte() << 40 |
                (ulong)GetByte() << 48 |
                (ulong)GetByte() << 56;
        }

        public async Task<ulong> GetUInt64Async()
        {
            if (IsMotorolaByteOrder)
            {
                return
                    (ulong)await GetByteAsync() << 56 |
                    (ulong)await GetByteAsync() << 48 |
                    (ulong)await GetByteAsync() << 40 |
                    (ulong)await GetByteAsync() << 32 |
                    (ulong)await GetByteAsync() << 24 |
                    (ulong)await GetByteAsync() << 16 |
                    (ulong)await GetByteAsync() << 8  |
                           await GetByteAsync();
            }
            return
                       await GetByteAsync()       |
                (ulong)await GetByteAsync() << 8  |
                (ulong)await GetByteAsync() << 16 |
                (ulong)await GetByteAsync() << 24 |
                (ulong)await GetByteAsync() << 32 |
                (ulong)await GetByteAsync() << 40 |
                (ulong)await GetByteAsync() << 48 |
                (ulong)await GetByteAsync() << 56;
        }

#pragma warning restore format

        public async Task<float> GetS15Fixed16Async()
        {
            if (IsMotorolaByteOrder)
            {
                float res = await GetByteAsync() << 8 | await GetByteAsync();
                var d = await GetByteAsync() << 8 | await GetByteAsync();
                return (float)(res + d / 65536.0);
            }
            else
            {
                var d = await GetByteAsync() | await GetByteAsync() << 8;
                float res = await GetByteAsync() | await GetByteAsync() << 8;
                return (float)(res + d / 65536.0);
            }
        }

        public async Task<float> GetFloat32Async() => BitConverter.ToSingle(BitConverter.GetBytes(await GetInt32Async()), 0);

        public async Task<double> GetDouble64Async() => BitConverter.Int64BitsToDouble(await GetInt64Async());

        public async Task<string> GetStringAsync(int bytesRequested, Encoding encoding)
        {
            var bytes = await GetBytesAsync(bytesRequested);
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public async Task<StringValue> GetStringValueAsync(int bytesRequested, Encoding? encoding = null)
        {
            return new(await GetBytesAsync(bytesRequested), encoding);
        }

        public async Task<string> GetNullTerminatedStringAsync(int maxLengthBytes, Encoding? encoding = null)
        {
            var bytes = await GetNullTerminatedBytesAsync(maxLengthBytes);

            return (encoding ?? Encoding.UTF8).GetString(bytes, 0, bytes.Length);
        }

        public async Task<StringValue> GetNullTerminatedStringValueAsync(int maxLengthBytes, Encoding? encoding = null)
        {
            var bytes = await GetNullTerminatedBytesAsync(maxLengthBytes);

            return new StringValue(bytes, encoding);
        }

        public async Task<byte[]> GetNullTerminatedBytesAsync(int maxLengthBytes)
        {
            var buffer = await GetBytesAsync(Math.Min(maxLengthBytes, Available()));

            var index = Array.IndexOf(buffer, (byte)0);

            if (index != -1)
            {
                Seek(index - buffer.Length + 1);

                return buffer.Take(index).ToArray();
            }

            return buffer;

            /*
            var buffer = new byte[maxLengthBytes];

            var length = 0;
            while (length < buffer.Length && (buffer[length] = await GetByteAsync()) != 0)
                length++;

            if (length == maxLengthBytes)
                return buffer;

            var bytes = new byte[length];
            if (length > 0)
                Array.Copy(buffer, bytes, length);
            return bytes;
            */
        }

        public virtual bool IsCloserToEnd(long numberOfBytes)
        {
            return false;
        }
    }
}
