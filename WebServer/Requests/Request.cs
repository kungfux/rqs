namespace WebServer.Requests
{
    public class Request
    {
        public int? Length { get; internal set; }
        public string Url { get; internal set; }
        public Method? Method { get; internal set; }
        public Target? Target { get; internal set; }

        internal Request()
        {

        }

        public override string ToString()
        {
            return $"Url: {Url} Method: {Method} Length: {Length} Target: {Target}";
        }
    }
}
