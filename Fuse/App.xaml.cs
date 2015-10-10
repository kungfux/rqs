using Fuse.GUI.Models;
using System;
using System.Threading;
using System.Windows;

namespace Fuse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            LanguageDictionary.Instance.FindString("Init LanguageDictionary");
        }
    }
}
