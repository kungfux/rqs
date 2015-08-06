using System.Threading;
using System.Windows;

namespace Fuse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WebServer.Server ws = new WebServer.Server();
        private Thread s;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s = new Thread(Start);
            s.Start();
        }

        private void Start()
        {
            ws.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (s != null)
            {
                ws.Stop();
                s.Abort();
            }
        }
    }
}
