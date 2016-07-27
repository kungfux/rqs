using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Import
{
    internal class Options
    {
        [Option('f', "file", HelpText = "File to be imported")]
        public string InputFile { get; set; }

        [Option('d', "directory", HelpText = "Directory with files to be imported")]
        public string InputDirectory { get; set; }

        [ValueList(typeof(List<string>))]
        public List<string> OtherInputFiles { get; set; }

        [Option('a', "auto-accept", HelpText = "Accept processing automatically without prompt")]
        public bool AutoAccept { get; set; }

        [Option('s', "skip-check", HelpText = "Skip checking of repeated file import")]
        public bool SkipFileCheck { get; set; }

        [Option('n', "no-save", HelpText = "Do not save processed results")]
        public bool DoNotSave { get; set; }

        [Option('t', "template", HelpText = "Template file")]
        public string TemplateFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
