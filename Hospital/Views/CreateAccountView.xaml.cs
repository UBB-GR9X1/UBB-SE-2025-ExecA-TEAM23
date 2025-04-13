using Hospital.Exceptions;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Hospital.Views;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Hospital.DatabaseServices;
using Hospital.Interfaces;
using System.Threading.Tasks;

namespace Hospital
{
    public sealed partial class CreateAccountView : Window
    {
        private readonly IAuthService _authService;
        private readonly IConfigProvider _configProvider;

        public CreateAccountView(IAuthService authService)
        {
            this.InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _configProvider = Hospital.Configs.Config.GetInstance();
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;
            string mail = EmailTextBox.Text;
            string name = NameTextBox.Text;
            string emergencyContact = EmergencyContactTextBox.Text;

            if (!BirthDateCalendarPicker.Date.HasValue)
            {
                await DisplayErrorDialogAsync("Birth date is required.");
                return;
            }

            DateOnly birthDate = DateOnly.FromDateTime(BirthDateCalendarPicker.Date.Value.DateTime);
            BirthDateCalendarPicker.Date = new DateTimeOffset(birthDate.ToDateTime(TimeOnly.MinValue));

            string cnp = CNPTextBox.Text;

            BloodType? selectedBloodType = null;
            if (BloodTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? selectedTag = selectedItem.Tag?.ToString();
                if (selectedTag != null && Enum.TryParse(selectedTag, out BloodType parsedBloodType))
                {
                    selectedBloodType = parsedBloodType;
                }
            }

            if (selectedBloodType == null)
            {
                await DisplayErrorDialogAsync("Please select a blood type.");
                return;
            }

            bool weightValid = double.TryParse(WeightTextBox.Text, out double weight);
            bool heightValid = int.TryParse(HeightTextBox.Text, out int height);

            if (!weightValid || !heightValid || weight <= 0 || height <= 0)
            {
                await DisplayErrorDialogAsync("Please enter valid Weight (kg) and Height (cm).");
                return;
            }

            try
            {
                UserCreateAccountModel userModel = new(
                    username, password, mail, name, birthDate, cnp,
                    (BloodType)selectedBloodType, emergencyContact, weight, height
                );

                await _authService.CreateAccount(userModel);

                // Create patient service using config provider
                IPatientService patientService = new PatientsDatabaseService(_configProvider);

                // Create manager model with service
                PatientManagerModel patientManagerModel = new PatientManagerModel(patientService);

                // Create view model with manager and user ID
                PatientViewModel patientViewModel = new PatientViewModel(patientManagerModel, _authService.GetUserId());

                // Navigate to dashboard window
                PatientDashboardWindow patientDashboardWindow = new PatientDashboardWindow(patientViewModel, _authService);
                patientDashboardWindow.Activate();
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                await DisplayErrorDialogAsync(ex.Message);
            }
            catch (SqlException)
            {
                await DisplayErrorDialogAsync("Database Error");
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainWindow();
        }

        private void NavigateToMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            this.Close();
        }

        private async Task DisplayErrorDialogAsync(string message)
        {
            var errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await errorDialog.ShowAsync();
        }
    }
}
