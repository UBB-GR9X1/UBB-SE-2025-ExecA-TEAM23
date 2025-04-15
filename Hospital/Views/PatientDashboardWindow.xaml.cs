using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Hospital.Exceptions;
using System.Threading.Tasks;

namespace Hospital.Views
{
    public sealed partial class PatientDashboardWindow : Window
    {
        private readonly IAuthViewModel _authenticationViewModel;

        public PatientDashboardWindow(IPatientViewModel patientViewModel, IAuthViewModel authenticationViewModel)
        {
            InitializeComponent();
            _authenticationViewModel = authenticationViewModel;

            var patientDashboardControl = new PatientDashboardControl(patientViewModel);
            patientDashboardControl.LogoutButtonClicked += HandleLogoutRequested;

            PatientDashboard.Content = patientDashboardControl;
        }

        private async void HandleLogoutRequested()
        {
            try
            {
                await _authenticationViewModel.Logout();
                var loginWindow = new MainWindow();
                loginWindow.Activate();
                Close();
            }
            catch (AuthenticationException authenticationException)
            {
                await ShowErrorDialog("Authentication Error", authenticationException.Message);
            }
            catch (SqlException databaseException)
            {
                await ShowErrorDialog("Database Error", databaseException.Message);
            }
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            var errorDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await errorDialog.ShowAsync();
        }
    }
}
