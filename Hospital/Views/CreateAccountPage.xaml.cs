// <copyright file="CreateAccountPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital
{
    using System;
    using Hospital.Exceptions;
    using Hospital.Models;
    using Hospital.Services;
    using Hospital.ViewModels;
    using Hospital.Views;
    using Microsoft.Data.SqlClient;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;

    /// <summary>
    /// The create an account window for the hospital application:
    /// Asks for user information necessary for creating an account.
    /// Creates a new account if the information is valid, otherwise it throws exceptions that will be seen by the user.
    /// </summary>
    public sealed partial class CreateAccountPage : Page
    {

        private AuthViewModel viewModelCreateAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountPage"/> class.
        /// </summary>
        public CreateAccountPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountPage"/> class.
        /// </summary>
        /// <param name="authviewModel">View Model for Creating an account.</param>
        public CreateAccountPage(AuthViewModel authviewModel)
        {
            this.InitializeComponent();
            this.viewModelCreateAccount = authviewModel;
        }

        /// <summary>
        /// Handle navigation parameters
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is AuthViewModel authViewModel)
            {
                this.viewModelCreateAccount = authViewModel;
            }
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = this.UsernameField.Text;
            string password = this.PasswordField.Password;
            string mail = this.EmailTextBox.Text;
            string name = this.NameTextBox.Text;
            string emergencyContact = this.EmergencyContactTextBox.Text;

            if (this.BirthDateCalendarPicker.Date.HasValue)
            {
                DateOnly birthDate = DateOnly.FromDateTime(this.BirthDateCalendarPicker.Date.Value.DateTime);
                this.BirthDateCalendarPicker.Date = new DateTimeOffset(birthDate.ToDateTime(TimeOnly.MinValue));

                string cnp = this.CNPTextBox.Text;

                BloodType? selectedBloodType = null;
                if (this.BloodTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    string? selectedTag = selectedItem.Tag.ToString();
                    if (selectedTag != null && Enum.TryParse(selectedTag, out BloodType parsedBloodType))
                    {
                        selectedBloodType = parsedBloodType;
                    }
                }

                if (selectedBloodType == null)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please select a blood type.",
                        CloseButtonText = "OK",
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                    return;
                }

                bool weightValid = double.TryParse(this.WeightTextBox.Text, out double weight);
                bool heightValid = int.TryParse(this.HeightTextBox.Text, out int height);

                if (!weightValid || !heightValid || weight <= 0 || height <= 0)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please enter valid Weight (kg) and Height (cm).",
                        CloseButtonText = "OK",
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                    return;
                }

                try
                {
                    await this.viewModelCreateAccount.CreateAccount(new UserCreateAccountModel(username, password, mail, name, birthDate, cnp, (BloodType)selectedBloodType, emergencyContact, weight, height));

                    PatientService patientService = new PatientService();
                    PatientViewModel patientViewModel = new PatientViewModel(patientService, this.viewModelCreateAccount.AuthService.allUserInformation.UserId);
                    // Navigate to PatientDashboardPage
                    if (App.MainWindow is LoginWindow loginWindow)
                    {
                        var parameters = new Tuple<IPatientViewModel, IAuthViewModel>(patientViewModel, this.viewModelCreateAccount);
                        loginWindow.ReturnToLogin();
                        // Optionally navigate to patient dashboard if auto-login is desired
                        // loginWindow.mainFrame.Navigate(typeof(PatientDashboardPage), parameters);
                    }
                    return;

                }
                catch (AuthenticationException err)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"{err.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot,
                    };
                    await validationDialog.ShowAsync();
                }
                catch (SqlException)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Database Error",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot,
                    };
                    await validationDialog.ShowAsync();
                }
            }
            else
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Birth date is required.",
                    CloseButtonText = "OK",
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow is LoginWindow loginWindow)
            {
                loginWindow.ReturnToLogin();
            }
        }
    }
}
