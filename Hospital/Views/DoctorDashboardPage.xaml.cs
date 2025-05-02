using Hospital.Services;
using Hospital.Repositories;
using Hospital.Exceptions;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorDashboardPage : Page
    {
        private AuthViewModel _authViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorDashboardPage"/> class.
        /// </summary>
        public DoctorDashboardPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorDashboardPage"/> class with specific view models.
        /// </summary>
        /// <param name="doctorViewModel">The doctor view model.</param>
        /// <param name="authViewModel">The authentication view model.</param>
        public DoctorDashboardPage(IDoctorViewModel doctorViewModel, AuthViewModel authViewModel)
        {
            this.InitializeComponent();

            // Create the DoctorDashboardControl and set its DataContext
            DoctorDashboardControl doctorDashboardControl = new DoctorDashboardControl(doctorViewModel);

            // Add it to the grid
            this.Content = doctorDashboardControl; // DoctorDashboard is the x:Name of your Grid
            _authViewModel = authViewModel;
            doctorDashboardControl.LogoutButtonClicked += Logout; // Add the event handler for the Logout button

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<IDoctorViewModel, AuthViewModel> parameters)
            {
                var doctorViewModel = parameters.Item1;
                _authViewModel = parameters.Item2;

                // Create the control with the view model
                DoctorDashboardControl doctorDashboardControl = new DoctorDashboardControl(doctorViewModel);

                // Set it as the content
                this.Content = doctorDashboardControl;

                // Subscribe to the logout event
                doctorDashboardControl.LogoutButtonClicked += Logout;
            }
        }

        private async void Logout()
        {
            try
            {
                await _authViewModel.Logout(); // Log out the user

                if (App.MainWindow is LoginWindow loginWindow)
                {
                    loginWindow.ReturnToLogin();
                }
            }
            catch (AuthenticationException ex)
            {
                await ShowErrorDialog("Authentication Error", ex.Message);
            }
            catch (SqlException err)
            {
                await ShowErrorDialog("Database Error", err.Message);
            }
        }
        private async System.Threading.Tasks.Task ShowErrorDialog(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }

}
