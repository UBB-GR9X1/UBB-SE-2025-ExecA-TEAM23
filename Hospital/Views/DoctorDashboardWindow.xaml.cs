using Hospital.Exceptions;
using Hospital.Interfaces;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorDashboardWindow : Window
    {
        private readonly IAuthService _authService;
        public DoctorDashboardWindow(DoctorViewModel doctorViewModel, IAuthService authService)
        {
            this.InitializeComponent();

            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            // Create the DoctorDashboardControl and set its DataContext
            DoctorDashboardControl doctorDashboardControl = new DoctorDashboardControl(doctorViewModel);
            doctorDashboardControl.LogoutButtonClicked += LogoutAsync;

            // Add it to the grid
            DoctorDashboard.Content = doctorDashboardControl;
        }

        private async void LogoutAsync()
        {
            try
            {
                await _authService.Logout();
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
