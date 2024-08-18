using System.Diagnostics.CodeAnalysis;

using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace MetadataExtractor
{
    public abstract class Directory(Dictionary<int, string> tagNameMap)
    {
        internal static readonly DirectoryList EmptyList = [];

        private readonly Dictionary<int, string> _tagNameMap = tagNameMap;

        private readonly Dictionary<int, bool> _tagBoolMap = [];

        private readonly Dictionary<int, short> _tagShortMap = [];

        private readonly Dictionary<int, int> _tagIntMap = [];

        private readonly Dictionary<int, long> _tagLongMap = [];

        private readonly Dictionary<int, ushort> _tagUshortMap = [];

        private readonly Dictionary<int, uint> _tagUintMap = [];

        private readonly Dictionary<int, ulong> _tagUlongMap = [];

        private readonly Dictionary<int, float> _tagFloatMap = [];

        private readonly Dictionary<int, double> _tagDoubleMap = [];

        private readonly Dictionary<int, byte> _tagBytesMap = [];

        private readonly Dictionary<int, Array> _tagArrayMap = [];

        private readonly Dictionary<int, string> _tagStringMap = [];

        private readonly Dictionary<int, StringValue> _tagStringValueMap = [];

        private readonly Dictionary<int, DateTime> _tagDateTimeMap = [];

        private readonly Dictionary<int, DateTimeOffset> _tagDateTimeOffsetMap = [];

        private readonly Dictionary<int, Version> _tagVersionMap = [];

        private readonly Dictionary<int, Rational> _tagRationalMap = [];

        private readonly Dictionary<int, KeyValuePair<string, StringValue>> _tagKeyValuePairMap = [];

        private readonly Dictionary<int, MetadataExtractor.Formats.Jpeg.JpegComponent> _tagJpegComponentMap = [];

        private readonly List<Tag> _definedTagList = [];

        private readonly List<string> _errorList = new(capacity: 4);

        private ITagDescriptor? _descriptor;

        public abstract string? Name { get; }

        public Directory? Parent { get; internal set; }

        protected virtual bool TryGetTagName(int tagType, [NotNullWhen(returnValue: true)] out string? tagName)
        {
            if (_tagNameMap is null)
            {
                tagName = default;
                return false;
            }

            return _tagNameMap.TryGetValue(tagType, out tagName);
        }

        public bool IsEmpty => _errorList.Count == 0 && _definedTagList.Count == 0;

        public bool ContainsTag(int tagType) => _tagBoolMap.ContainsKey(tagType) || _tagShortMap.ContainsKey(tagType) || _tagIntMap.ContainsKey(tagType) || _tagLongMap.ContainsKey(tagType) || _tagUshortMap.ContainsKey(tagType) || _tagUintMap.ContainsKey(tagType) || _tagUlongMap.ContainsKey(tagType) || _tagFloatMap.ContainsKey(tagType) || _tagDoubleMap.ContainsKey(tagType) || _tagBytesMap.ContainsKey(tagType) || _tagArrayMap.ContainsKey(tagType) || _tagStringMap.ContainsKey(tagType) || _tagDateTimeMap.ContainsKey(tagType) || _tagDateTimeOffsetMap.ContainsKey(tagType) || _tagVersionMap.ContainsKey(tagType) || _tagRationalMap.ContainsKey(tagType) || _tagKeyValuePairMap.ContainsKey(tagType) || _tagJpegComponentMap.ContainsKey(tagType);

        public bool ContainsShortTag(int tagType) => _tagShortMap.ContainsKey(tagType);

        public bool ContainsBoolTag(int tagType) => _tagBoolMap.ContainsKey(tagType);

        public bool ContainsIntTag(int tagType) => _tagIntMap.ContainsKey(tagType);

        public bool ContainsLongTag(int tagType) => _tagLongMap.ContainsKey(tagType);

        public bool ContainsUshortTag(int tagType) => _tagUshortMap.ContainsKey(tagType);

        public bool ContainsUintTag(int tagType) => _tagUintMap.ContainsKey(tagType);

        public bool ContainsUlongTag(int tagType) => _tagUlongMap.ContainsKey(tagType);

        public bool ContainsFloatTag(int tagType) => _tagFloatMap.ContainsKey(tagType);

        public bool ContainsDoubleTag(int tagType) => _tagDoubleMap.ContainsKey(tagType);

        public bool ContainsByteTag(int tagType) => _tagBytesMap.ContainsKey(tagType);

        public bool ContainsStringTag(int tagType) => _tagStringMap.ContainsKey(tagType);

        public bool ContainsStringValueTag(int tagType) => _tagStringValueMap.ContainsKey(tagType);

        public bool ContainsArrayTag(int tagType) => _tagArrayMap.ContainsKey(tagType);

        public bool ContainsDataTimeTag(int tagType) => _tagDateTimeMap.ContainsKey(tagType);

        public bool ContainsDataTimeOffsetTag(int tagType) => _tagDateTimeOffsetMap.ContainsKey(tagType);

        public bool ContainsVersionTag(int tagType) => _tagVersionMap.ContainsKey(tagType);

        public bool ContainsRationalTag(int tagType) => _tagRationalMap.ContainsKey(tagType);

        public bool ContainsKeyValuePairTag(int tagType) => _tagKeyValuePairMap.ContainsKey(tagType);

        public bool ContainsJpegComponentTag(int tagType) => _tagJpegComponentMap.ContainsKey(tagType);

        public
            IReadOnlyList<Tag>
            Tags => _definedTagList;

        public int TagCount => _definedTagList.Count;

        protected void SetDescriptor(ITagDescriptor descriptor)
        {
            _descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
        }

        public void AddError(string message) => _errorList.Add(message);

        public bool HasError => _errorList.Count > 0;

        public IReadOnlyList<string> Errors => _errorList;

        public virtual void Set(int tagType, bool value)
        {
            if (!_tagBoolMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagBoolMap[tagType] = value;
        }

        public virtual void Set(int tagType, short value)
        {
            if (!_tagShortMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagShortMap[tagType] = value;
        }

        public virtual void Set(int tagType, int value)
        {
            if (!_tagIntMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagIntMap[tagType] = value;
        }

        public virtual void Set(int tagType, long value)
        {
            if (!_tagLongMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagLongMap[tagType] = value;
        }

        public virtual void Set(int tagType, ushort value)
        {
            if (!_tagUshortMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagUshortMap[tagType] = value;
        }

        public virtual void Set(int tagType, uint value)
        {
            if (!_tagUintMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagUintMap[tagType] = value;
        }

        public virtual void Set(int tagType, ulong value)
        {
            if (!_tagUlongMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagUlongMap[tagType] = value;
        }

        public virtual void Set(int tagType, float value)
        {
            if (!_tagFloatMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagFloatMap[tagType] = value;
        }

        public virtual void Set(int tagType, double value)
        {
            if (!_tagDoubleMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagDoubleMap[tagType] = value;
        }

        public virtual void Set(int tagType, Array value)
        {
            if (!_tagArrayMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagArrayMap[tagType] = value;
        }

        public virtual void Set(int tagType, string value)
        {
            if (!_tagStringMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagStringMap[tagType] = value;
        }

        public virtual void Set(int tagType, StringValue value)
        {
            if (!_tagStringValueMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagStringValueMap[tagType] = value;
        }

        public virtual void Set(int tagType, DateTime value)
        {
            if (!_tagDateTimeMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagDateTimeMap[tagType] = value;
        }

        public virtual void Set(int tagType, DateTimeOffset value)
        {
            if (!_tagDateTimeOffsetMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagDateTimeOffsetMap[tagType] = value;
        }

        public virtual void Set(int tagType, Version value)
        {
            if (!_tagVersionMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagVersionMap[tagType] = value;
        }

        public virtual void Set(int tagType, Rational value)
        {
            if (!_tagRationalMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagRationalMap[tagType] = value;
        }

        public virtual void Set(int tagType, KeyValuePair<string, StringValue> value)
        {
            if (!_tagKeyValuePairMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagKeyValuePairMap[tagType] = value;
        }

        public virtual void Set(int tagType, MetadataExtractor.Formats.Jpeg.JpegComponent value)
        {
            if (!_tagJpegComponentMap.ContainsKey(tagType))
                _definedTagList.Add(new Tag(tagType, this));

            _tagJpegComponentMap[tagType] = value;
        }

        public bool GetBool(int tagType) => _tagBoolMap.TryGetValue(tagType, out var val) && val;

        public short GetShort(int tagType) => _tagShortMap.TryGetValue(tagType, out short val) ? val : default;

        public int GetInt(int tagType) => _tagIntMap.TryGetValue(tagType, out int val) ? val : default;

        public long GetLong(int tagType) => _tagLongMap.TryGetValue(tagType, out long val) ? val : default;

        public ushort GetUshort(int tagType) => _tagUshortMap.TryGetValue(tagType, out ushort val) ? val : default;

        public uint GetUint(int tagType) => _tagUintMap.TryGetValue(tagType, out uint val) ? val : default;

        public ulong GetUlong(int tagType) => _tagUlongMap.TryGetValue(tagType, out ulong val) ? val : default;

        public float GetFloat(int tagType) => _tagFloatMap.TryGetValue(tagType, out float val) ? val : default;

        public double GetDouble(int tagType) => _tagDoubleMap.TryGetValue(tagType, out double val) ? val : default;

        public byte GetByte(int tagType) => _tagBytesMap.TryGetValue(tagType, out byte val) ? val : default;

        public T? GetArray<T>(int tagType) where T : class => _tagArrayMap.TryGetValue(tagType, out Array? val) ? val as T : null;

        public string? GetString(int tagType) => _tagStringMap.TryGetValue(tagType, out string? val) ? val : default;

        public StringValue GetStringValue(int tagType) => _tagStringValueMap.TryGetValue(tagType, out StringValue val) ? val : default;

        public DateTime GetDateTime(int tagType) => _tagDateTimeMap.TryGetValue(tagType, out DateTime val) ? val : default;

        public Version? GetVersion(int tagType) => _tagVersionMap.TryGetValue(tagType, out Version? val) ? val : default;

        public Rational GetRational(int tagType) => _tagRationalMap.TryGetValue(tagType, out Rational val) ? val : default;

        public KeyValuePair<string, StringValue> GetKeyValuePair(int tagType) => _tagKeyValuePairMap.TryGetValue(tagType, out KeyValuePair<string, StringValue> val) ? val : default;

        public MetadataExtractor.Formats.Jpeg.JpegComponent? GetJpegComponent(int tagType) => _tagJpegComponentMap.TryGetValue(tagType, out MetadataExtractor.Formats.Jpeg.JpegComponent? val) ? val : null;

        public void RemoveTag(int tagId)
        {
            if (_tagBoolMap.Remove(tagId) || _tagIntMap.Remove(tagId) || _tagLongMap.Remove(tagId) || _tagDoubleMap.Remove(tagId) || _tagArrayMap.Remove(tagId) || _tagStringMap.Remove(tagId) || _tagStringValueMap.Remove(tagId) || _tagDateTimeMap.Remove(tagId) || _tagDateTimeOffsetMap.Remove(tagId) || _tagVersionMap.Remove(tagId) || _tagRationalMap.Remove(tagId) || _tagKeyValuePairMap.Remove(tagId) || _tagJpegComponentMap.Remove(tagId))
            {
                var index = _definedTagList.FindIndex(tag => tag.Type == tagId);

                if (index != -1)
                {
                    _definedTagList.RemoveAt(index);
                }
            }
        }

        public string GetTagName(int tagType)
        {
            return !TryGetTagName(tagType, out string? name)
                ? $"Unknown tag (0x{tagType:x4})"
                : name;
        }

        public bool HasTagName(int tagType) => TryGetTagName(tagType, out _);

        public string? GetDescription(int tagType)
        {
            return _descriptor!.GetDescription(tagType);
        }

        public override string ToString() => $"{Name} Directory ({_tagBoolMap.Count + _tagShortMap.Count + _tagIntMap.Count + _tagLongMap.Count + _tagUshortMap.Count + _tagUintMap.Count + _tagUlongMap.Count + _tagFloatMap.Count + _tagDoubleMap.Count + _tagBytesMap.Count + _tagStringMap.Count + _tagStringValueMap.Count + _tagArrayMap.Count + _tagDateTimeMap.Count + _tagDateTimeOffsetMap.Count + _tagVersionMap.Count + _tagRationalMap.Count + _tagKeyValuePairMap.Count + _tagJpegComponentMap.Count} {((_tagBoolMap.Count + _tagIntMap.Count + _tagLongMap.Count + _tagDoubleMap.Count + _tagArrayMap.Count + _tagStringMap.Count + _tagStringValueMap.Count + _tagDateTimeMap.Count + _tagDateTimeOffsetMap.Count + _tagVersionMap.Count + _tagRationalMap.Count + _tagKeyValuePairMap.Count + _tagJpegComponentMap.Count) == 1 ? "tag" : "tags")})";
    }

    public sealed class ErrorDirectory : Directory
    {
        public override string Name => "Error";

        public ErrorDirectory() : base([]) { }

        public ErrorDirectory(string error) : this() => AddError(error);

        public override void Set(int tagType, int value) => throw new NotSupportedException($"Cannot add values to {nameof(ErrorDirectory)}.");
    }
}
