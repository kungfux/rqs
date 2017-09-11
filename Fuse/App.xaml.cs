using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Castle.INPC;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fuse.ViewModels;
using Fuse.Views;
using WebServer;

namespace Fuse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Log levels:
            //  Debug - to log info useful for debugging
            //  Error - to log not critical exceptions
            //  Fatal - to log critical exceptions
            //  Info -  to log business logic info useful for elaborations
            //  Warn -  to log not critical situations not handled by implementation
            log4net.Config.XmlConfigurator.Configure();

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            InitializeResourceDictionaries();

            var container = new WindsorContainer();
            ConfigureDependencies(container);

            container.Resolve<IViewFactory>().BuildView<MainWindowView>().Show();
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

        private void ConfigureDependencies(IWindsorContainer container)
        {
            ConfigureViewModels(container);
            InitializeConfiguration(container);

            container.Register(Component.For<ILanguageDictionary>().ImplementedBy<LanguageDictionary>().LifestyleSingleton());
            container.Register(Component.For<IViewFactory>().Instance(new ViewFactory(container)));
            container.Install(new WebServerInstaller());
        }

        private void ConfigureViewModels(IWindsorContainer container)
        {
            var viewModels = Classes.FromThisAssembly().IncludeNonPublicTypes().BasedOn(typeof(IViewModel<>)).LifestyleTransient();
            viewModels.Configure(c =>
            {
                var abstraction = c.Implementation.GetInterfaces()
                    .Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewModel<>));
                container.Register(Component.For(abstraction).UsingFactoryMethod(() => container.Resolve(c.Implementation)));
            });
            container.Install(new INPCInstaller(viewModels));
        }

        private void InitializeConfiguration(IWindsorContainer container)
        {
            var configuration = new Configuration();
            configuration.Port = Fuse.Properties.Settings.Default.PORT;
            configuration.RootPath = Fuse.Properties.Settings.Default.ROOT_PATH;
            configuration.IndexFile = Fuse.Properties.Settings.Default.INDEX_FILE;
            container.Register(Component.For<IConfiguration>().Instance(configuration).LifestyleSingleton());
        }
    }
}
