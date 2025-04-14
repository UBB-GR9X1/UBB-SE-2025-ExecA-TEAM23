using Hospital.Exceptions;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.System.UserProfile;

namespace Hospital
{
    public sealed partial class LogoutWindow : Window
    {
        private readonly AuthViewModel _viewModel;

        public LogoutWindow(AuthViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel; // Pass ViewModel to interact with auth logic
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.Logout(); 


                LoginWindow newLogInWindow = new LoginWindow();
                newLogInWindow.Activate();

                this.Close(); 
            }
            catch (AuthenticationException ex)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
            catch (SqlException err)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{err.Message}",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }
    }
}
