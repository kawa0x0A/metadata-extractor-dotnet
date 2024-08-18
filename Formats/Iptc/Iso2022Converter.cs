using System.Text;

namespace MetadataExtractor.Formats.Iptc
{
    public static class Iso2022Converter
    {
        private const int Dot = 0xe280a2;
        private const byte LatinCapitalA = (byte)'A';
        private const byte LatinCapitalG = (byte)'G';
        private const byte MinusSign = (byte)'-';
        private const byte PercentSign = (byte)'%';
        private const byte Esc = 0x1B;

        public static string? ConvertEscapeSequenceToEncodingName(byte[] bytes)
        {
            if (bytes.Length > 2 && bytes[0] == Esc && bytes[1] == PercentSign && bytes[2] == LatinCapitalG)
                return "UTF-8";

            if (bytes.Length > 3 && bytes[0] == Esc && (bytes[3] | (bytes[2] << 8) | (bytes[1] << 16)) == Dot && bytes[4] == LatinCapitalA)
                return "ISO-8859-1";

            if (bytes.Length > 2 && bytes[0] == Esc && bytes[1] == MinusSign && bytes[2] == LatinCapitalA)
                return "ISO-8859-1";

            return null;
        }

        internal static Encoding? GuessEncoding(byte[] bytes)
        {
            var ascii = true;
            foreach (var b in bytes)
            {
                if (b < 0x20 || b > 0x7f)
                {
                    ascii = false;
                    break;
                }
            }

            if (ascii)
            {
                return Encoding.ASCII;
            }

            var utf8 = false;
            var i = 0;
            while (i < bytes.Length - 4)
            {
                if (bytes[i] <= 0x7F) { i++; continue; }
                if (bytes[i] >= 0xC2 && bytes[i] <= 0xDF && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0) { i += 2; utf8 = true; continue; }
                if (bytes[i] >= 0xE0 && bytes[i] <= 0xF0 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && bytes[i + 2] >= 0x80 && bytes[i + 2] < 0xC0) { i += 3; utf8 = true; continue; }
                if (bytes[i] >= 0xF0 && bytes[i] <= 0xF4 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && bytes[i + 2] >= 0x80 && bytes[i + 2] < 0xC0 && bytes[i + 3] >= 0x80 && bytes[i + 3] < 0xC0) { i += 4; utf8 = true; continue; }
                utf8 = false;
                break;
            }

            if (utf8)
                return Encoding.UTF8;

            Encoding[] encodings =
            [
                Encoding.UTF8,
                Encoding.GetEncoding("iso-8859-1") // Latin-1
            ];

            foreach (var encoding in encodings)
            {
                try
                {
                    var s = encoding!.GetString(bytes, 0, bytes.Length);
                    if (s.Contains((char)65533))
                        continue;
                    return encoding;
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
