using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    // Constructor for command with execution logic and optional CanExecute logic
    public RelayCommand(Action execute) : this(execute, null) { }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    // Determines if the command can execute
    public bool CanExecute(object parameter) => canExecute == null || canExecute();

    // Executes the command
    public void Execute(object parameter) => execute();

    // Event for handling when CanExecute changes
    public event EventHandler CanExecuteChanged;

    // You can manually raise this event to indicate a change in CanExecute
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
