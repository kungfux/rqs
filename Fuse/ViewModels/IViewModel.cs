using System.Windows.Controls;
using Castle.INPC;

namespace Fuse.ViewModels
{
    internal interface IViewModel<T> : INPCInvoker where T : ContentControl
    {
        void RegisterCommands();
    }
}
