using System.ComponentModel;
using Fuse.Views;

namespace Fuse.ViewModels
{
    internal class MainWindowViewModel : IViewModel<MainWindowView>
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
    }
}
