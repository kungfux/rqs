using System;

namespace Parser.Parsers.Requirements
{
    public class ProgressEventArgs : EventArgs
    {
        public string FileBeingProcessed { get; private set; }
        public int RecordNumberBeingProcessed { get; private set; }
        public int PercentsComplete { get; private set; }

        public ProgressEventArgs(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete)
        {
            FileBeingProcessed = fileBeingProcessed;
            RecordNumberBeingProcessed = recordNumberBeingProcessed;
            PercentsComplete = percentsComplete;
        }
    }
}
