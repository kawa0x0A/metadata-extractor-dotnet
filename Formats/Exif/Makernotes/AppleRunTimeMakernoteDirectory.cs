﻿using MetadataExtractor.Formats.Apple;

namespace MetadataExtractor.Formats.Exif.Makernotes;

public sealed class AppleRunTimeMakernoteDirectory : Directory
{
    public const int TagFlags = 1;
    public const int TagEpoch = 2;
    public const int TagScale = 3;
    public const int TagValue = 4;

    private static readonly Dictionary<int, string> _tagNameMap = [];

    static AppleRunTimeMakernoteDirectory()
    {
        _tagNameMap[TagFlags] = "Flags";
        _tagNameMap[TagEpoch] = "Epoch";
        _tagNameMap[TagScale] = "Scale";
        _tagNameMap[TagValue] = "Value";
    }

    public AppleRunTimeMakernoteDirectory() : base(_tagNameMap)
    {
        SetDescriptor(new AppleRunTimeMakernoteDescriptor(this));
    }

    public override string Name => "Apple Run Time";

    public static async Task<AppleRunTimeMakernoteDirectory> ParseAsync(byte[] bytes)
    {
        AppleRunTimeMakernoteDirectory directory = new();

        if (!BplistReader.IsValid(bytes))
        {
            directory.AddError("Input array is not a bplist.");
        }
        else
        {
            try
            {
                await ProcessAppleRunTimeAsync();
            }
            catch (IOException ex)
            {
                directory.AddError($"Error processing {nameof(AppleRunTimeMakernoteDirectory)}: {ex.Message}");
            }
        }

        return directory;

        async Task ProcessAppleRunTimeAsync()
        {
            var results = await BplistReader.ParseAsync(bytes);

            var entrySet = results.GetTopObject();

            if (entrySet is not null)
            {
                Dictionary<string, object> values = new(entrySet.Count);

                foreach (var pair in entrySet)
                {
                    var key = (string)results.Get(pair.Key);
                    var value = results.Get(pair.Value);

                    values[key] = value;
                }

                if (values.TryGetValue("flags", out var flagsObject))
                {
                    if (flagsObject is byte flags)
                    {
                        if ((flags & 0x1) == 0x1)
                        {
                            directory.Set(TagFlags, flags);
                            directory.Set(TagEpoch, (byte)values["epoch"]);
                            directory.Set(TagScale, (int)values["timescale"]);
                            directory.Set(TagValue, (long)values["value"]);
                        }
                    }
                    else if (flagsObject is string flagsString)
                    {
                        var parsedFlags = byte.Parse(flagsString);
                        if ((parsedFlags & 0x1) == 0x1)
                        {
                            directory.Set(TagFlags, parsedFlags);
                            directory.Set(TagEpoch, byte.Parse((string)values["epoch"]));
                            directory.Set(TagScale, int.Parse((string)values["timescale"]));
                            directory.Set(TagValue, long.Parse((string)values["value"]));
                        }
                    }
                }
            }
        }
    }
}
