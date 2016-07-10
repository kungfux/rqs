using Parser;
using System;

namespace ImportTool
{
    internal class Program
    {
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

            var parser = new RequirementsParser();
            parser.OnUpdateStatus += Parser_OnUpdateStatus;

            if (!string.IsNullOrEmpty(options.File))
            {
                parser.AddFromExcel(options.File);
            }

            if (!string.IsNullOrEmpty(options.Directory))
            {
                parser.AddFromDirectory(options.Directory);
            }

            Console.WriteLine();
        }

        private static string _lastProcessedFile;
        private static void Parser_OnUpdateStatus(object sender, ProgressEventArgs e)
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
