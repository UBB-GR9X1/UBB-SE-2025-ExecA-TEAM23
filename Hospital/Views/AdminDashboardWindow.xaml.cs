// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminDashboardWindow.xaml.cs" company="Hospital">
//   Copyright (c) Hospital. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the AdminDashboardWindow for managing administrator functionalities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital.Views
{
    using System;
    using System.Threading.Tasks;
    using Hospital.Interfaces;
    using Hospital.Managers;
    using Hospital.ViewModels;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Window for the Admin Dashboard functionality.
    /// </summary>
    public sealed partial class AdminDashboardWindow : Window
    {
        private readonly IAuthService authService;
        private readonly LoggerViewModel loggerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDashboardWindow"/> class.
        /// </summary>
        /// <param name="authService">Authentication service for user operations.</param>
        /// <param name="loggerService">Logger service for auditing.</param>
        /// <exception cref="ArgumentNullException">Thrown if auth service is null.</exception>
        public AdminDashboardWindow(IAuthService authService, ILoggerService loggerService)
        {
            this.InitializeComponent();
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));

            // Initialize LoggerViewModel with LoggerManagerModel
            var loggerManagerModel = new LoggerManagerModel(loggerService);
            this.loggerViewModel = new LoggerViewModel(loggerManagerModel);

            // Set up UI bindings
            this.ConfigureUserInterface();

            // Load all logs initially
            this.LoadInitialLogData();
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
        }

        private void LoadAllLogsButton_Click(object sender, RoutedEventArgs e)
        {
            this.loggerViewModel.LoadAllLogsCommand.Execute(null);
        }

        private void LoadLogsByUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            this.loggerViewModel.FilterLogsByUserIdCommand.Execute(null);
        }

        private void LoadLogsByActionTypeButton_Click(object sender, RoutedEventArgs e)
        {
            this.loggerViewModel.FilterLogsByActionTypeCommand.Execute(null);
        }

        private void LoadLogsBeforeTimestampButton_Click(object sender, RoutedEventArgs e)
        {
            this.loggerViewModel.FilterLogsByTimestampCommand.Execute(null);
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            this.loggerViewModel.ApplyAllFiltersCommand.Execute(null);
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PerformLogoutAsync();
        }

        private async Task PerformLogoutAsync()
        {
            try
            {
                await this.authService.Logout();
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
