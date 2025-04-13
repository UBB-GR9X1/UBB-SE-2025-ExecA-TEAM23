using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.Interfaces;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientDashboardWindow : Window
    {
        private readonly IAuthService _authService;
        public PatientDashboardWindow(PatientViewModel patientViewModel, IAuthService authService)
        {
            this.InitializeComponent();
            
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            
            // Create the PatientDashboardControl and set its DataContext
            PatientDashboardControl patientDashboardControl = new PatientDashboardControl(patientViewModel);
            patientDashboardControl.LogoutButtonClicked += LogoutAsync; // Add the event handler for the Logout button
            
            // Add it to the grid
            PatientDashboard.Content = patientDashboardControl; // PatientDashboard is the x:Name of your Grid
        }

        private async void LogoutAsync()
        {
            try
            {
                await _authService.Logout(); // Log out the user
                NavigateToMainWindow();
            }
            catch (Exception ex) when (ex is AuthenticationException || ex is SqlException)
            {
                await DisplayErrorDialogAsync(ex.Message);
            }
        }

        private void NavigateToMainWindow()
        {
            MainWindow main = new MainWindow();
            main.Activate();
            this.Close();
        }

        private async Task DisplayErrorDialogAsync(string errorMessage)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = errorMessage,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await errorDialog.ShowAsync();
        }

    }
}
