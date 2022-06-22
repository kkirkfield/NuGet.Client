using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NuGet.Options
{
    internal class ShowButtonCommand : ICommand
    {
        private Action<object> action;
        private Func<object, bool> canExecute;
        public ShowButtonCommand(Action<object> executeShowButtonCommand, Func<object, bool> canExecuteShowButtonCommand)
        {
            action = executeShowButtonCommand;
            canExecute = canExecuteShowButtonCommand;
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
