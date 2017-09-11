using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace WebServer
{
    public class WebServerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //TODO: Add log info
            //TODO: _log.Debug($"Listener initialized on {_ipAddress}:{configuration.Port}.");
            //TODO: _log.Fatal("Exception occurs while initializing the listener.", ex);
            container.Register(Component.For<ITcpListener>().ImplementedBy<TcpListener>().LifestyleTransient());
            container.Register(Component.For<IServer>().ImplementedBy<Server>().LifestyleSingleton());
            container.Register(Component.For<IClientProcessor>().ImplementedBy<ClientProcessor>().LifestyleSingleton());
        }
    }
}
