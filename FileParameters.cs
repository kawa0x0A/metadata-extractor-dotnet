﻿namespace MetadataExtractor
{
    public class FileParameters
    {
        public string Prompt { get; set; } = "";
        public string NegativePrompt { get; set; } = "";
        public int Steps { get; set; }
        public string Sampler { get; set; } = "";
        public decimal CFGScale { get; set; }
        public long Seed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ModelHash { get; set; } = "";
        public string Model { get; set; } = "";
        public int BatchSize { get; set; }
        public int BatchPos { get; set; }
        public string OtherParameters { get; set; } = "";
        public string Parameters { get; set; } = "";
        public decimal? AestheticScore { get; set; }
        public string HyperNetwork { get; set; } = "";
        public decimal? HyperNetworkStrength { get; set; }
        public int? ClipSkip { get; set; }
        public int? ENSD { get; set; }
        public decimal? PromptStrength { get; set; }
        public bool NoMetadata { get; set; }
    }
}