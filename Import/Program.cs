using Parser;
using System;
using System.Collections.Generic;
using System.Reflection;
using Parser.Parsers.Requirements;

namespace Import
{
    internal static class Program
    {
        private const string MessageFilesImported = "file(s) have been processed.";

        private static int _filesProcessed;
        private static string _lastProcessedFile;
        private static TargetSource _targetSource;

        private static readonly Options Options = new Options();

        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            ParseArguments(args);

            WriteGreetings();

            var parser = new RequirementsFileParser(Options.SkipFileCheck);
            parser.OnUpdateStatus += Parser_OnUpdateStatus;

            // Determinate specified arguments: files or directories?
            if (!string.IsNullOrEmpty(Options.InputFile))
            {
                _targetSource = TargetSource.File;
            }
            else if (!string.IsNullOrEmpty(Options.InputDirectory))
            {
                _targetSource = TargetSource.Directory;
            }

            // Combine all arguments
            var allFilesDirectories = CombineFileDirectoryArguments(Options);
            
            if (!Options.AutoAccept)
            {
                // Get comfirmation from user
                var preProcessor = new PreProcessor(parser);
                var confirmed = preProcessor.ConfirmProcessFiles(allFilesDirectories, _targetSource);

                if (!confirmed)
                {
                    return;
                }
            }

            switch (_targetSource)
            {
                case TargetSource.File:
                    allFilesDirectories.ForEach(parser.ParseFile);
                    break;
                case TargetSource.Directory:
                    allFilesDirectories.ForEach(parser.ParseDirectory);
                    break;
                default:
                    Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
                    break;
            }

            WriteGoodBye();
        }

        private static void ParseArguments(string[] args)
        {
            if (args.Length == 0)
            {
                FailAndExit(true);
            }

            CommandLine.Parser.Default.ParseArguments(args, Options);

            if (string.IsNullOrEmpty(Options.InputFile) && string.IsNullOrEmpty(Options.InputDirectory))
            {
                FailAndExit("Cannot proceed without both -f and -d options specified.");
            }

            if (!string.IsNullOrEmpty(Options.InputFile) && !string.IsNullOrEmpty(Options.InputDirectory))
            {
                FailAndExit("Cannot proceed with both -f and -d options not specified.");
            }
        }

        private static void FailAndExit(string message)
        {
            FailAndExit(false, message);
        }

        private static void FailAndExit(bool displayUsage, string message = null)
        {
            if (displayUsage)
            {
                Console.WriteLine(Options.GetUsage());
            }
            else
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine(message);
                }
            }

            Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
        }

        private static List<string> CombineFileDirectoryArguments(Options options)
        {
            var combinedList = new List<string>();

            switch (_targetSource)
            {
                case TargetSource.File:
                    combinedList.Add(options.InputFile);
                    break;
                case TargetSource.Directory:
                    combinedList.Add(options.InputDirectory);
                    break;
                default:
                    return combinedList;
            }

            if (options.OtherInputFiles.Count > 0)
            {
                options.OtherInputFiles.ForEach(combinedList.Add);
            }

            return combinedList;
        }

        private static void WriteGreetings()
        {
            Console.WriteLine(
                $"{GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title)} {Assembly.GetExecutingAssembly().GetName().Version} {Environment.NewLine}" +
                $"{GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright)}");
            Console.WriteLine();
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
