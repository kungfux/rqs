using System.ComponentModel;
using System.Windows.Controls;

namespace Fuse.ViewModels
{
    internal interface IViewModel<T> : INotifyPropertyChanged where T : ContentControl
    {
    }
}
