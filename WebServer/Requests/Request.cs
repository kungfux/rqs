namespace WebServer.Requests
{
    public class Request
    {
        public int? Length { get; private set; }
        public string Url { get; private set; }
        public Method Method { get; private set; }
        public Target Target { get; private set; }

        public Request()
        {

        }

        internal Request(int? length, string url, Method method, Target target)
        {
            Length = length;
            Url = url;
            Method = method;
            Target = target;
        }
    }
}
