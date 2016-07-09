using System;

namespace Parser
{
    public class ProgressEventArgs : EventArgs
    {
        public string FileBeingProcessed { get; private set; }
        public int RecordNumberBeingProcessed { get; private set; }

        public ProgressEventArgs(string fileBeingProcessed, int recordNumberBeingProcessed)
        {
            FileBeingProcessed = fileBeingProcessed;
            RecordNumberBeingProcessed = recordNumberBeingProcessed;
        }
    }
}
