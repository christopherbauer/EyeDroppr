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
}