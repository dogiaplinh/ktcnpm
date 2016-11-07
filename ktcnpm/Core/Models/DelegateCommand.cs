using System;
using System.Linq;
using System.Windows.Input;

namespace Core.Models
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command. This can be null.</param>
        /// <exception cref="ArgumentNullException">When both <paramref name="execute"/> and <paramref name="canExecute"/> are <see langword="null" />.</exception>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute) : this((x) => execute())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
        public DelegateCommand(Action<T> execute) : this(execute, (T o) => true)
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        ///<summary>
        ///Determines if the command can execute by invoked the <see cref="Func{T,Bool}"/> provided during construction.
        ///</summary>
        ///<param name="parameter">Data used by the command to determine if it can execute.</param>
        ///<returns>
        ///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        ///</returns>
        public bool CanExecute(T parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        ///<summary>
        ///Executes the command and invokes the <see cref="Action{T}"/> provided during construction.
        ///</summary>
        ///<param name="parameter">Data used by the command.</param>
        public void Execute(T parameter)
        {
            _execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }
}