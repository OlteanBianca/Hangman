using System;
using System.Windows.Input;

namespace Hangman.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action commandTask;
        private readonly Action<object> action;

        public RelayCommand(Action workToDo) : this(workToDo, DefaultCanExecute)
        {
            commandTask = workToDo;
        }

        public RelayCommand(Action workToDo, Predicate<object> canExecute)
        {
            commandTask = workToDo;
        }

        public RelayCommand(Action<object> action)
        {
            this.action = action;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            if (commandTask == null)
            {
                action(parameter);
            }
            else
            {
                commandTask();
            }
        }
    }
}
