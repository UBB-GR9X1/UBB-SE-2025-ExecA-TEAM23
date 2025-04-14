using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Hospital.Managers;
using Hospital.ViewModels;
using Hospital.Views;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital
{
    public sealed partial class MainWindow : Window
    {

        private readonly AuthViewModel _viewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            LogInDatabaseService logInService = new LogInDatabaseService();
            AuthManagerModel managerModel = new AuthManagerModel(logInService);
            _viewModel = new AuthViewModel(managerModel);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;

            try
            {
                await _viewModel.Login(username, password);


                if (_viewModel._authManagerModel._userInfo.Role == "Patient")
                {
                    PatientManagerModel patientManagerModel = new PatientManagerModel();
                    PatientViewModel patientViewModel = new PatientViewModel(patientManagerModel, _viewModel._authManagerModel._userInfo.UserId);
                    PatientDashboardWindow patientDashboardWindow = new PatientDashboardWindow(patientViewModel, _viewModel);
                    patientDashboardWindow.Activate();
                    this.Close();

                    return;
                }
                else if (_viewModel._authManagerModel._userInfo.Role == "Doctor")
                {
                    DoctorsDatabaseHelper doctorDbHelper = new DoctorsDatabaseHelper();
                    DoctorService doctorService = new DoctorService(doctorDbHelper);
                    DoctorViewModel doctorViewModel = new DoctorViewModel(doctorService, _viewModel._authManagerModel._userInfo.UserId);
                    DoctorDashboardWindow doctorDashboardWindow = new DoctorDashboardWindow(doctorViewModel, _viewModel);
                    doctorDashboardWindow.Activate();
                    this.Close();
                    return;
                }

                // Fallback for other roles (admin, etc.)
                LogoutWindow log = new LogoutWindow(_viewModel);
                log.Activate();
                // Show Logger window after successful login just for the presentation (uncomment when needed)
                // LoggerView logger = new LoggerView();
                // logger.Activate();
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{ex.Message}",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
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

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountView createAccWindow = new CreateAccountView(_viewModel);
            createAccWindow.Activate();
            this.Close();
        }
    }
}
