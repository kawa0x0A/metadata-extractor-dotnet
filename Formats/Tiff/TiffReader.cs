using MetadataExtractor.IO;

namespace MetadataExtractor.Formats.Tiff
{
    public static class TiffReader
    {
        public static async Task ProcessTiffAsync(IndexedReader reader, ITiffHandler handler)
        {
            var byteOrder = reader.GetInt16(0);

            reader = byteOrder switch
            {
                0x4d4d => reader.WithByteOrder(isMotorolaByteOrder: true),
                0x4949 => reader.WithByteOrder(isMotorolaByteOrder: false),
                _ => throw new TiffProcessingException("Unclear distinction between Motorola/Intel byte ordering: " + byteOrder),
            };

            var tiffMarker = reader.GetUInt16(2);
            var tiffStandard = handler.ProcessTiffMarker(tiffMarker);

            bool isBigTiff;

            int firstIfdOffset;

            switch (tiffStandard)
            {
                case TiffStandard.Tiff:
                    isBigTiff = false;
                    firstIfdOffset = checked((int)reader.GetUInt32(4));

                    if (firstIfdOffset >= reader.Length - 1)
                    {
                        handler.Warn("First IFD offset is beyond the end of the TIFF data segment -- trying default offset");

                        firstIfdOffset = 2 + 2 + 4;
                    }

                    break;

                case TiffStandard.BigTiff:
                    isBigTiff = true;
                    var offsetByteSize = reader.GetInt16(4);

                    if (offsetByteSize != 8)
                    {
                        handler.Error($"Unsupported offset byte size: {offsetByteSize}");
                        return;
                    }

                    firstIfdOffset = checked((int)reader.GetUInt64(8));
                    break;

                default:
                    handler.Error($"Unsupported TiffStandard {tiffStandard}.");
                    return;
            }

            var context = new TiffReaderContext(reader, reader.IsMotorolaByteOrder, isBigTiff);

            await ProcessIfdAsync(handler, context, firstIfdOffset);
        }

        public static async Task ProcessIfdAsync(ITiffHandler handler, TiffReaderContext context, int ifdOffset)
        {
            try
            {
                if (!context.TryVisitIfd(ifdOffset, handler.Kind))
                    return;

                if (ifdOffset >= context.Reader.Length || ifdOffset < 0)
                {
                    handler.Error("Ignored IFD marked to start outside data segment");
                    return;
                }

                var dirTagCount = context.IsBigTiff
                    ? checked((int)context.Reader.GetUInt64(ifdOffset))
                    : context.Reader.GetUInt16(ifdOffset);

                if (!context.IsBigTiff && dirTagCount > 0xFF && (dirTagCount & 0xFF) == 0)
                {
                    dirTagCount >>= 8;
                    context = context.WithByteOrder(!context.Reader.IsMotorolaByteOrder);
                }

                var dirLength = context.IsBigTiff
                    ? 8 + 20 * dirTagCount + 8
                    : 2 + 12 * dirTagCount + 4;

                if (dirLength + ifdOffset > checked((int)context.Reader.Length))
                {
                    handler.Error("Illegally sized IFD");
                    return;
                }

                var inlineValueSize = context.IsBigTiff ? 8u : 4u;

                var invalidTiffFormatCodeCount = 0;
                for (var tagNumber = 0; tagNumber < dirTagCount; tagNumber++)
                {
                    var tagOffset = CalculateTagOffset(ifdOffset, tagNumber, context.IsBigTiff);

                    int tagId = context.Reader.GetUInt16(tagOffset);

                    var formatCode = (TiffDataFormatCode)context.Reader.GetUInt16(tagOffset + 2);

                    var componentCount = context.IsBigTiff
                        ? context.Reader.GetUInt64(tagOffset + 4)
                        : context.Reader.GetUInt32(tagOffset + 4);

                    var format = TiffDataFormat.FromTiffFormatCode(formatCode, context.IsBigTiff);

                    ulong byteCount;
                    if (format is null)
                    {
                        if (!handler.TryCustomProcessFormat(tagId, formatCode, componentCount, out byteCount))
                        {
                            handler.Error($"Invalid TIFF tag format code {(int)formatCode} for tag 0x{tagId:X4}");

                            if (++invalidTiffFormatCodeCount > 5)
                            {
                                handler.Error("Stopping processing as too many errors seen in TIFF IFD");
                                return;
                            }
                            continue;
                        }
                    }
                    else
                    {
                        byteCount = checked(componentCount * format.ComponentSizeBytes);
                    }

                    uint tagValueOffset;
                    if (byteCount > inlineValueSize)
                    {
                        tagValueOffset = context.IsBigTiff
                            ? checked((uint)context.Reader.GetUInt64(tagOffset + 12))
                            : context.Reader.GetUInt32(tagOffset + 8);

                        if (tagValueOffset + byteCount > checked((ulong)context.Reader.Length))
                        {
                            handler.Error("Illegal TIFF tag pointer offset");
                            continue;
                        }
                    }
                    else
                    {
                        tagValueOffset = context.IsBigTiff
                            ? checked((uint)tagOffset + 12)
                            : checked((uint)tagOffset + 8);
                    }

                    if (tagValueOffset > context.Reader.Length)
                    {
                        handler.Error("Illegal TIFF tag pointer offset");
                        continue;
                    }

                    if (tagValueOffset + byteCount > checked((ulong)context.Reader.Length))
                    {
                        handler.Error("Illegal number of bytes for TIFF tag data: " + byteCount);
                        continue;
                    }

                    var isIfdPointer = false;
                    if (byteCount == checked(4L * componentCount) || formatCode == TiffDataFormatCode.Ifd8)
                    {
                        for (ulong i = 0; i < componentCount; i++)
                        {
                            if (handler.TryEnterSubIfd(tagId))
                            {
                                isIfdPointer = true;
                                var subDirOffset = context.Reader.GetUInt32(checked((int)(tagValueOffset + i * 4)));
                                await ProcessIfdAsync(handler, context, (int)subDirOffset);
                            }
                        }
                    }

                    if (!isIfdPointer && !await handler.CustomProcessTagAsync(context, tagId, (int)tagValueOffset, (int)byteCount))
                    {
                        ProcessTag(handler, tagId, (int)tagValueOffset, (int)componentCount, formatCode, context.Reader);
                    }
                }

                var finalTagOffset = CalculateTagOffset(ifdOffset, dirTagCount, context.IsBigTiff);

                var nextIfdOffsetLong = context.IsBigTiff
                    ? context.Reader.GetUInt64(finalTagOffset)
                    : context.Reader.GetUInt32(finalTagOffset);

                if (nextIfdOffsetLong != 0 && nextIfdOffsetLong <= int.MaxValue)
                {
                    var nextIfdOffset = (int)nextIfdOffsetLong;

                    if (nextIfdOffset >= context.Reader.Length)
                    {
                        return;
                    }
                    else if (nextIfdOffset < ifdOffset)
                    {
                        return;
                    }

                    if (handler.HasFollowerIfd())
                    {
                        await ProcessIfdAsync(handler, context, nextIfdOffset);
                    }
                }
            }
            finally
            {
                handler.EndingIfd(in context);
            }
        }

        private static void ProcessTag(ITiffHandler handler, int tagId, int tagValueOffset, int componentCount, TiffDataFormatCode formatCode, IndexedReader reader)
        {
            switch (formatCode)
            {
                case TiffDataFormatCode.Undefined:
                    {
                        handler.SetByteArray(tagId, reader.GetBytes(tagValueOffset, componentCount));
                        break;
                    }
                case TiffDataFormatCode.String:
                    {
                        handler.SetString(tagId, reader.GetNullTerminatedStringValue(tagValueOffset, componentCount));
                        break;
                    }
                case TiffDataFormatCode.RationalS:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetRational(tagId, new Rational(reader.GetInt32(tagValueOffset), reader.GetInt32(tagValueOffset + 4)));
                        }
                        else if (componentCount > 1)
                        {
                            var array = new Rational[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = new Rational(reader.GetInt32(tagValueOffset + 8 * i), reader.GetInt32(tagValueOffset + 4 + 8 * i));
                            handler.SetRationalArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.RationalU:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetRational(tagId, new Rational(reader.GetUInt32(tagValueOffset), reader.GetUInt32(tagValueOffset + 4)));
                        }
                        else if (componentCount > 1)
                        {
                            var array = new Rational[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = new Rational(reader.GetUInt32(tagValueOffset + 8 * i), reader.GetUInt32(tagValueOffset + 4 + 8 * i));
                            handler.SetRationalArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Single:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetFloat(tagId, reader.GetFloat32(tagValueOffset));
                        }
                        else
                        {
                            var array = new float[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetFloat32(tagValueOffset + i * 4);
                            handler.SetFloatArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Double:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetDouble(tagId, reader.GetDouble64(tagValueOffset));
                        }
                        else
                        {
                            var array = new double[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetDouble64(tagValueOffset + i * 8);
                            handler.SetDoubleArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int8S:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt8S(tagId, reader.GetSByte(tagValueOffset));
                        }
                        else
                        {
                            var array = new sbyte[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetSByte(tagValueOffset + i);
                            handler.SetInt8SArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int8U:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt8U(tagId, reader.GetByte(tagValueOffset));
                        }
                        else
                        {
                            var array = new byte[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetByte(tagValueOffset + i);
                            handler.SetInt8UArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int16S:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt16S(tagId, reader.GetInt16(tagValueOffset));
                        }
                        else
                        {
                            var array = new short[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetInt16(tagValueOffset + i * 2);
                            handler.SetInt16SArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int16U:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt16U(tagId, reader.GetUInt16(tagValueOffset));
                        }
                        else
                        {
                            var array = new ushort[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetUInt16(tagValueOffset + i * 2);
                            handler.SetInt16UArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int32S:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt32S(tagId, reader.GetInt32(tagValueOffset));
                        }
                        else
                        {
                            var array = new int[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetInt32(tagValueOffset + i * 4);
                            handler.SetInt32SArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int32U:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt32U(tagId, reader.GetUInt32(tagValueOffset));
                        }
                        else
                        {
                            var array = new uint[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetUInt32(tagValueOffset + i * 4);
                            handler.SetInt32UArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int64S:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt64S(tagId, reader.GetInt64(tagValueOffset));
                        }
                        else
                        {
                            var array = new long[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetInt64(tagValueOffset + i * 8);
                            handler.SetInt64SArray(tagId, array);
                        }
                        break;
                    }
                case TiffDataFormatCode.Int64U:
                    {
                        if (componentCount == 1)
                        {
                            handler.SetInt64U(tagId, reader.GetUInt64(tagValueOffset));
                        }
                        else
                        {
                            var array = new ulong[componentCount];
                            for (var i = 0; i < componentCount; i++)
                                array[i] = reader.GetUInt64(tagValueOffset + i * 8);
                            handler.SetInt64UArray(tagId, array);
                        }
                        break;
                    }
                default:
                    {
                        handler.Error($"Invalid TIFF tag format code {(int)formatCode} for tag 0x{tagId:X4}");
                        break;
                    }
            }
        }

        private static int CalculateTagOffset(int ifdStartOffset, int entryNumber, bool isBigTiff)
        {
            return !isBigTiff
                ? ifdStartOffset + 2 + 12 * entryNumber
                : ifdStartOffset + 8 + 20 * entryNumber;
        }
    }
}
