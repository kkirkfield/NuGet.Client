using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuGet.Options
{
    internal class RemoveButtonCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> canExecute;
        public RemoveButtonCommand(Action<object> executeRemoveButtonCommand, Func<object, bool> canExecuteRemoveButtonCommand)
        {
            action = executeRemoveButtonCommand;
            canExecute = canExecuteRemoveButtonCommand;
        }

        public event EventHandler CanExecuteChanged;

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
            InvokeCanExecuteChanged();
        }
    }
}
