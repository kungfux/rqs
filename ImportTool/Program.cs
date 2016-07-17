using Parser;
using System;
using System.Reflection;
using Parser.Parsers.Requirements;

namespace ImportTool
{
    internal static class Program
    {
        private const string MessageFilesCanBeSkipped = "Note: Not qualified file(s) by extension will be skipped from import.";
        private const string MessageFilesImported = "file(s) have been processed.";

        private static int _filesProcessed;
        private static string _lastProcessedFile;

        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var options = new Options();

            if (args.Length == 0)
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            CommandLine.Parser.Default.ParseArguments(args, options);

            if (string.IsNullOrEmpty(options.File) && string.IsNullOrEmpty(options.Directory))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            WriteGreetings();

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


            WriteGoodBye();
        }

        private static void WriteGreetings()
        {
            Console.WriteLine(
                $"{GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title)} {Assembly.GetExecutingAssembly().GetName().Version} {Environment.NewLine}" +
                $"{GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright)}");
            Console.WriteLine();

            Console.WriteLine(MessageFilesCanBeSkipped);
        }

        private static void WriteGoodBye()
        {
            Console.WriteLine();
            Console.WriteLine($"{_filesProcessed} {MessageFilesImported}");
        }

        private static string GetAssemblyAttribute<T>(Func<T, string> value)
            where T : Attribute
        {
            var attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }

        private static void Parser_OnUpdateStatus(ProgressEventArgs e)
        {
            if (_lastProcessedFile != e.FileBeingProcessed)
                _filesProcessed++;

            if (!string.IsNullOrEmpty(_lastProcessedFile))
            {
                Console.Write(_lastProcessedFile != e.FileBeingProcessed ? "\r\n" : "\r");
            }

            _lastProcessedFile = e.FileBeingProcessed;

            Console.Write($"Processing file: {e.FileBeingProcessed} - {e.RecordNumberBeingProcessed} record(s) [{e.PercentsComplete}%]");
        }
    }
}
