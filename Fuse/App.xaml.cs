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
            log4net.Config.XmlConfigurator.Configure();

            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            LanguageDictionary.Instance.FindString("Init LanguageDictionary");
        }
    }
}
