using Parser;

namespace ImportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            RequirementsParser parser = new RequirementsParser();
            parser.AddFromDirectory(args[0]);
        }
    }
}
