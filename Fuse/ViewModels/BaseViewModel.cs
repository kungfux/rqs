using System.ComponentModel;
using Castle.INPC;

namespace Fuse.ViewModels
{
    internal abstract class BaseViewModel : INPCInvoker
    {
        public virtual void RegisterCommands()
        {
        }

        public void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
    }
}
