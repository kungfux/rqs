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
            view.DataContext = _container.Resolve<IViewModel<T>>();
            return view;
        }
    }
}
