using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Fuse.GUI
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
            e.CanExecute = false;
        }

        private void startServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static RoutedCommand StopServerCommand = new RoutedCommand();
        private void stopServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void stopServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            bool isServerRunning = false;
            if (!isServerRunning)
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBoxResult exitDialogResult = 
                    MessageBox.Show("Do you want to stop the server and exit?", this.Title , 
                    MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No);
                if (exitDialogResult == MessageBoxResult.Yes)
                {
                    // TODO
                    // stop server before exit
                    Application.Current.Shutdown();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exit_Executed(this, null);
        }
    }
}
