using XmpCore;
using XmpCore.Impl;
using XmpCore.Options;

namespace MetadataExtractor.Formats.Xmp
{
    public sealed class XmpDirectory : Directory
    {
        public const int TagXmpValueCount = 0xFFFF;


        private static readonly Dictionary<int, string> _tagNameMap = new()
        {
            { TagXmpValueCount, "XMP Value Count" }
        };

        public IXmpMeta? XmpMeta { get; private set; }

        public XmpDirectory() : base(_tagNameMap)
        {
            SetDescriptor(new XmpDescriptor(this));
        }

        public override string Name => "XMP";

        private static readonly IteratorOptions _iteratorOptions = new() { IsJustLeafNodes = true };

        public IDictionary<string, string> GetXmpProperties()
        {
            var propertyValueByPath = new Dictionary<string, string>();
            if (XmpMeta is not null)
            {
                try
                {
                    XmpIterator i = new((XmpMeta)XmpMeta, null, null, _iteratorOptions);
                    while (i.HasNext())
                    {
                        var prop = (IXmpPropertyInfo)i.Next();
                        var path = prop.Path;
                        var value = prop.Value;
                        if (path is not null && value is not null)
                            propertyValueByPath.Add(path, value);
                    }
                }
                catch (XmpException) { }
            }

            return propertyValueByPath;
        }

        public void SetXmpMeta(IXmpMeta xmpMeta)
        {
            XmpMeta = xmpMeta;

            int valueCount = 0;
            XmpIterator i = new((XmpMeta)XmpMeta, null, null, _iteratorOptions);

            while (i.HasNext())
            {
                var prop = (IXmpPropertyInfo)i.Next();

                if (prop.Path is not null)
                    valueCount++;
            }

            Set(TagXmpValueCount, valueCount);
        }
    }
}
