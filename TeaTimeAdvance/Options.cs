using CommandLine;

namespace TeaTimeAdvance
{
    class Options
    {
        [Option("bios", Required = true, HelpText = "BIOS to load")]
        public string BiosPath { get; set; }

        [Value(0, MetaName = "input", HelpText = "Input game to load.", Required = true)]
        public string GamePath { get; set; }
    }
}
