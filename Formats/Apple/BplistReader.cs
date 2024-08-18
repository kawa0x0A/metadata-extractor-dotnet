using MetadataExtractor.IO;
using System.Text;

namespace MetadataExtractor.Formats.Apple;

public sealed class BplistReader
{
    private static readonly byte[] _bplistHeader = { (byte)'b', (byte)'p', (byte)'l', (byte)'i', (byte)'s', (byte)'t', (byte)'0', (byte)'0' };

    public static bool IsValid(byte[] bplist)
    {
        if (bplist.Length < _bplistHeader.Length)
        {
            return false;
        }

        for (int i = 0; i < _bplistHeader.Length; i++)
        {
            if (bplist[i] != _bplistHeader[i])
            {
                return false;
            }
        }

        return true;
    }

    public static async Task<PropertyListResults> ParseAsync(byte[] bplist)
    {
        if (!IsValid(bplist))
        {
            throw new ArgumentException("Input is not a bplist.", nameof(bplist));
        }

        Trailer trailer = await ReadTrailerAsync();

        SequentialByteArrayReader reader = new(bplist, baseIndex: checked((int)(trailer.OffsetTableOffset + trailer.TopObject)));

        int[] offsets = new int[(int)trailer.NumObjects];

        for (long i = 0; i < trailer.NumObjects; i++)
        {
            if (trailer.OffsetIntSize == 1)
            {
                offsets[(int)i] = await reader.GetByteAsync();
            }
            else if (trailer.OffsetIntSize == 2)
            {
                offsets[(int)i] = await reader.GetUInt16Async();
            }
        }

        List<object> objects = new();

        for (int i = 0; i < offsets.Length; i++)
        {
            reader = new SequentialByteArrayReader(bplist, offsets[i]);

            byte b = await reader.GetByteAsync();

            byte objectFormat = (byte)((b >> 4) & 0x0F);
            byte marker = (byte)(b & 0x0F);

            object obj = objectFormat switch
            {
                0x0D => HandleDictAsync(marker),
                0x05 => reader.GetStringAsync(bytesRequested: marker & 0x0F, Encoding.ASCII),
                0x04 => HandleDataAsync(marker),
                0x01 => HandleInt(marker),
                _ => throw new NotSupportedException($"Unsupported object format {objectFormat:X2}.")
            };

            objects.Add(obj);
        }

        return new PropertyListResults(objects, trailer);

        async Task<Trailer> ReadTrailerAsync()
        {
            SequentialByteArrayReader reader = new(bplist, bplist.Length - Trailer.SizeBytes);

            reader.Skip(6);

            return new Trailer
            {
                OffsetIntSize = await reader.GetByteAsync(),
                ObjectRefSize = await reader.GetByteAsync(),
                NumObjects = await reader.GetInt64Async(),
                TopObject = await reader.GetInt64Async(),
                OffsetTableOffset = await reader.GetInt64Async()
            };
        }

        object HandleInt(byte marker)
        {
            return marker switch
            {
                0 => (object)reader.GetByteAsync(),
                1 => reader.GetInt16Async(),
                2 => reader.GetInt32Async(),
                3 => reader.GetInt64Async(),
                _ => throw new NotSupportedException($"Unsupported int size {marker}.")
            };
        }

        async Task<Dictionary<byte, byte>> HandleDictAsync(byte count)
        {
            var keyRefs = new byte[count];

            for (int j = 0; j < count; j++)
            {
                keyRefs[j] = await reader.GetByteAsync();
            }

            Dictionary<byte, byte> map = new();

            for (int j = 0; j < count; j++)
            {
                map.Add(keyRefs[j], await reader.GetByteAsync());
            }

            return map;
        }

        async Task<object> HandleDataAsync(byte marker)
        {
            int byteCount = marker;

            if (marker == 0x0F)
            {
                byte sizeMarker = await reader.GetByteAsync();

                if (((sizeMarker >> 4) & 0x0F) != 1)
                {
                    throw new NotSupportedException($"Invalid size marker {sizeMarker:X2}.");
                }

                int sizeType = sizeMarker & 0x0F;

                if (sizeType == 0)
                {
                    byteCount = await reader.GetByteAsync();
                }
                else if (sizeType == 1)
                {
                    byteCount = await reader.GetUInt16Async();
                }
            }

            return reader.GetBytesAsync(byteCount);
        }
    }

    public sealed class PropertyListResults
    {
        private readonly List<object> _objects;
        private readonly Trailer _trailer;

        internal PropertyListResults(List<object> objects, Trailer trailer)
        {
            _objects = objects;
            _trailer = trailer;
        }

        public Dictionary<byte, byte>? GetTopObject()
        {
            return _objects[checked((int)_trailer.TopObject)] as Dictionary<byte, byte>;
        }

        public object Get(byte key)
        {
            return _objects[key];
        }
    }

    internal class Trailer
    {
        public const int SizeBytes = 32;
        public byte OffsetIntSize { get; init; }
        public byte ObjectRefSize { get; init; }
        public long NumObjects { get; init; }
        public long TopObject { get; init; }
        public long OffsetTableOffset { get; init; }
    }
}
