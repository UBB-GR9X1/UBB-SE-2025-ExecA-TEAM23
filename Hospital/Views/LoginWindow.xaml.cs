// <copyright file="LoginWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital
{
    using System;
    using Hospital.DatabaseServices;
    using Hospital.Doctor_Dashboard;
    using Hospital.Exceptions;
    using Hospital.Managers;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWindow"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public LoginWindow()
        {
            this.InitializeComponent();
            ILogInDatabaseService logInService = new LogInDatabaseService();
            IAuthManagerModel managerModel = new AuthManagerModel(logInService);
            this.loginPageViewModel = new AuthViewModel(managerModel);
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


                if (this.loginPageViewModel.GetUserRole() == "Patient")
                {
                    PatientManagerModel patientManagerModel = new PatientManagerModel();
                    PatientViewModel patientViewModel = new PatientViewModel(patientManagerModel, this.loginPageViewModel.authManagerModel.allUserInformation.UserId);
                    PatientDashboardWindow patientDashboardWindow = new PatientDashboardWindow(patientViewModel, this.loginPageViewModel);
                    patientDashboardWindow.Activate();
                    this.Close();

                    return;
                }
                else if (this.loginPageViewModel.GetUserRole() == "Doctor")
                {
                    IDoctorsDatabaseHelper doctorsDatabaseHelper = new DoctorsDatabaseHelper();
                    IDoctorService doctorService = new DoctorService(doctorsDatabaseHelper);
                    IDoctorViewModel doctorViewModel = new DoctorViewModel(doctorService, this.loginPageViewModel.authManagerModel.allUserInformation.UserId);
                    DoctorDashboardWindow doctorDashboardWindow = new DoctorDashboardWindow(doctorViewModel, this.loginPageViewModel);
                    doctorDashboardWindow.Activate();
                    this.Close();
                    return;
                }
                else if (this.loginPageViewModel.GetUserRole() == "Admin")
                {
                    ILoggerDatabaseService loggerDatabaseService = new LoggerDatabaseService();

                    AdminDashboardWindow adminDashboard = new AdminDashboardWindow(
                        this.loginPageViewModel,
                        loggerDatabaseService);

                    adminDashboard.Activate();
                    this.Close();
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
            CreateAccountView createAccWindow = new CreateAccountView(this.loginPageViewModel);
            createAccWindow.Activate();
            this.Close();
        }
    }
}
