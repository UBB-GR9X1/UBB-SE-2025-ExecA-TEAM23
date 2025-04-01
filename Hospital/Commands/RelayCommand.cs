using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    // Constructor for command with execution logic and optional CanExecute logic
    public RelayCommand(Action execute) : this(execute, null) { }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    // Determines if the command can execute
    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

    // Executes the command
    public void Execute(object parameter) => _execute();

    // Event for handling when CanExecute changes
    public event EventHandler CanExecuteChanged;

    // You can manually raise this event to indicate a change in CanExecute
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
