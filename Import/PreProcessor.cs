using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser;

namespace Import
{
    internal class PreProcessor
    {
        private readonly RequirementsFileParser _parser;

        public PreProcessor(RequirementsFileParser parser)
        {
            _parser = parser;
        }

        public bool ConfirmProcessFiles(List<string> allFilesAndDirectoriesList, TargetSource targetSource)
        {
            var qualifiedFilesList = GetQualifiedFilesList(allFilesAndDirectoriesList, targetSource);

            if (qualifiedFilesList.Count == 0)
            {
                Console.WriteLine("No files are qualified.");
                return false;
            }

            Console.WriteLine($"Following {qualifiedFilesList.Count} file(s) are qualified for processing:");
            qualifiedFilesList.ForEach(Console.WriteLine);
            Console.Write("Do you want to proceed [y/n]?");
            var answer = Convert.ToChar(Console.Read());
            return answer == 'y' || answer == 'Y';
        }

        private List<string> GetQualifiedFilesList(List<string> fileDirectoriesList, TargetSource targetSource)
        {
            var qualifiedFilesList = new List<string>();

            if (targetSource == TargetSource.File)
            {
                fileDirectoriesList.ForEach(x => { if (_parser.CheckIsFileQualified(x)) qualifiedFilesList.Add(x); });
            }
            else if (targetSource == TargetSource.Directory)
            {
                fileDirectoriesList.ForEach(x =>
                {
                    GetListOfFilesInDirectory(x).ForEach(y => { if (_parser.CheckIsFileQualified(y)) qualifiedFilesList.Add(y); });
                });
            }

            return qualifiedFilesList;
        }

        private List<string> GetListOfFilesInDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.GetFiles().ToList().Select(x => x.ToString()).ToList();
        }
    }
}
