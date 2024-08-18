namespace MetadataExtractor
{
    public static class DirectoryExtensions
    {
        public static bool TryGetByte(this Directory directory, int tagType, out byte value)
        {
            if (directory.ContainsByteTag(tagType))
            {
                value = directory.GetByte(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetShort(this Directory directory, int tagType, out short value)
        {
            if (directory.ContainsIntTag(tagType))
            {
                value = directory.GetShort(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetUshort(this Directory directory, int tagType, out ushort value)
        {
            if (directory.ContainsUshortTag(tagType))
            {
                value = directory.GetUshort(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetInt(this Directory directory, int tagType, out int value)
        {
            if (directory.ContainsIntTag(tagType))
            {
                value = directory.GetInt(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetUint(this Directory directory, int tagType, out uint value)
        {
            if (directory.ContainsUintTag(tagType))
            {
                value = directory.GetUint(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetLong(this Directory directory, int tagType, out long value)
        {
            if (directory.ContainsLongTag(tagType))
            {
                value = directory.GetLong(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetUlong(this Directory directory, int tagType, out ulong value)
        {
            if (directory.ContainsUlongTag(tagType))
            {
                value = directory.GetUlong(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetFloat(this Directory directory, int tagType, out float value)
        {
            if (directory.ContainsFloatTag(tagType))
            {
                value = directory.GetFloat(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetDouble(this Directory directory, int tagType, out double value)
        {
            if (directory.ContainsDoubleTag(tagType))
            {
                value = directory.GetDouble(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetBoolean(this Directory directory, int tagType, out bool value)
        {
            if (directory.ContainsBoolTag(tagType))
            {
                value = directory.GetBool(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetArray(this Directory directory, int tagType, out Array? array)
        {
            if (directory.ContainsArrayTag(tagType))
            {
                array = directory.GetArray<Array>(tagType);

                return true;
            }

            array = null;

            return false;
        }

        public static bool TryGetDateTime(this Directory directory, int tagType, out DateTime dateTime)
        {
            if (directory.ContainsDataTimeTag(tagType))
            {
                dateTime = directory.GetDateTime(tagType);

                return true;
            }

            dateTime = default;

            return false;
        }

        public static bool TryGetRational(this Directory directory, int tagType, out Rational value)
        {
            if (directory.ContainsRationalTag(tagType))
            {
                value = directory.GetRational(tagType);

                return true;
            }

            value = default;

            return false;
        }

        public static bool TryGetString(this Directory directory, int tagType, out string? value)
        {
            if (directory.ContainsStringTag(tagType))
            {
                value = directory.GetString(tagType);

                return true;
            }

            value = string.Empty;

            return false;
        }

        public static bool TryGetStringValue(this Directory directory, int tagType, out StringValue value)
        {
            if (directory.ContainsStringValueTag(tagType))
            {
                value = directory.GetStringValue(tagType);

                return true;
            }

            value = default;

            return false;
        }
    }
}
