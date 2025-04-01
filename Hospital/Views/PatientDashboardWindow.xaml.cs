using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Hospital.Exceptions;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientDashboardWindow : Window
    {
        private readonly AuthViewModel _authViewModel;
        public PatientDashboardWindow(PatientViewModel patientViewModel, AuthViewModel authViewModel )
        {
            this.InitializeComponent();
            // Create the PatientDashboardControl and set its DataContext
            PatientDashboardControl patientDashboardControl = new PatientDashboardControl(patientViewModel);
            _authViewModel = authViewModel;
            patientDashboardControl.LogoutButtonClicked += Logout; // Add the event handler for the Logout button
            // Add it to the grid
            PatientDashboard.Content = patientDashboardControl; // PatientDashboard is the x:Name of your Grid

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
