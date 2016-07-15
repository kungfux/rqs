using System;
using System.Threading;
using System.Windows;

namespace Fuse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
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

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            InitializeResourceDictionaries();
        }

        private void InitializeResourceDictionaries()
        {
            var dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
