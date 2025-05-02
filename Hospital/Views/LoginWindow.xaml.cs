// <copyright file="LoginWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Hospital.Repositories;
using Hospital.Services;

namespace Hospital
{
    using System;
    using Hospital.Exceptions;
    using Hospital.ViewModels;
    using Hospital.Views;
    using Microsoft.Data.SqlClient;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// The loggin window for the hospital application:
    /// Asks for a username and password
    /// If the user does not have an account there is a button for creating one.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {

        private readonly AuthViewModel loginPageViewModel;
        private Frame mainFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWindow"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public LoginWindow()
        {
            this.InitializeComponent();

            ILogInRepository logInService = new LogInRepository();
            IAuthService service = new AuthService(logInService);
            this.loginPageViewModel = new AuthViewModel(service);

            this.mainFrame = this.LoginFrame;

            this.LoginPanel.Visibility = Visibility.Visible;
            // Create login form page and navigate to it
            // this.mainFrame.Navigate(typeof(LoginPage), this.loginPageViewModel);
        }

        /// <summary>
        /// It gets the text inside the Username Text Block and the Password Text Block,
        /// and if the user is not existent it shows an error, otherwise:
        /// Depending on the user, if it is patient or not, it sends the user to the Patient Window
        /// or to the Doctor Window.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">..</param>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = this.UsernameTextField.Text;
            string password = this.PasswordTextField.Password;

            try
            {
                await this.loginPageViewModel.Login(username, password);

                this.LoginPanel.Visibility = Visibility.Collapsed;

                if (this.loginPageViewModel.GetUserRole() == "Patient")
                {
                    PatientService patientService = new PatientService();
                    PatientViewModel patientViewModel = new PatientViewModel(patientService, this.loginPageViewModel.AuthService.allUserInformation.UserId);

                    var parameters = new Tuple<IPatientViewModel, IAuthViewModel>(patientViewModel, this.loginPageViewModel);
                    this.mainFrame.Navigate(typeof(PatientDashboardPage), parameters);
                    return;
                }
                else if (this.loginPageViewModel.GetUserRole() == "Doctor")
                {
                    IDoctorRepository doctorRepository = new DoctorRepository();
                    IDoctorService doctorService = new DoctorService(doctorRepository);
                    IDoctorViewModel doctorViewModel = new DoctorViewModel(doctorService, this.loginPageViewModel.AuthService.allUserInformation.UserId);

                    var parameters = new Tuple<IDoctorViewModel, AuthViewModel>(doctorViewModel, this.loginPageViewModel);
                    this.mainFrame.Navigate(typeof(DoctorDashboardPage), parameters);
                    return;
                }
                else if (this.loginPageViewModel.GetUserRole() == "Admin")
                {
                    ILoggerRepository loggerRepository = new LoggerRepository();
                    var parameters = new Tuple<IAuthViewModel, ILoggerRepository>(this.loginPageViewModel, loggerRepository);
                    this.mainFrame.Navigate(typeof(AdminDashboardPage), parameters);
                    return;
                }

                LogoutWindow newLogOutWindow = new LogoutWindow(this.loginPageViewModel);
                newLogOutWindow.Activate();
                this.Close();
            }
            catch (AuthenticationException newAuthenticationException)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{newAuthenticationException.Message}",
                    CloseButtonText = "OK",
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
            catch (SqlException errorSQLException)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{errorSQLException.Message}",
                    CloseButtonText = "OK",
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoginPanel.Visibility = Visibility.Collapsed;
            this.mainFrame.Navigate(typeof(CreateAccountPage), this.loginPageViewModel);
        }

        /// <summary>
        /// Returns to login from page
        /// </summary>
        public void ReturnToLogin()
        {
            // Clear the frame and show login controls
            this.mainFrame.Content = null;
            this.LoginPanel.Visibility = Visibility.Visible;
        }
    }
}
