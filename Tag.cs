namespace MetadataExtractor
{
    public sealed class Tag(int type, Directory directory)
    {
        public int Type { get; } = type;

        [Obsolete("Use Type instead.")]
        public int TagType => Type;

        public string? Description => directory.GetDescription(Type);

        public bool HasName => directory.HasTagName(Type);

        [Obsolete("Use HasName instead.")]
        public bool HasTagName => HasName;

        public string Name => directory.GetTagName(Type);

        [Obsolete("Use Name instead")]
        public string TagName => Name;

        public string? DirectoryName => directory.Name;

        public override string ToString() => $"[{DirectoryName}] {Name} - {Description ?? directory.GetString(Type) + " (unable to formulate description)"}";
    }
}
