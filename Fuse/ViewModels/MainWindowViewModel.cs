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
        private readonly IConfiguration _configuration;
        private readonly IServer _server;

        public MainWindowViewModel(IConfiguration configuration, IServer server)
        {
            _configuration = configuration;
            _server = server;

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

                _configuration.Port = Properties.Settings.Default.PORT;
                _configuration.RootPath = Properties.Settings.Default.ROOT_PATH;
                _configuration.IndexFile = Properties.Settings.Default.INDEX_FILE;
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

        public bool TryExit()
        {
            var result = true;
            if (!IsServerRunning)
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBoxResult exitDialogResult =
                    MessageBox.Show(LanguageDictionary.Instance.FindString("MessageWantToStopAndExit"), "Fuse",
                    MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No);
                if (exitDialogResult == MessageBoxResult.Yes)
                {
                    _server.Stop();
                    Application.Current.Shutdown();
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
