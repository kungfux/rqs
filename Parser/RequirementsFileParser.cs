using System;
using System.Collections.Generic;
using Parser.Parsers.Requirements;

namespace Parser
{
    public class RequirementsFileParser
    {
        private readonly ICollection<IFileParser> _fileParsers;

        public RequirementsFileParser()
        {
            _fileParsers = new List<IFileParser>()
            {
                new ExcelParser()
            };
        }

        public void ParseFile(string filePath)
        {
            foreach (var fileParser in _fileParsers)
            {
                fileParser.ParseFile(filePath);
            }
        }

        public void ParseDirectory(string path)
        {
            foreach (var fileParser in _fileParsers)
            {
                fileParser.ParseDirectory(path);
            }
        }

        public event Action<ProgressEventArgs> OnUpdateStatus
        {
            add
            {
                foreach (var fileParser in _fileParsers)
                {
                    if (fileParser != null)
                        fileParser.OnUpdateStatus += value;
                }
            }
            remove
            {
                foreach (var fileParser in _fileParsers)
                {
                    if (fileParser != null)
                        fileParser.OnUpdateStatus -= value;
                }
            }
        }
    }
}
