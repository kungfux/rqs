using Parser;
using System;
using Parser.Parsers.Requirements;

namespace ImportTool
{
    internal static class Program
    {
        private static string _lastProcessedFile;

        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var options = new Options();
            CommandLine.Parser.Default.ParseArguments(args, options);
            if (string.IsNullOrEmpty(options.File) && string.IsNullOrEmpty(options.Directory))
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            var parser = new RequirementsFileParser();
            parser.OnUpdateStatus += Parser_OnUpdateStatus;

            if (!string.IsNullOrEmpty(options.File))
            {
                parser.ParseFile(options.File);
            }

            if (!string.IsNullOrEmpty(options.Directory))
            {
                parser.ParseDirectory(options.Directory);
            }

            Console.WriteLine();
        }

        private static void Parser_OnUpdateStatus(ProgressEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastProcessedFile))
            {
                Console.Write(_lastProcessedFile != e.FileBeingProcessed ? "\r\n" : "\r");
            }
            _lastProcessedFile = e.FileBeingProcessed;
            Console.Write($"File: {e.FileBeingProcessed} Record: {e.RecordNumberBeingProcessed} Completed: {e.PercentsComplete}%");
        }
    }
}
