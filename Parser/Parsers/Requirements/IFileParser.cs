using System;

namespace Parser.Parsers.Requirements
{
    internal interface IFileParser : IParser
    {
        bool SkipCheck { get; set; }

        void ParseFile(string filePath);
        bool CheckIsParsedPreviously(string filePath);
        bool CheckIsFileQualified(string filePath);

        void ParseDirectory(string path);
        

        event Action<ProgressEventArgs> OnUpdateStatus;
        void UpdateStatus(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete);
    }
}
