using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using WebServer.API;
using Xunit;

namespace WebServer.UnitTests
{
    public class ServerTests : UnitTest
    {
        private class FakeServer : Server
        {
            public FakeServer(ITcpListener tcpListener, IClientProcessor clientProcessor) : base(tcpListener, clientProcessor)
            {
            }

            protected override ICollection<IExtension> LoadExtensions() => Enumerable.Empty<IExtension>().ToList();
        }

        private readonly FakeServer _server;

        public ServerTests()
        {
            _server = Mock.CreateInstance<FakeServer>();
        }

        [Fact]
        public void Start_StartsListeningAndUpdatesStatus()
        {
            var started = false;
            Mock.Setup<ITcpListener>(listener => listener.Start());
            _server.StatusChanged += (sender, status) => started = status == Status.ServerStatus.Started;
            _server.Start();

            Assert.True(started);
            Mock.VerifyAll();
        }

        [Fact]
        public void Stop_StopsListeningAndUpdatesStatus()
        {
            var stopped = false;
            Mock.Setup<ITcpListener>(listener => listener.Stop());
            _server.StatusChanged += (sender, status) => stopped = status == Status.ServerStatus.Stopped;

            _server.Start();
            Thread.Sleep(50);
            _server.Stop();

            Assert.True(stopped);
            Mock.VerifyAll();
        }
    }
}
