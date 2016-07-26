using CommandLine;
using CommandLine.Text;

namespace Import
{
    internal class Options
    {
        [Option('f', "file", HelpText = "File to be imported")]
        public string File { get; set; }

        [Option('d', "directory", HelpText = "Directory with files to be imported")]
        public string Directory { get; set; }

        [Option('s', "skip-check", HelpText = "Skip checking of the hash")]
        public bool SkipHashCheck { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
