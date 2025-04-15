using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Hospital.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows;
using Hospital.Helpers;
using Hospital.Repositories;
using Hospital.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    public sealed partial class LoggerView : Window
    {
        private readonly LoggerViewModel _loggerViewModel;

        // In LoggerView constructor
        public LoggerView(ILoggerRepository loggerRepository)
        {
            this.InitializeComponent();

            LoggerService loggerService = new LoggerService(loggerRepository);
            _loggerViewModel = new LoggerViewModel(loggerService);

            this.BindUserInterface();
            this.LoadInitialLogs();
        }


        private void LoadInitialLogs()
        {
            _loggerViewModel.LoadAllLogsCommand.Execute(null);
        }

        private void BindUserInterface()
        {
            LogListView.ItemsSource = _loggerViewModel.Logs;

            LoadAllLogsButton.Command = _loggerViewModel.LoadAllLogsCommand;

            LoadLogsByUserIdButton.Command = _loggerViewModel.FilterLogsByUserIdCommand;
            UserIdTextBox.DataContext = _loggerViewModel;

            LoadLogsByActionTypeButton.Command = _loggerViewModel.FilterLogsByActionTypeCommand;
            ActionTypeComboBox.ItemsSource = _loggerViewModel.ActionTypes;

            LoadLogsBeforeTimestampButton.Command = _loggerViewModel.FilterLogsByTimestampCommand;

            LoadLogsWithAllParametersButton.Command = _loggerViewModel.ApplyAllFiltersCommand;

            // Bind TextBox, ComboBox, and DatePicker to ViewModel properties
            UserIdTextBox.SetBinding(Microsoft.UI.Xaml.Controls.TextBox.TextProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new Microsoft.UI.Xaml.PropertyPath("UserIdInput"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            ActionTypeComboBox.SetBinding(Microsoft.UI.Xaml.Controls.ComboBox.SelectedItemProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Source = _loggerViewModel,
                Path = new Microsoft.UI.Xaml.PropertyPath("SelectedActionType"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            TimestampDatePicker.SetBinding(Microsoft.UI.Xaml.Controls.DatePicker.DateProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Source = _loggerViewModel,
                Path = new Microsoft.UI.Xaml.PropertyPath("SelectedTimestamp"),
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
                Converter = new DateTimeToDateTimeOffsetConverter()
            });

        }
    }
}