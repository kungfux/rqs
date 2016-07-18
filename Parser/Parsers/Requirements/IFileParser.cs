using System;

namespace Parser.Parsers.Requirements
{
    internal interface IFileParser : IParser
    {
        string AcceptableFileMask { get; }
        bool IsOverrideMode { get; set; }

        void ParseFile(string filePath);
        void ParseDirectory(string path);
        bool IsParsedPreviously(string filePath);

        event Action<ProgressEventArgs> OnUpdateStatus;
        void UpdateStatus(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete);
    }
}
