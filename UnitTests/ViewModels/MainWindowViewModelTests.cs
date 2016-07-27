using System.Windows;
using Fuse;
using Fuse.ViewModels;
using Fuse.Views;
using Moq;
using WebServer;
using Xunit;

namespace UnitTests.ViewModels
{
    public class MainWindowViewModelTests : UnitTest
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class FakeMainWindowViewModel : MainWindowViewModel
        {
            public FakeMainWindowViewModel(IServer server, IViewFactory viewFactory, ILanguageDictionary languageDictionary)
                : base(server, viewFactory, languageDictionary)
            {
            }

            public bool ApplicationClosed { get; private set; }

            protected override void Shutdown()
            {
                ApplicationClosed = true;
            }
        }

        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindowViewModelTests()
        {
            _mainWindowViewModel = Mock.CreateInstance<FakeMainWindowViewModel>();
            _mainWindowViewModel.RegisterCommands();
        }

        [Fact]
        public void StartServerCommand_CallsStartMethod()
        {
            Mock.Setup<IServer>(server => server.Start());
            _mainWindowViewModel.StartServerCommand.Execute(null);
            Mock.VerifyAll();
        }

        [Fact]
        public void StartServerCommand_DependsOnServerStatus()
        {
            _mainWindowViewModel.IsServerRunning = true;
            Assert.False(_mainWindowViewModel.StartServerCommand.CanExecute(null));
            _mainWindowViewModel.IsServerRunning = false;
            Assert.True(_mainWindowViewModel.StartServerCommand.CanExecute(null));
        }

        [Fact]
        public void StopServerCommand_CallsStartMethod()
        {
            Mock.Setup<IServer>(server => server.Stop());
            _mainWindowViewModel.StopServerCommand.Execute(null);
            Mock.VerifyAll();
        }

        [Fact]
        public void StopServerCommand_DependsOnServerStatus()
        {
            _mainWindowViewModel.IsServerRunning = true;
            Assert.True(_mainWindowViewModel.StopServerCommand.CanExecute(null));
            _mainWindowViewModel.IsServerRunning = false;
            Assert.False(_mainWindowViewModel.StopServerCommand.CanExecute(null));
        }

        private void ConfigureMessageBox(MessageBoxResult expectedResult)
        {
            const string message = "Do you want to stop the Server and exit?";
            Mock.Setup<ILanguageDictionary, string>(dictionary => dictionary.FindString("MessageWantToStopAndExit"))
                .Returns(message);
            Mock.Setup<IViewFactory, MessageBoxResult>(factory => factory
                .ShowDialog(message, "Fuse", MessageBoxButton.YesNo, MessageBoxImage.Asterisk,
                    MessageBoxResult.No, It.IsAny<MessageBoxOptions>()))
                .Returns(expectedResult);
        }

        [Fact]
        public void TryExit_ServerRunningAndUserConfirmed_StopsTheServer_ShutsDownTheApp()
        {
            ConfigureMessageBox(MessageBoxResult.Yes);
            Mock.Setup<IServer>(server => server.Stop());

            _mainWindowViewModel.IsServerRunning = true;
            Assert.True(_mainWindowViewModel.TryExit());
            Assert.True(((FakeMainWindowViewModel)_mainWindowViewModel).ApplicationClosed);
            Mock.VerifyAll();
        }

        [Fact]
        public void TryExit_ServerRunningAndUserCancelled_FalseResult()
        {
            ConfigureMessageBox(MessageBoxResult.No);

            _mainWindowViewModel.IsServerRunning = true;

            Assert.False(_mainWindowViewModel.TryExit());
            Assert.False(((FakeMainWindowViewModel)_mainWindowViewModel).ApplicationClosed);
            Mock.VerifyAll();
        }

        [Fact]
        public void TryExit_ServerStopped_ShutsDownTheApp()
        {
            _mainWindowViewModel.IsServerRunning = false;
            Assert.True(_mainWindowViewModel.TryExit());
            Assert.True(((FakeMainWindowViewModel)_mainWindowViewModel).ApplicationClosed);
        }
    }
}
