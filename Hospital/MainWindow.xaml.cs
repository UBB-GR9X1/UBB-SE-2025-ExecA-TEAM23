using Hospital.Configs;
using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Hospital.Interfaces;
using Hospital.Managers;
using Hospital.ViewModels;
using Hospital.Views;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Hospital
{
    public sealed partial class MainWindow : Window
    {
        private readonly IAuthService _authService;
        private readonly IConfigProvider _configProvider;

        public MainWindow()
        {
            this.InitializeComponent();

            // Get the configuration instance that implements IConfigProvider
            _configProvider = Config.GetInstance();

            // Create services with configuration
            ILoginService loginService = new LogInDatabaseService(_configProvider);
            AuthManagerModel authManagerModel = new AuthManagerModel(loginService);

            // Create view model that implements IAuthService
            _authService = new AuthViewModel(authManagerModel);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;

            try
            {
                await _authService.Login(username, password);

                string userRole = _authService.GetUserRole();
                int userId = _authService.GetUserId();

                switch (userRole)
                {
                    case "Patient":
                        NavigateToPatientDashboard(userId);
                        break;
                    case "Doctor":
                        NavigateToDoctorDashboard(userId);
                        break;
                    case "Admin":
                        NavigateToAdminDashboard();
                        break;
                    default:
                        NavigateToLogoutWindow();
                        break;
                }
            }
            catch (Exception ex) when (ex is AuthenticationException || ex is SqlException)
            {
                await DisplayErrorDialog(ex.Message);
            }
        }

        private void NavigateToPatientDashboard(int userId)
        {
            // Create patient service using config provider
            IPatientService patientService = new PatientsDatabaseService(_configProvider);

            // Create manager model with service
            PatientManagerModel patientManagerModel = new PatientManagerModel(patientService);

            // Create view model with manager and ID
            PatientViewModel patientViewModel = new PatientViewModel(patientManagerModel, userId);

            // Navigate to dashboard window
            PatientDashboardWindow patientDashboardWindow = new PatientDashboardWindow(patientViewModel, _authService);
            patientDashboardWindow.Activate();
            this.Close();
        }

        private void NavigateToDoctorDashboard(int userId)
        {
            // Create doctor service using config provider
            IDoctorService doctorService = new DoctorsDatabaseService(_configProvider);

            // Create manager model with service
            DoctorManagerModel doctorManagerModel = new DoctorManagerModel(doctorService);

            // Create view model with manager and ID
            DoctorViewModel doctorViewModel = new DoctorViewModel(doctorManagerModel, userId);

            // Navigate to dashboard window
            DoctorDashboardWindow doctorDashboardWindow = new DoctorDashboardWindow(doctorViewModel, _authService);
            doctorDashboardWindow.Activate();
            this.Close();
        }

        private void NavigateToAdminDashboard()
        {
            // Create logger service using config provider
            ILoggerService loggerService = new LoggerDatabaseService(_configProvider);

            // Navigate to admin dashboard with auth service and logger service
            AdminDashboardWindow adminDashboard = new AdminDashboardWindow(_authService, loggerService);
            adminDashboard.Activate();
            this.Close();
        }

        private void NavigateToLogoutWindow()
        {
            // Navigate to logout window with auth service
            LogoutWindow logoutWindow = new LogoutWindow(_authService);
            logoutWindow.Activate();
            this.Close();
        }

        private async Task DisplayErrorDialog(string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK"
            };

            errorDialog.XamlRoot = this.Content.XamlRoot;
            await errorDialog.ShowAsync();
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to create account view with auth service
            CreateAccountView createAccountWindow = new CreateAccountView(_authService);
            createAccountWindow.Activate();
            this.Close();
        }
    }
}
