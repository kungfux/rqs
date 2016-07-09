using CommandLine;
using CommandLine.Text;

namespace ImportTool
{
    internal class Options
    {
        [Option('f', "file", HelpText = "Single Excel file to be imported")]
        public string File { get; set; }

        [Option('d', "directory", HelpText = "Directory with Excel files to be imported")]
        public string Directory { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
