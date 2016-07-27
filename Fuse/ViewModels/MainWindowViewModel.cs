using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Castle.INPC;
using Fuse.Views;
using WebServer;

namespace Fuse.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, IViewModel<MainWindowView>
    {
        private readonly IServer _server;
        private readonly IViewFactory _viewFactory;
        private readonly ILanguageDictionary _languageDictionary;

        public MainWindowViewModel(IServer server, IViewFactory viewFactory, ILanguageDictionary languageDictionary)
        {
            _server = server;
            _viewFactory = viewFactory;
            _languageDictionary = languageDictionary;

            _server.StatusChanged += (sender, status) =>
            {
                IsServerRunning = status == Status.ServerStatus.Started;
            };
        }

        [InjectINPC]
        public virtual bool IsServerRunning { get; set; }

        public ICommand StartServerCommand { get; private set; }
        public ICommand StopServerCommand { get; private set; }
        public ICommand RestartServerCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand ViewLogsCommand { get; private set; }
        public ICommand PreferencesCommand { get; private set; }
        public ICommand GitHubCommand { get; private set; }

        public override void RegisterCommands()
        {
            StartServerCommand = LambdaCommand.From(param =>
            {
                _server.Start();
            })
            .CanExecuteIf(param => !IsServerRunning);

            StopServerCommand = LambdaCommand.From(param => _server.Stop())
                .CanExecuteIf(param => IsServerRunning);

            RestartServerCommand = LambdaCommand.From(param => { throw new NotImplementedException(); })
                .CanExecuteIf(param => false);

            ExitCommand = LambdaCommand.From(param => TryExit());

            ViewLogsCommand = LambdaCommand.From(param => { throw new NotImplementedException(); })
                .CanExecuteIf(param => false);

            PreferencesCommand = LambdaCommand.From(param => { throw new NotImplementedException(); })
                .CanExecuteIf(param => false);

            GitHubCommand = LambdaCommand.From(param => Process.Start("https://github.com/kungfux/rqs"));

            base.RegisterCommands();
        }

        protected virtual void Shutdown() => Application.Current.Shutdown();

        public bool TryExit()
        {
            var result = true;
            if (!IsServerRunning)
            {
                Shutdown();
            }
            else
            {
                var exitDialogResult = _viewFactory.ShowDialog(_languageDictionary.FindString("MessageWantToStopAndExit"), "Fuse",
                    MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No);
                if (exitDialogResult == MessageBoxResult.Yes)
                {
                    _server.Stop();
                    Shutdown();
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
