using FuseControlSystem.Models;
using log4net;
using System.Windows;

namespace FuseControlSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            // Log levels:
            //  Debug - to log info useful for debugging
            //  Error - to log not critical exceptions
            //  Fatal - to log critical exceptions
            //  Info -  to log business logic info useful for elaborations
            //  Warn -  to log not critical situations not handled by implementation
            log4net.Config.XmlConfigurator.Configure();

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LanguageDictionary.Instance.FindString("Init LanguageDictionary");
        }
    }
}
