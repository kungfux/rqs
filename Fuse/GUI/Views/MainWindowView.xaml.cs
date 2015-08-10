using Fuse.GUI.Models;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Fuse.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        public static RoutedCommand StartServerCommand = new RoutedCommand();
        private void startServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !WebServerModel.Instance.IsAlive;
        }

        private void startServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WebServerModel.Instance.StartInstance();
        }

        public static RoutedCommand StopServerCommand = new RoutedCommand();
        private void stopServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WebServerModel.Instance.IsAlive;
        }

        private void stopServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WebServerModel.Instance.StopInstance();
        }

        public static RoutedCommand RestartServerCommand = new RoutedCommand();
        private void restartServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WebServerModel.Instance.IsAlive;
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
            bool isServerRunning = WebServerModel.Instance.IsAlive;
            if (!isServerRunning)
            {
                Application.Current.Shutdown();
                return true;
            }
            else
            {
                MessageBoxResult exitDialogResult =
                    MessageBox.Show("Do you want to stop the server and exit?", this.Title,
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
            return false;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !exit_Confirm();
        }
    }
}
