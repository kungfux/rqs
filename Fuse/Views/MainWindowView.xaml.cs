using System.ComponentModel;
using Fuse.ViewModels;

namespace Fuse.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void MainWindowViewOnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !((MainWindowViewModel)DataContext).TryExit();
        }
    }
}
