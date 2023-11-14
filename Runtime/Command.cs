namespace UIFlow.Runtime
{
    using System;
    using System.Windows.Input;

    public sealed class Command : ICommand
    {
        private readonly Func<object, bool> m_CanExecute;
        private readonly Action<object> m_ExecuteAction;
        
        public event EventHandler CanExecuteChanged;

        public Command(Action<object> executeAction)
        {
            m_ExecuteAction = executeAction;
        }

        public Command(Func<object, bool> canExecute, Action<object> executeAction)
        {
            m_CanExecute = canExecute;
            m_ExecuteAction = executeAction;
        }
        
        public bool CanExecute(object parameter)
        {
            return m_CanExecute == null || m_CanExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            m_ExecuteAction.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}