using System;
using System.Windows.Input;

namespace Fuse.ViewModels
{
    internal class LambdaCommand : ICommand
    {
        private Action<object> _commandAction;

        private LambdaCommand()
        {
        }

        public static LambdaCommand From(Action<object> commandAction)
        {
            var cmd = new LambdaCommand { _commandAction = commandAction };
            return cmd;
        }

        public static LambdaCommand From(Action<ICommand, object> commandAction)
        {
            var cmd = new LambdaCommand();
            cmd._commandAction = obj => commandAction(cmd, obj);
            return cmd;
        }

        public FullLambdaCommand CanExecuteIf(Func<object, bool> canExecuteAction)
            => new FullLambdaCommand(this, (cmd, obj) => canExecuteAction(obj));

        public FullLambdaCommand CanExecuteIf(Func<ICommand, object, bool> canExecuteAction)
            => new FullLambdaCommand(this, canExecuteAction);

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _commandAction(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        internal class FullLambdaCommand : ICommand
        {
            private readonly Action<object> _commandAction;
            private readonly Func<object, bool> _canExecuteAction;

            public FullLambdaCommand(LambdaCommand command, Func<ICommand, object, bool> canExecuteAction)
            {
                _commandAction = command._commandAction;
                _canExecuteAction = obj => canExecuteAction(this, obj);
            }

            public bool CanExecute(object parameter) => _canExecuteAction(parameter);

            public void Execute(object parameter)
            {
                _commandAction(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}
