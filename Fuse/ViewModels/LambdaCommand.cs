using System;
using System.Windows.Input;

namespace Fuse.ViewModels
{
    internal interface ILambdaCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    internal class LambdaCommand : ILambdaCommand
    {
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;

        private LambdaCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            if (executeAction == null)
                throw new ArgumentNullException(nameof(executeAction));
            if (canExecuteAction == null)
                throw new ArgumentNullException(nameof(canExecuteAction));

            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public static EnabledLambdaCommand From(Action commandAction)
            => new EnabledLambdaCommand(parameter => commandAction());

        public static EnabledLambdaCommand From(Action<object> commandAction)
            => new EnabledLambdaCommand(commandAction);

        public void Execute(object parameter) => _executeAction(parameter);

        public bool CanExecute(object parameter) => _canExecuteAction(parameter);

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        internal class EnabledLambdaCommand : ILambdaCommand
        {
            private readonly Action<object> _executeAction;

            protected internal EnabledLambdaCommand(Action<object> executeAction)
            {
                _executeAction = executeAction;
            }

            public LambdaCommand CanExecuteIf(Func<bool> canExecuteAction)
                => new LambdaCommand(_executeAction, parameter => canExecuteAction());

            public LambdaCommand CanExecuteIf(Func<object, bool> canExecuteAction)
                => new LambdaCommand(_executeAction, canExecuteAction);

            public void Execute(object parameter) => _executeAction(parameter);

            public bool CanExecute(object parameter) => true;

            public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }
        }
    }
}
