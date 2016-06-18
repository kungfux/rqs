using FuseControlSystem.Models;
using FuseWebServer.WebServer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace FuseControlSystem.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window, INotifyPropertyChanged
    {
        public MainWindowView()
        {
            InitializeComponent();

            brdStatus.DataContext = this;
        }

        #region Commands
        public static RoutedCommand StartServerCommand = new RoutedCommand();
        private void startServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsServerRunning;
        }

        private void startServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WebServerModel.Instance.StartInstance();
        }

        public static RoutedCommand StopServerCommand = new RoutedCommand();
        private void stopServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsServerRunning;
        }

        private void stopServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WebServerModel.Instance.StopInstance();
        }

        public static RoutedCommand RestartServerCommand = new RoutedCommand();
        private void restartServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void restartServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static RoutedCommand ExitCommand = new RoutedCommand();
        private void exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            exit_Confirm();
        }

        private bool exit_Confirm()
        {
            bool isServerRunning = IsServerRunning;
            if (!isServerRunning)
            {
                Application.Current.Shutdown();
                return true;
            }
            else
            {
                MessageBoxResult exitDialogResult =
                    MessageBox.Show(LanguageDictionary.Instance.FindString("messageWantToStopAndExit"), this.Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No);
                if (exitDialogResult == MessageBoxResult.Yes)
                {
                    WebServerModel.Instance.StopInstance();
                    Application.Current.Shutdown();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static RoutedCommand ViewLogsCommand = new RoutedCommand();
        private void viewLogs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void viewLogs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static RoutedCommand PreferencesCommand = new RoutedCommand();
        private void preferences_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void preferences_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static RoutedCommand GitHubCommand = new RoutedCommand();
        private void github_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("https://github.com/kungfux/rqs");
        }
        #endregion Commands

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebServerModel.Instance.server.StatusChanged += server_StatusChanged;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !exit_Confirm();
        }

        void server_StatusChanged(object sender, Status e)
        {
            IsServerRunning = e == Status.Started;
        }

        #region

        private bool _isServerRunning;
        public bool IsServerRunning
        {
            get { return _isServerRunning; }
            set
            {
                _isServerRunning = value;
                OnProperyChanged("IsServerRunning");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnProperyChanged(string pProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pProperty));
        }

        #endregion
    }
}
