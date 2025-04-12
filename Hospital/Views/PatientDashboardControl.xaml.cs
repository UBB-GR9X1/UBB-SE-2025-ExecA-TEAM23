using Hospital.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.Views
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel? _patientViewModel;

        public event Action? Logout;

        public PatientDashboardControl()
        {
            InitializeComponent();
        }

        public PatientDashboardControl(PatientViewModel patientViewModel)
        {
            InitializeComponent();
            _patientViewModel = patientViewModel;
            DataContext = _patientViewModel;
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_patientViewModel == null)
                    throw new Exception("Patient is not initialized");

                bool hasChanges = false;
                bool passwordChanged = false;

                if (_patientViewModel.Name != _patientViewModel._originalPatient.PatientName)
                {
                    bool nameUpdated = await _patientViewModel.UpdateName(_patientViewModel.Name);
                    hasChanges |= nameUpdated;
                }

                if (_patientViewModel.Email != _patientViewModel._originalPatient.Email)
                {
                    bool emailUpdated = await _patientViewModel.UpdateEmail(_patientViewModel.Email);
                    hasChanges |= emailUpdated;
                }

                if (_patientViewModel.Username != _patientViewModel._originalPatient.Username)
                {
                    bool usernameUpdated = await _patientViewModel.UpdateUsername(_patientViewModel.Username);
                    hasChanges |= usernameUpdated;
                }

                if (_patientViewModel.Address != _patientViewModel._originalPatient.Address)
                {
                    bool addressUpdated = await _patientViewModel.UpdateAddress(_patientViewModel.Address);
                    hasChanges |= addressUpdated;
                }

                if (_patientViewModel.PhoneNumber != _patientViewModel._originalPatient.PhoneNumber)
                {
                    bool phoneUpdated = await _patientViewModel.UpdatePhoneNumber(_patientViewModel.PhoneNumber);
                    hasChanges |= phoneUpdated;
                }

                if (_patientViewModel.EmergencyContact != _patientViewModel._originalPatient.EmergencyContact)
                {
                    bool emergencyUpdated = await _patientViewModel.UpdateEmergencyContact(_patientViewModel.EmergencyContact);
                    hasChanges |= emergencyUpdated;
                }

                if (_patientViewModel.Weight != _patientViewModel._originalPatient.Weight)
                {
                    bool weightUpdated = await _patientViewModel.UpdateWeight(_patientViewModel.Weight);
                    hasChanges |= weightUpdated;
                }

                if (_patientViewModel.Height != _patientViewModel._originalPatient.Height)
                {
                    bool heightUpdated = await _patientViewModel.UpdateHeight(_patientViewModel.Height);
                    hasChanges |= heightUpdated;
                }

                if (_patientViewModel.Password != _patientViewModel._originalPatient.Password)
                {
                    bool passwordUpdated = await _patientViewModel.UpdatePassword(_patientViewModel.Password);
                    if (passwordUpdated)
                    {
                        hasChanges = true;
                        passwordChanged = true;
                    }
                }

                if (hasChanges)
                {
                    await _patientViewModel.LogUpdate(
                        _patientViewModel.UserId,
                        passwordChanged ? ActionType.CHANGE_PASSWORD : ActionType.UPDATE_PROFILE
                    );

                    await ShowDialogAsync("Success", "Changes applied successfully.");
                }
                else
                {
                    await ShowDialogAsync("No Changes", "Please modify the fields you want to update.");
                }
            }
            catch (Exception exception)
            {
                if (_patientViewModel != null)
                {
                    RestoreOriginalPatientData();
                    await ShowDialogAsync("Error", exception.Message);
                    await _patientViewModel.LoadPatientInfoByUserIdAsync(_patientViewModel.UserId);
                }
            }
        }

        private void RestoreOriginalPatientData()
        {
            var original = _patientViewModel!._originalPatient;
            _patientViewModel!.Name = original.PatientName;
            _patientViewModel.Email = original.Email;
            _patientViewModel.Username = original.Username;
            _patientViewModel.Address = original.Address;
            _patientViewModel.PhoneNumber = original.PhoneNumber;
            _patientViewModel.EmergencyContact = original.EmergencyContact;
            _patientViewModel.Weight = original.Weight;
            _patientViewModel.Height = original.Height;
            _patientViewModel.Password = original.Password;
        }

        private async Task ShowDialogAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            Logout?.Invoke();
        }
    }
}
