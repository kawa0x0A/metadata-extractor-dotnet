namespace MetadataExtractor.Formats.Tiff
{
    public interface ITiffHandler
    {
        TiffStandard ProcessTiffMarker(ushort marker);

        object Kind { get; }

        bool TryEnterSubIfd(int tagType);

        bool HasFollowerIfd();

        void EndingIfd(in TiffReaderContext context);

        Task<bool> CustomProcessTagAsync(TiffReaderContext context, int tagId, int valueOffset, int byteCount);

        bool TryCustomProcessFormat(int tagId, TiffDataFormatCode formatCode, ulong componentCount, out ulong byteCount);

        void Warn(string message);

        void Error(string message);

        void SetByteArray(int tagId, byte[] bytes);

        void SetString(int tagId, StringValue str);

        void SetRational(int tagId, Rational rational);

        void SetRationalArray(int tagId, Rational[] array);

        void SetFloat(int tagId, float float32);

        void SetFloatArray(int tagId, float[] array);

        void SetDouble(int tagId, double double64);

        void SetDoubleArray(int tagId, double[] array);

        void SetInt8S(int tagId, sbyte int8S);

        void SetInt8SArray(int tagId, sbyte[] array);

        void SetInt8U(int tagId, byte int8U);

        void SetInt8UArray(int tagId, byte[] array);

        void SetInt16S(int tagId, short int16S);

        void SetInt16SArray(int tagId, short[] array);

        void SetInt16U(int tagId, ushort int16U);

        void SetInt16UArray(int tagId, ushort[] array);

        void SetInt32S(int tagId, int int32S);

        void SetInt32SArray(int tagId, int[] array);

        void SetInt32U(int tagId, uint int32U);

        void SetInt32UArray(int tagId, uint[] array);

        void SetInt64S(int tagId, long int64S);

        void SetInt64SArray(int tagId, long[] array);

        void SetInt64U(int tagId, ulong int64U);

        void SetInt64UArray(int tagId, ulong[] array);
    }
}
