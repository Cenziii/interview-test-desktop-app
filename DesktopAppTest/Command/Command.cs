using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DesktopAppTest.Command
{
    public class Command : ICommand
    {
        Action<object> executeMethod;
        Predicate<object> canExecuteMethod;
        event EventHandler CanExecuteChangedInternal;

        public ICommand Comando { get { return this; } }

        public Command(Action<object> exe, Predicate<object> canexe)
        {
            this.executeMethod = exe;
            this.canExecuteMethod = canexe;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            if (canExecuteMethod != null)
                OnExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod != null)
            {
                return this.canExecuteMethod != null && this.canExecuteMethod(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }

        public void OnExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

    }
}
