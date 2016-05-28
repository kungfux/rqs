using Fuse.GUI.Models;
using log4net;
using System;
using System.Threading;
using System.Windows;

namespace Fuse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            // Log levels:
            //  Debug - to log info useful for debugging
            //  Error - to log not critical exceptions
            //  Fatal - to log critical exceptions
            //  Info -  to log business logic info useful for elaborations
            //  Warn -  to log not critical situations not handled by implementation
            log4net.Config.XmlConfigurator.Configure();

            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            LanguageDictionary.Instance.FindString("Init LanguageDictionary");
        }
    }
}
