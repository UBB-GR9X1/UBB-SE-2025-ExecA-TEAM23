using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Hospital.Exceptions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;

namespace Hospital.Views
{
    public sealed partial class PatientDashboardPage : Page
    {
        private IAuthViewModel _authenticationViewModel;

        public PatientDashboardPage()
        {
            InitializeComponent();
        }

        public PatientDashboardPage(IPatientViewModel patientViewModel, IAuthViewModel authenticationViewModel)
        {
            InitializeComponent();
            _authenticationViewModel = authenticationViewModel;

            var patientDashboardControl = new PatientDashboardControl(patientViewModel);
            patientDashboardControl.LogoutButtonClicked += HandleLogoutRequested;

            this.Content = patientDashboardControl;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<IPatientViewModel, IAuthViewModel> parameters)
            {
                var patientViewModel = parameters.Item1;
                _authenticationViewModel = parameters.Item2;

                var patientDashboardControl = new PatientDashboardControl(patientViewModel);
                patientDashboardControl.LogoutButtonClicked += HandleLogoutRequested;

                this.Content = patientDashboardControl;
            }
        }

        private async void HandleLogoutRequested()
        {
            try
            {
                await _authenticationViewModel.Logout();

                if (App.MainWindow is LoginWindow loginWindow)
                {
                    loginWindow.ReturnToLogin();
                }
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
