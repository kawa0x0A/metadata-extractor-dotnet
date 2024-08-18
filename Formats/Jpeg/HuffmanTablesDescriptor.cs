namespace MetadataExtractor.Formats.Jpeg
{
    public sealed class HuffmanTablesDescriptor(HuffmanTablesDirectory directory) : TagDescriptor<HuffmanTablesDirectory>(directory)
    {
        public override string? GetDescription(int tagType)
        {
            return tagType switch
            {
                HuffmanTablesDirectory.TagNumberOfTables => GetNumberOfTablesDescription(),
                _ => base.GetDescription(tagType),
            };
        }

        public string? GetNumberOfTablesDescription()
        {
            if (!Directory.TryGetInt(HuffmanTablesDirectory.TagNumberOfTables, out int value))
                return null;

            return value + (value == 1 ? " Huffman table" : " Huffman tables");
        }

    }
}
