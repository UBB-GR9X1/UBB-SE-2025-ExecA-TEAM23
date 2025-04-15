// <copyright file="AdminDashboardWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>


namespace Hospital.Views
{
    using System;
    using System.Threading.Tasks;
    using Hospital.Repositories;
    using Hospital.Services;
    using Hospital.ViewModels;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Window for the Admin Dashboard functionality.
    /// </summary>
    public sealed partial class AdminDashboardWindow : Window
    {
        private readonly IAuthViewModel authViewModel;
        private readonly ILoggerViewModel loggerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDashboardWindow"/> class.
        /// </summary>
        /// <param name="authViewModel">Authentication service for user operations.</param>
        /// <param name="loggerRepository">Logger service for auditing.</param>
        /// <exception cref="ArgumentNullException">Thrown if auth service is null.</exception>
        public AdminDashboardWindow(IAuthViewModel authViewModel, ILoggerRepository loggerRepository)
        {
            this.InitializeComponent();
            this.authViewModel = authViewModel ?? throw new ArgumentNullException(nameof(authViewModel));

            // Initialize LoggerViewModel with LoggerService
            var loggerManagerModel = new LoggerService(loggerRepository);
            this.loggerViewModel = new LoggerViewModel(loggerManagerModel);

            // Load all logs initially
            this.LoadInitialLogData();

            // Set up UI bindings
            this.ConfigureUserInterface();
        }

        private void LoadInitialLogData()
        {
            this.loggerViewModel.LoadAllLogsCommand.Execute(null);
        }

        private void ConfigureUserInterface()
        {
            // Set the item source for ListView
            this.LogListView.ItemsSource = this.loggerViewModel.Logs;

            // Set up ComboBox for action types
            this.ActionTypeComboBox.ItemsSource = this.loggerViewModel.ActionTypes;

            // Bind TextBox for user ID filtering
            this.UserIdTextBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Path = new PropertyPath("UserIdInput"),
                Source = this.loggerViewModel,
                Mode = BindingMode.TwoWay,
            });

            // Bind ComboBox for action type filtering
            this.ActionTypeComboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath("SelectedActionType"),
                Source = this.loggerViewModel,
                Mode = BindingMode.TwoWay,
            });

            // Bind DatePicker for timestamp filtering
            this.TimestampDatePicker.SetBinding(DatePicker.DateProperty, new Binding
            {
                Path = new PropertyPath("SelectedTimestamp"),
                Source = this.loggerViewModel,
                Mode = BindingMode.TwoWay,
                Converter = new Helpers.DateTimeToDateTimeOffsetConverter(),
            });

            // Bind the buttons directly to commands in the ViewModel
            this.LoadAllLogsButton.Command = this.loggerViewModel.LoadAllLogsCommand;
            this.LoadLogsByUserIdButton.Command = this.loggerViewModel.FilterLogsByUserIdCommand;
            this.LoadLogsByActionTypeButton.Command = this.loggerViewModel.FilterLogsByActionTypeCommand;
            this.LoadLogsBeforeTimestampButton.Command = this.loggerViewModel.FilterLogsByTimestampCommand;
            this.ApplyFiltersButton.Command = this.loggerViewModel.ApplyAllFiltersCommand;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PerformLogoutAsync();
        }

        private async Task PerformLogoutAsync()
        {
            try
            {
                await this.authViewModel.Logout();
                this.NavigateToMainWindow();
            }
            catch (Exception exception)
            {
                await this.DisplayErrorDialogAsync($"Logout error: {exception.Message}");
            }
        }

        private void NavigateToMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            this.Close();
        }

        private async Task DisplayErrorDialogAsync(string errorMessage)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = errorMessage,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };

            await errorDialog.ShowAsync();
        }
    }
}
