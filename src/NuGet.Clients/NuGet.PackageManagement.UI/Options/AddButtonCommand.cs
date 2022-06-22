using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuGet.Options
{
    internal class AddButtonCommand : ICommand
    {
        private Action<object> _action;
        private Func<object, bool> _canExecute;
        public AddButtonCommand(Action<object> executeAddButtonCommand, Func<object, bool> canExecuteAddButtonCommand)
        {
            _action = executeAddButtonCommand;
            _canExecute = canExecuteAddButtonCommand;
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
