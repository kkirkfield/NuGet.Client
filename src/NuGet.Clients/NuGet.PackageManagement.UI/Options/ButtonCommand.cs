using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuGet.Options
{
    internal class ButtonCommand : ICommand
    {
        private Action<object> _action;
        private Func<object, bool> _canExecute;
        public ButtonCommand(Action<object> executeButtonCommand, Func<object, bool> canExecuteButtonCommand)
        {
            _action = executeButtonCommand;
            _canExecute = canExecuteButtonCommand;
        }

        public event EventHandler CanExecuteChanged;

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
            InvokeCanExecuteChanged();
        }

    }
}
