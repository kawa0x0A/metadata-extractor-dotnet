using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.WebP;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MetadataExtractor
{
    public static partial class Metadata
    {
        [GeneratedRegex("[0-9A-F]{8}", RegexOptions.IgnoreCase | RegexOptions.Compiled, "ja-JP")]
        private static partial Regex HashRegex();

        private static async Task<FileParameters?> ReadPngMetadataFromStreamAsync(Stream stream)
        {
            FileParameters? fileParameters = null;

            IReadOnlyList<MetadataExtractor.Directory> directories = await PngMetadataReader.ReadMetadataAsync(stream);

            var ihdrDirectory = directories.FirstOrDefault(d => d.Name == "PNG-IHDR");

            const string RootDirectoryName = "PNG-tEXt";
            const string RootTagName = "Textual Data";

            if (TryFindTag(directories, RootDirectoryName, RootTagName, out var tag))
            {
                if ((tag is not null) && (tag.Description is not null))
                {
                    if (tag.Description == "Software: NovelAI")
                    {
                        fileParameters = ReadNovelAIParameters(tag, ihdrDirectory!);
                    }
                    else if (tag.Description.StartsWith("Dream: "))
                    {
                        fileParameters = ReadInvokeAIParameters(tag);
                    }
                    else if (tag.Description.StartsWith("sd-metadata: "))
                    {
                        fileParameters = ReadInvokeAIParametersNew(tag);
                    }
                    else if (tag.Description.StartsWith("invokeai_metadata: "))
                    {
                        fileParameters = ReadInvokeAIParameters2(tag);
                    }
                    else if (tag.Description.StartsWith("prompt: ") && tag.Description["prompt: ".Length..].Trim().StartsWith('{'))
                    {
                        fileParameters = ReadComfyUIParameters(tag);
                    }
                    else if (tag.Description.StartsWith("prompt: ") && !tag.Description["prompt: ".Length..].Trim().StartsWith('{'))
                    {
                        fileParameters = ReadEasyDiffusionParameters(tag);
                    }
                    else
                    {
                        TryFindTag(directories, "Exif SubIFD", "User Comment", out var exifTag);
                        TryFindTag(directories, "PNG-tEXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var textTag);
                        TryFindTag(directories, "PNG-iTXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var itxtTag);

                        fileParameters = ReadAutomatic1111Parameters(exifTag!, textTag!, itxtTag!);
                    }
                }
            }
            else if (TryFindTags(directories, RootDirectoryName, RootTagName, out var tags))
            {
                if (tags!.ContainsKey("Software: NovelAI"))
                {
                    fileParameters = ReadNovelAIParameters(tags.Values, ihdrDirectory!);
                }
            }

            if (fileParameters is not null && (fileParameters.Width == 0 || fileParameters.Height == 0))
            {
                var imageWidthTag = ihdrDirectory!.Tags.SingleOrDefault(tag => tag.Name == "Image Width");
                var imageHeightTag = ihdrDirectory!.Tags.SingleOrDefault(tag => tag.Name == "Image Height");

                if ((imageWidthTag is not null) && (imageWidthTag.Description is not null))
                {
                    fileParameters.Width = int.Parse(imageWidthTag.Description);
                }

                if ((imageHeightTag is not null) && (imageHeightTag.Description is not null))
                {
                    fileParameters.Height = int.Parse(imageHeightTag.Description);
                }
            }

            return fileParameters;
        }

        private static async Task<FileParameters?> ReadJpgMetadataFromStreamAsync(Stream stream)
        {
            var directories = await JpegMetadataReader.ReadMetadataAsync(stream);

            TryFindTag(directories, "Exif SubIFD", "User Comment", out var exifTag);
            TryFindTag(directories, "PNG-tEXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var textTag);
            TryFindTag(directories, "PNG-iTXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var itxtTag);

            return ReadAutomatic1111Parameters(exifTag!, textTag!, itxtTag!);
        }

        private static async Task<FileParameters?> ReadWebpMetadataFromStreamAsync(Stream stream)
        {
            FileParameters? fileParameters = null;

            var directories = await WebPMetadataReader.ReadMetadataAsync(stream);

            TryFindTag(directories, "Exif SubIFD", "User Comment", out var exifTag);

            if (TryFindTag(directories, "PNG-tEXt", "Textual Data", out var tag))
            {
                if ((tag is not null) && (tag.Description is not null))
                {
                    if (tag.Description.StartsWith("Dream: "))
                    {
                        fileParameters = ReadInvokeAIParameters(tag);
                    }
                    else if (tag.Description.StartsWith("sd-metadata: "))
                    {
                        fileParameters = ReadInvokeAIParametersNew(tag);
                    }
                    else if (tag.Description.StartsWith("invokeai_metadata: "))
                    {
                        fileParameters = ReadInvokeAIParameters2(tag);
                    }
                    else if (tag.Description.StartsWith("prompt: ") && tag.Description["prompt: ".Length..].Trim().StartsWith('{'))
                    {
                        fileParameters = ReadComfyUIParameters(tag);
                    }
                    else if (tag.Description.StartsWith("prompt: ") && !tag.Description["prompt: ".Length..].Trim().StartsWith('{'))
                    {
                        fileParameters = ReadEasyDiffusionParameters(tag);
                    }
                    else
                    {
                        TryFindTag(directories, "PNG-tEXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var textTag);
                        TryFindTag(directories, "PNG-iTXt", "Textual Data", tag => tag.Description!.StartsWith("parameters:"), out var itxtTag);

                        fileParameters = ReadAutomatic1111Parameters(exifTag!, textTag!, itxtTag!);
                    }
                }
            }

            return fileParameters;
        }

        public static async Task<FileParameters> ReadFromStreamAsync(string extension, Stream stream)
        {
            FileParameters? fileParameters = null;

            switch (extension)
            {
                case ".png":
                    fileParameters = await ReadPngMetadataFromStreamAsync(stream);
                    break;

                case ".jpg" or ".jpeg":
                    fileParameters = await ReadJpgMetadataFromStreamAsync(stream);
                    break;

                case ".webp":
                    fileParameters = await ReadWebpMetadataFromStreamAsync(stream);
                    break;
            }

            fileParameters ??= new FileParameters()
            {
                NoMetadata = true
            };

            return fileParameters;
        }

        private static bool IsStableDiffusion(string metadata)
        {
            return metadata.Contains("\nWidth:") && metadata.Contains("\nHeight:") && metadata.Contains("\nSeed:");
        }

        private static FileParameters? ReadInvokeAIParameters(MetadataExtractor.Tag tag)
        {
            if (tag is null)
            {
                return null;
            }

            var fp = new FileParameters();

            if (tag.Description is not null)
            {
                var command = tag.Description["Dream: ".Length..];
                var start = command.IndexOf('\"');
                var end = command.IndexOf('\"', start + 1);

                fp.Prompt = command.Substring(start + 1, end - start - 1);

                var others = command[(end + 1)..];
                var args = others.Split([' ']);

                for (var index = 0; index < args.Length; index++)
                {
                    var arg = args[index];
                    switch (arg)
                    {
                        case "-s":
                            fp.Steps = int.Parse(args[index + 1], CultureInfo.InvariantCulture);
                            index++;
                            break;
                        case "-S":
                            fp.Seed = long.Parse(args[index + 1], CultureInfo.InvariantCulture);
                            index++;
                            break;
                        case "-W":
                            fp.Width = int.Parse(args[index + 1], CultureInfo.InvariantCulture);
                            index++;
                            break;
                        case "-H":
                            fp.Height = int.Parse(args[index + 1], CultureInfo.InvariantCulture);
                            index++;
                            break;
                        case "-C":
                            fp.CFGScale = decimal.Parse(args[index + 1], CultureInfo.InvariantCulture);
                            index++;
                            break;
                        case "-A":
                            fp.Sampler = args[index + 1];
                            index++;
                            break;
                    }
                }

                fp.OtherParameters = $"Steps: {fp.Steps} Sampler: {fp.Sampler} CFG Scale: {fp.CFGScale} Size: {fp.Width}x{fp.Height}";
            }

            return fp;
        }

        private static FileParameters ReadEasyDiffusionParameters(MetadataExtractor.Tag tag)
        {
            var fp = new FileParameters();

            string GetTag(string key)
            {
                if ((tag is not null) && (tag.Description is not null))
                {
                    if (tag.Description.StartsWith($"{key}: "))
                    {
                        return tag.Description[$"{key}: ".Length..];
                    }
                }

                return string.Empty;
            }

            decimal GetDecimalTag(string key)
            {
                if ((tag is not null) && (tag.Description is not null))
                {
                    if (tag.Description.StartsWith($"{key}: "))
                    {
                        var value = tag.Description[$"{key}: ".Length..];

                        return decimal.Parse(value);
                    }
                }

                return 0m;
            }

            int GetIntTag(string key)
            {
                if ((tag is not null) && (tag.Description is not null))
                {
                    if (tag.Description.StartsWith($"{key}: "))
                    {
                        var value = tag.Description[$"{key}: ".Length..];

                        return int.Parse(value);
                    }
                }

                return 0;
            }

            fp.Prompt = GetTag("prompt")!;
            fp.NegativePrompt = GetTag("negative_prompt")!;
            fp.Width = GetIntTag("width");
            fp.Height = GetIntTag("height");
            fp.Steps = GetIntTag("num_inference_steps");
            fp.CFGScale = GetDecimalTag("guidance_scale");
            fp.Seed = GetIntTag("seed");
            fp.Sampler = GetTag("sampler_name")!;
            fp.Model = GetTag("use_stable_diffusion_model")!;

            fp.OtherParameters = $"Steps: {fp.Steps} Sampler: {fp.Sampler} CFG Scale: {fp.CFGScale} Seed: {fp.Seed} Size: {fp.Width}x{fp.Height}";

            return fp;
        }

        private static FileParameters? ReadComfyUIParameters(MetadataExtractor.Tag tag)
        {
            if (tag is null)
            {
                return null;
            }

            var fp = new FileParameters();
            var json = (tag.Description is null) ? "" : tag.Description["prompt: ".Length..];

            var root = JsonDocument.Parse(json);
            var nodes = root.RootElement.EnumerateObject().ToDictionary(o => o.Name, o => o.Value);

            var isSDXL = false;

            var ksampler = nodes.Values.SingleOrDefault(o =>
            {
                if (o.TryGetProperty("class_type", out var element))
                {
                    return element.GetString() == "KSampler";
                }

                return false;
            });


            if (ksampler.ValueKind == JsonValueKind.Undefined)
            {
                ksampler = nodes.Values.FirstOrDefault(o =>
                {
                    if (o.TryGetProperty("class_type", out var element))
                    {
                        return element.GetString() == "KSamplerAdvanced";
                    }

                    return false;
                });

                if (ksampler.ValueKind != JsonValueKind.Undefined)
                {
                    isSDXL = true;
                }
            }

            var image = ksampler.GetProperty("inputs");

            if (image.TryGetProperty("positive", out var positive))
            {
                var promptIndex = positive.EnumerateArray().First().GetString();
                var promptObject = nodes[promptIndex!].GetProperty("inputs");
                if (isSDXL)
                {
                    fp.Prompt = promptObject.GetProperty("text_g").GetString()!;
                }
                else
                {
                    fp.Prompt = promptObject.GetProperty("text").GetString()!;
                }
            }

            if (image.TryGetProperty("negative", out var negative))
            {
                var promptIndex = negative.EnumerateArray().First().GetString();
                var promptObject = nodes[promptIndex!].GetProperty("inputs");
                if (isSDXL)
                {
                    fp.NegativePrompt = promptObject.GetProperty("text_g").GetString()!;
                }
                else
                {
                    fp.NegativePrompt = promptObject.GetProperty("text").GetString()!;
                }
            }

            if (image.TryGetProperty("latent_image", out var latent_image))
            {
                var index = latent_image.EnumerateArray().First().GetString();
                var promptObject = nodes[index!].GetProperty("inputs");
                var hasWidth = promptObject.TryGetProperty("width", out var widthObject);
                var hasHeight = promptObject.TryGetProperty("height", out var heightObject);

                if (hasWidth && hasHeight)
                {
                    fp.Width = widthObject.GetInt32();
                    fp.Height = heightObject.GetInt32();
                }
            }

            fp.Steps = image.GetProperty("steps").GetInt32();
            fp.CFGScale = image.GetProperty("cfg").GetDecimal();

            if (isSDXL)
            {
                fp.Seed = image.GetProperty("noise_seed").GetInt64();
            }
            else
            {
                fp.Seed = image.GetProperty("seed").GetInt64();
            }

            fp.Sampler = image.GetProperty("sampler_name").GetString()!;

            fp.OtherParameters = $"Steps: {fp.Steps} Sampler: {fp.Sampler} CFG Scale: {fp.CFGScale} Seed: {fp.Seed} Size: {fp.Width}x{fp.Height}";

            return fp;
        }

        private static FileParameters? ReadInvokeAIParametersNew(MetadataExtractor.Tag tag)
        {
            if (tag is null)
            {
                return null;
            }

            var fp = new FileParameters();
            var json = (tag.Description is null) ? "" : tag.Description["sd-metadata: ".Length..];
            var root = JsonDocument.Parse(json);
            var image = root.RootElement.GetProperty("image");
            var prompt = image.GetProperty("prompt");

            if (prompt.ValueKind == JsonValueKind.Array)
            {
                var promptArrayEnumerator = prompt.EnumerateArray();
                promptArrayEnumerator.MoveNext();
                var promptObject = promptArrayEnumerator.Current;
                fp.Prompt = promptObject.GetProperty("prompt").GetString()!;
                fp.PromptStrength = promptObject.GetProperty("weight").GetDecimal();
            }
            else if (prompt.ValueKind == JsonValueKind.String)
            {
                fp.Prompt = prompt.GetString()!;
            }

            fp.ModelHash = root.RootElement.GetProperty("model_hash").GetString()!;
            fp.Steps = image.GetProperty("steps").GetInt32();
            fp.CFGScale = image.GetProperty("cfg_scale").GetDecimal();
            fp.Height = image.GetProperty("height").GetInt32();
            fp.Width = image.GetProperty("width").GetInt32();
            fp.Seed = image.GetProperty("seed").GetInt64();
            fp.Sampler = image.GetProperty("sampler").GetString()!;

            fp.OtherParameters = $"Steps: {fp.Steps} Sampler: {fp.Sampler} CFG Scale: {fp.CFGScale} Seed: {fp.Seed} Size: {fp.Width}x{fp.Height}";

            return fp;
        }

        private static FileParameters? ReadInvokeAIParameters2(MetadataExtractor.Tag tag)
        {
            if (tag is null)
            {
                return null;
            }

            var fp = new FileParameters();
            var json = (tag.Description is null) ? "" : tag.Description["invokeai_metadata: ".Length..];
            var root = JsonDocument.Parse(json);
            var image = root.RootElement;

            fp.Prompt = image.GetProperty("positive_prompt").GetString()!;
            fp.NegativePrompt = image.GetProperty("negative_prompt").GetString()!;
            fp.Steps = image.GetProperty("steps").GetInt32();
            fp.CFGScale = image.GetProperty("cfg_scale").GetDecimal();
            fp.Height = image.GetProperty("height").GetInt32();
            fp.Width = image.GetProperty("width").GetInt32();
            fp.Seed = image.GetProperty("seed").GetInt64();
            fp.Sampler = image.GetProperty("scheduler").GetString()!;

            fp.OtherParameters = $"Steps: {fp.Steps} Sampler: {fp.Sampler} CFG Scale: {fp.CFGScale} Seed: {fp.Seed} Size: {fp.Width}x{fp.Height}";

            return fp;
        }

        private static FileParameters ReadNovelAIParameters(MetadataExtractor.Tag tag, MetadataExtractor.Directory ihdrDirectory)
        {
            var fileParameters = new FileParameters();

            if ((tag is not null) && (tag.Description is not null))
            {
                if (tag.Description.StartsWith("Description:"))
                {
                    fileParameters.Prompt = tag.Description["Description: ".Length..];
                }

                if (tag.Description.StartsWith("Source:"))
                {
                    var match = HashRegex().Match(tag.Description["Source: ".Length..]);
                    if (match.Success)
                    {
                        fileParameters.ModelHash = match.Groups[0].Value;
                    }
                }

                var propList = new List<string>();

                if (tag.Description.StartsWith("Comment:"))
                {
                    var json = JsonDocument.Parse(tag.Description["Comment: ".Length..]);

                    fileParameters.Steps = json.RootElement.GetProperty("steps").GetInt32();
                    fileParameters.Sampler = json.RootElement.GetProperty("sampler").GetString()!;
                    fileParameters.Seed = json.RootElement.GetProperty("seed").GetInt64();
                    fileParameters.CFGScale = json.RootElement.GetProperty("scale").GetDecimal();

                    var properties = json.RootElement.EnumerateObject();

                    foreach (var property in properties)
                    {
                        if (property.Name != "uc")
                        {
                            propList.Add($"{property.Name}: {property.Value}");
                        }
                    }


                    fileParameters.NegativePrompt = json.RootElement.GetProperty("uc").GetString()!;
                }

                propList.Add($"model hash: {fileParameters.ModelHash}");

                fileParameters.OtherParameters = string.Join(", ", propList);
            }

            if (ihdrDirectory is not null)
            {
                foreach (var ptag in ihdrDirectory.Tags)
                {
                    if (ptag.Description is not null)
                    {
                        switch (ptag.Name)
                        {
                            case "Image Width":
                                fileParameters.Width = int.Parse(ptag.Description, CultureInfo.InvariantCulture);
                                break;
                            case "Image Height":
                                fileParameters.Height = int.Parse(ptag.Description, CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }
            }

            return fileParameters;
        }

        private static FileParameters ReadNovelAIParameters(IEnumerable<MetadataExtractor.Tag> tags, MetadataExtractor.Directory ihdrDirectory)
        {
            var fileParameters = new FileParameters();

            var descriptionTag = tags.SingleOrDefault(tag => ((tag.Description is not null) && tag.Description.StartsWith("Description: ")));

            if ((descriptionTag is not null) && (descriptionTag.Description is not null))
            {
                fileParameters.Prompt = descriptionTag.Description["Description: ".Length..];
            }

            var sourceTag = tags.SingleOrDefault(tag => ((tag.Description is not null) && tag.Description.StartsWith("Source:")));

            if ((sourceTag is not null) && (sourceTag.Description is not null))
            {
                var match = HashRegex().Match(sourceTag.Description["Source: ".Length..]);
                if (match.Success)
                {
                    fileParameters.ModelHash = match.Groups[0].Value;
                }
            }

            var propList = new List<string>();

            var commentTag = tags.SingleOrDefault(tag => ((tag.Description is not null) && tag.Description.StartsWith("Comment:")));

            if ((commentTag is not null) && (commentTag.Description is not null))
            {
                var json = JsonDocument.Parse(commentTag.Description["Comment: ".Length..]);

                fileParameters.Steps = json.RootElement.GetProperty("steps").GetInt32();
                fileParameters.Sampler = json.RootElement.GetProperty("sampler").GetString()!;
                fileParameters.Seed = json.RootElement.GetProperty("seed").GetInt64();
                fileParameters.CFGScale = json.RootElement.GetProperty("scale").GetDecimal();

                var properties = json.RootElement.EnumerateObject();

                foreach (var property in properties)
                {
                    if (property.Name != "uc")
                    {
                        propList.Add($"{property.Name}: {property.Value}");
                    }
                }


                fileParameters.NegativePrompt = json.RootElement.GetProperty("uc").GetString()!;
            }

            propList.Add($"model hash: {fileParameters.ModelHash}");

            fileParameters.OtherParameters = string.Join(", ", propList);

            if (ihdrDirectory is not null)
            {
                foreach (var ptag in ihdrDirectory.Tags)
                {
                    if ((ptag is not null) && (ptag.Description is not null))
                    {
                        switch (ptag.Name)
                        {
                            case "Image Width":
                                fileParameters.Width = int.Parse(ptag.Description, CultureInfo.InvariantCulture);
                                break;
                            case "Image Height":
                                fileParameters.Height = int.Parse(ptag.Description, CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }
            }

            return fileParameters;
        }

        private static FileParameters ReadStableDiffusionParameters(string data)
        {
            var fileParameters = new FileParameters();

            var parts = data.Split(['\n']);

            const string negativePromptKey = "Negative prompt: ";
            const string modelKey = "Stable Diffusion model: ";
            const string widthKey = "Width: ";

            fileParameters.Parameters = data;

            var state = 0;

            fileParameters.Prompt = "";
            fileParameters.NegativePrompt = "";

            string otherParam = "";

            foreach (var part in parts)
            {
                var isNegativePrompt = part.StartsWith(negativePromptKey, StringComparison.InvariantCultureIgnoreCase);
                var isModel = part.StartsWith(modelKey, StringComparison.InvariantCultureIgnoreCase);
                var isWidth = part.StartsWith(widthKey, StringComparison.InvariantCultureIgnoreCase);

                if (isWidth)
                {
                    state = 1;
                }
                else if (isNegativePrompt)
                {
                    state = 2;
                }
                else if (isModel)
                {
                    state = 3;
                }

                switch (state)
                {
                    case 0:
                        fileParameters.Prompt += part + "\n";
                        break;
                    case 2:
                        if (isNegativePrompt)
                        {
                            fileParameters.NegativePrompt += part[negativePromptKey.Length..];
                        }
                        else
                        {
                            fileParameters.NegativePrompt += part + "\n";
                        }
                        break;
                    case 1:


                        var subParts = part.Split([',']);
                        foreach (var keyValue in subParts)
                        {
                            var kvp = keyValue.Split([':']);
                            switch (kvp[0].Trim())
                            {
                                case "Steps":
                                    fileParameters.Steps = int.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Sampler":
                                    fileParameters.Sampler = kvp[1].Trim();
                                    break;
                                case "Guidance Scale":
                                    fileParameters.CFGScale = decimal.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Seed":
                                    fileParameters.Seed = long.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Width":
                                    fileParameters.Width = int.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Height":
                                    fileParameters.Height = int.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Prompt Strength":
                                    fileParameters.PromptStrength = decimal.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                    //case "Model hash":
                                    //    fileParameters.ModelHash = kvp[1].Trim();
                                    //    break;
                                    //case "Batch size":
                                    //    fileParameters.BatchSize = int.Parse(kvp[1].Trim());
                                    //    break;
                                    //case "Hypernet":
                                    //    fileParameters.HyperNetwork = kvp[1].Trim();
                                    //    break;
                                    //case "Hypernet strength":
                                    //    fileParameters.HyperNetworkStrength = decimal.Parse(kvp[1].Trim());
                                    //    break;
                                    //case "aesthetic_score":
                                    //    fileParameters.AestheticScore = decimal.Parse(kvp[1].Trim());
                                    //    break;
                            }
                        }

                        otherParam += part + "\n";

                        break;
                    case 3:
                        otherParam += part + "\n";
                        break;
                }

            }

            fileParameters.OtherParameters = otherParam;

            return fileParameters;
        }

        private static FileParameters ReadA111Parameters(string data)
        {
            var fileParameters = new FileParameters();

            var parts = data.Split(['\n']);

            const string parametersKey = "parameters:";
            const string negativePromptKey = "Negative prompt:";
            const string stepsKey = "Steps:";

            fileParameters.Parameters = data;

            var state = 0;

            fileParameters.Prompt = "";
            fileParameters.NegativePrompt = "";

            foreach (var part in parts)
            {
                var isNegativePrompt = part.StartsWith(negativePromptKey, StringComparison.InvariantCultureIgnoreCase);
                var isPromptStart = part.StartsWith(parametersKey, StringComparison.InvariantCultureIgnoreCase);
                var isOther = part.StartsWith(stepsKey, StringComparison.InvariantCultureIgnoreCase);

                if (isPromptStart)
                {
                    state = 0;
                }
                else if (isNegativePrompt)
                {
                    state = 1;
                }
                else if (isOther)
                {
                    state = 2;
                }

                switch (state)
                {
                    case 0:
                        if (isPromptStart)
                        {
                            fileParameters.Prompt += part[(parametersKey.Length + 1)..] + "\n";
                        }
                        else
                        {
                            fileParameters.Prompt += part + "\n";
                        }
                        break;
                    case 1:
                        if (isNegativePrompt)
                        {
                            fileParameters.NegativePrompt += part[negativePromptKey.Length..];
                        }
                        else
                        {
                            fileParameters.NegativePrompt += part + "\n";
                        }
                        break;
                    case 2:

                        fileParameters.OtherParameters = part;

                        var subParts = part.Split([',']);
                        foreach (var keyValue in subParts)
                        {
                            var kvp = keyValue.Split([':']);
                            switch (kvp[0].Trim())
                            {
                                case "Steps":
                                    fileParameters.Steps = int.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Sampler":
                                    fileParameters.Sampler = kvp[1].Trim();
                                    break;
                                case "CFG scale":
                                    fileParameters.CFGScale = decimal.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Seed":
                                    fileParameters.Seed = long.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Size":
                                    var size = kvp[1].Split(['x']);
                                    fileParameters.Width = int.Parse(size[0].Trim(), CultureInfo.InvariantCulture);
                                    fileParameters.Height = int.Parse(size[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Model hash":
                                    fileParameters.ModelHash = kvp[1].Trim();
                                    break;
                                case "Model":
                                    fileParameters.Model = kvp[1].Trim();
                                    break;
                                case "Batch size":
                                    fileParameters.BatchSize = int.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "Hypernet":
                                    fileParameters.HyperNetwork = kvp[1].Trim();
                                    break;
                                case "Hypernet strength":
                                    fileParameters.HyperNetworkStrength = decimal.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                                case "aesthetic_score":
                                case "Score":
                                    fileParameters.AestheticScore = decimal.Parse(kvp[1].Trim(), CultureInfo.InvariantCulture);
                                    break;
                            }
                        }

                        state = 3;

                        break;
                }

            }

            return fileParameters;
        }

        private static FileParameters? ReadAutomatic1111Parameters(MetadataExtractor.Tag exifTag, MetadataExtractor.Tag textTag, MetadataExtractor.Tag itxtTag)
        {
            FileParameters? fileParameters = null;

            if ((exifTag is not null) && (exifTag.Description is not null))
            {
                fileParameters = ReadA111Parameters(exifTag.Description);
            }
            else if ((textTag is not null) && (textTag.Description is not null))
            {
                fileParameters = ReadA111Parameters(textTag.Description);
            }
            else if ((itxtTag is not null) && (itxtTag.Description is not null))
            {
                fileParameters = ReadA111Parameters(itxtTag.Description);
            }

            if ((textTag is not null) && (textTag.Description is not null) && textTag.Description.StartsWith("Score:"))
            {
                fileParameters ??= new FileParameters();
                fileParameters.AestheticScore = decimal.Parse(textTag.Description.AsSpan("Score:".Length), CultureInfo.InvariantCulture);
                fileParameters.OtherParameters ??= $"aesthetic_score: {fileParameters.AestheticScore}";
            }

            if ((textTag is not null) && (textTag.Description is not null) && textTag.Description.StartsWith("aesthetic_score:"))
            {
                fileParameters ??= new FileParameters();
                fileParameters.AestheticScore = decimal.Parse(textTag.Description.AsSpan("aesthetic_score:".Length), CultureInfo.InvariantCulture);
                fileParameters.OtherParameters ??= $"aesthetic_score: {fileParameters.AestheticScore}";
            }

            return fileParameters;
        }

        private static bool TryFindTag(IEnumerable<MetadataExtractor.Directory> directories, string DirectoryName, string tagName, out MetadataExtractor.Tag? foundTag)
        {
            var targetDirectory = directories.Where(directory => directory.Name == DirectoryName);

            if (targetDirectory is null)
            {
                foundTag = null;
                return false;
            }

            var tags = targetDirectory.SelectMany(directory => directory.Tags).Where(tag => tag.Name == tagName);

            if (tags.Count() == 1)
            {
                foundTag = tags.Single();
            }
            else
            {
                foundTag = null;
            }

            return foundTag is not null;
        }

        private static bool TryFindTag(IEnumerable<MetadataExtractor.Directory> directories, string DirectoryName, string tagName, Func<MetadataExtractor.Tag, bool> matchTag, out MetadataExtractor.Tag? foundTag)
        {
            var targetDirectory = directories.SingleOrDefault(directory => directory.Name == DirectoryName);

            if (targetDirectory is null)
            {
                foundTag = null;
                return false;
            }

            foundTag = targetDirectory.Tags.Where(tag => tag.Name == tagName).SingleOrDefault(tag => matchTag(tag))!;

            return foundTag is not null;
        }

        private static bool TryFindTags(IEnumerable<MetadataExtractor.Directory> directories, string DirectoryName, string tagName, out Dictionary<string, MetadataExtractor.Tag>? foundTag)
        {
            var targetDirectory = directories.Where(directory => directory.Name == DirectoryName);

            if (targetDirectory is null)
            {
                foundTag = null;
                return false;
            }

            foundTag = targetDirectory.SelectMany(directory => directory.Tags).Where(tag => tag.Name == tagName).ToList().ToDictionary(tag => tag.Description!, tag => tag);

            return foundTag.Count > 0;
        }
    }
}