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

        [Option('a', "auto-accept", HelpText = "Accept processing without prompt")]
        public bool AutoAccept { get; set; }

        [Option('s', "skip-check", HelpText = "Skip checking of repeated file import")]
        public bool SkipFileCheck { get; set; }

        [Option('n', "no-save", HelpText = "Do not save processed results")]
        public bool DoNotSave { get; set; }

        [Option('t', "template", HelpText = "Template file")]
        public string TemplateFile { get; set; }

        [Option('k', "keep", HelpText = "Keep stored data for the project being updated")]
        public bool KeepRequirements { get; set; }

        [Option('o', "overall-removing", HelpText = "Clear database before importing")]
        public bool OverallRemovingBeforeImporting { get; set; }

        [Option('i', "ignore-errors", HelpText = "Do not fail import process in case of errors")]
        public bool IgnoreErrors { get; set; }

        [Option('l', "log", HelpText = "Save summary report to file specified")]
        public string LogFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
