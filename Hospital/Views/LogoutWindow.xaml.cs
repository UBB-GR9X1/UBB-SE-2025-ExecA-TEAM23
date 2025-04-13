using Hospital.Exceptions;
using Hospital.Interfaces;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Hospital
{
    public sealed partial class LogoutWindow : Window
    {
        private readonly IAuthService _authService;

        public LogoutWindow(IAuthService authService)
        {
            this.InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await PerformLogoutAsync();
                NavigateToMainWindow();
            }
            catch (Exception ex) when (ex is AuthenticationException || ex is SqlException)
            {
                await DisplayErrorDialogAsync(ex.Message);
            }
        }

        private async Task PerformLogoutAsync()
        {
            await _authService.Logout();
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
                XamlRoot = Content.XamlRoot
            };

            await errorDialog.ShowAsync();
        }
    }
}
