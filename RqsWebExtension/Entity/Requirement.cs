namespace RqsWebExtension.Entity
{
    public class Requirement
    {
        public string Id;             // FR Id number (from file)
        public string TmsTask;        // FR TMS task number (from file)
        public string Ccp;            // FR Ccp (from file)
        public string Text;           // FR text (from file)
        public string Status;         // FR status (from file)
        public string Created;        // FR created date (from file)
        public string Modified;       // FR modified date (from file)
        public string ObjectNumber;   // FR Object Number
        public string Source;         // File name which contain this FR
        public string Project;        // To what project refers
    }
}
