namespace WebServer
{
    public interface IConfiguration
    {
        int Port { get; set; }
        string RootPath { get; set; }
        string IndexFile { get; set; }
    }
}
