using Hospital.Doctor_Dashboard;
using Hospital.Exceptions;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorDashboardWindow : Window
    {
        private readonly AuthViewModel _authViewModel;
        public DoctorDashboardWindow(IDoctorViewModel doctorViewModel, AuthViewModel authViewModel)
        {
            this.InitializeComponent();

            // Create the DoctorDashboardControl and set its DataContext
            DoctorDashboardControl doctorDashboardControl = new DoctorDashboardControl(doctorViewModel);

            // Add it to the grid
            DoctorDashboard.Content = doctorDashboardControl; // DoctorDashboard is the x:Name of your Grid
            _authViewModel = authViewModel;
            doctorDashboardControl.LogoutButtonClicked += Logout; // Add the event handler for the Logout button

        }

        private async void Logout()
        {
            try
            {
                await _authViewModel.Logout(); // Log out the user
                MainWindow main = new MainWindow();
                main.Activate();
                this.Close(); // Close logout window after successful logout
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
