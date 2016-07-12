using System;

namespace Parser.Parsers.Requirements
{
    internal interface IFileParser : IParser
    {
        string AcceptableFileMask { get; }

        void ParseFile(string filePath);
        void ParseDirectory(string path);

        event Action<ProgressEventArgs> OnUpdateStatus;
        void UpdateStatus(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete);
    }
}
