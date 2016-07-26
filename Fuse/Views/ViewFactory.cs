using System;
using System.Windows;
using Castle.Windsor;
using Fuse.ViewModels;

namespace Fuse.Views
{
    internal interface IViewFactory
    {
        T BuildView<T>() where T : Window, new();
    }

    internal class ViewFactory : IViewFactory
    {
        private readonly IWindsorContainer _container;

        public ViewFactory(IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public T BuildView<T>() where T : Window, new()
        {
            var view = new T();
            var viewModel = _container.Resolve<IViewModel<T>>();
            viewModel.RegisterCommands();
            view.DataContext = viewModel;
            return view;
        }
    }
}
