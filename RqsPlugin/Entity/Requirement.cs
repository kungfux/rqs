namespace RqsPlugin.Entity
{
    public class Requirement
    {
        public string ID;             // FR ID number (from file)
        public string TMSTask;        // FR TMS task number (from file)
        public string CCP;            // FR CCP (from file)
        public string Text;           // FR text (from file)
        public string Status;         // FR status (from file)
        public string Created;        // FR created date (from file)
        public string Modified;       // FR modified date (from file)
        public string ObjectNumber;   // FR Object Number
        public string Source;         // File name which contain this FR
        public string Project;        // To what project refers
    }
}
