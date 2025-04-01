using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;
using Hospital.Models;
using Hospital.Managers;
using Hospital.Views;

namespace Hospital
{
    public sealed partial class CreateAccountView : Window
    {

        private readonly AuthViewModel _viewModel;
        public CreateAccountView(AuthViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel;
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;
            string mail = EmailTextBox.Text;
            string name = NameTextBox.Text;
            string emergencyContact = EmergencyContactTextBox.Text;

            if (BirthDateCalendarPicker.Date.HasValue)
            {
                DateOnly birthDate = DateOnly.FromDateTime(BirthDateCalendarPicker.Date.Value.DateTime);
                BirthDateCalendarPicker.Date = new DateTimeOffset(birthDate.ToDateTime(TimeOnly.MinValue));

                string cnp = CNPTextBox.Text;

                BloodType? selectedBloodType = null;
                if (BloodTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
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
                        CloseButtonText = "OK"
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                    return;
                }

                bool weightValid = double.TryParse(WeightTextBox.Text, out double weight);
                bool heightValid = int.TryParse(HeightTextBox.Text, out int height);

                if (!weightValid || !heightValid || weight <= 0 || height <= 0)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please enter valid Weight (kg) and Height (cm).",
                        CloseButtonText = "OK"
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                    return;
                }

                try
                {
                    await _viewModel.CreateAccount(new UserCreateAccountModel(username, password, mail, name, birthDate, cnp, (BloodType)selectedBloodType, emergencyContact,weight, height));

                    PatientManagerModel patientManagerModel = new PatientManagerModel();
                    PatientViewModel patientViewModel = new PatientViewModel(patientManagerModel, _viewModel._authManagerModel._userInfo.UserId);
                    PatientDashboardWindow patientDashboardWindow = new PatientDashboardWindow(patientViewModel, _viewModel);
                    patientDashboardWindow.Activate();
                    this.Close();
                    return;

                }
                catch (AuthenticationException err)
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

                catch (SqlException)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Database Error",
                        CloseButtonText = "OK"
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                }
            }
            else
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Birth date is required.",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Activate();
            this.Close();
        }
    }
}
