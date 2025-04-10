using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Views
{
    public sealed partial class AdminDashboardWindow : Window
    {
        private readonly AuthViewModel _authViewModel;
        private readonly LoggerViewModel _loggerViewModel;

        public AdminDashboardWindow(AuthViewModel authViewModel)
        {
            this.InitializeComponent();
            _authViewModel = authViewModel;

            // Initialize LoggerViewModel with LoggerManagerModel
            _loggerViewModel = new LoggerViewModel(new LoggerManagerModel());

            // Set up UI bindings
            BindUi();

            // Load all logs initially
            PopulateLogsView();
        }

        private void PopulateLogsView()
        {
            _loggerViewModel.LoadAllLogsCommand.Execute(null);
        }

        private void BindUi()
        {
            // Set the item source for ListView
            LogListView.ItemsSource = _loggerViewModel.Logs;

            // Set up ComboBox for action types
            ActionTypeComboBox.ItemsSource = _loggerViewModel.ActionTypes;

            // Bind TextBox for user ID filtering
            UserIdTextBox.SetBinding(TextBox.TextProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("UserIdInput"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            // Bind ComboBox for action type filtering
            ActionTypeComboBox.SetBinding(ComboBox.SelectedItemProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("SelectedActionType"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay
            });

            // Bind DatePicker for timestamp filtering
            TimestampDatePicker.SetBinding(DatePicker.DateProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("SelectedTimestamp"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
                Converter = new Helpers.DateTimeToDateTimeOffsetConverter()
            });
        }

        private void LoadAllLogsButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.LoadAllLogsCommand.Execute(null);
        }

        private void LoadLogsByUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.LoadLogsByUserIdCommand.Execute(null);
        }

        private void LoadLogsByActionTypeButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.LoadLogsByActionTypeCommand.Execute(null);
        }

        private void LoadLogsBeforeTimestampButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.LoadLogsBeforeTimestampCommand.Execute(null);
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.LoadLogsWithParametersCommand.Execute(null);
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await _authViewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            this.Close();
        }
    }
}