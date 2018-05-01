namespace EyeDroppr
{
    using System;
    using System.Windows.Input;

    public class ActionCommand : ICommand
    {
        private readonly Action theAction;

        public ActionCommand(Action theAction)
        {
            this.theAction = theAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.theAction();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class ActionCommand<T> : ICommand
    {
        private readonly Action<T> _theAction;
        private readonly T _paramterValue;

        public ActionCommand(Action<T> theAction, T paramterValue)
        {
            _theAction = theAction;
            _paramterValue = paramterValue;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _theAction.Invoke(_paramterValue);
        }

        public event EventHandler CanExecuteChanged;
    }
}