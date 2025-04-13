using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;


namespace Hospital.Views
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel? _viewModel;
        public event Action? LogoutButtonClicked;
        // Constructor
        public PatientDashboardControl()
        {
            this.InitializeComponent();
        }
        public PatientDashboardControl(PatientViewModel patientViewModel)
        {
            this.InitializeComponent();
            _viewModel = patientViewModel;

            this.DataContext = _viewModel; // Bind the ViewModel to the UserControl
        }

        // Update Button Click Handler
        private async void OnUpdateButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                bool changeMade = false;

                if (_viewModel == null)
                    throw new Exception("Patient is not initialized");

                // Update Name
                if (_viewModel.Name != _viewModel.OriginalPatient.PatientName)
                {
                    bool isNameUpdated = await _viewModel.UpdateName(_viewModel.Name);
                    if (isNameUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Email
                if (_viewModel.Email != _viewModel.OriginalPatient.Mail)
                {
                    bool isEmailUpdated = await _viewModel.UpdateEmail(_viewModel.Email);
                    if (isEmailUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Username
                if (_viewModel.Username != _viewModel.OriginalPatient.Username)
                {
                    bool isUsernameUpdated = await _viewModel.UpdateUsername(_viewModel.Username);
                    if (isUsernameUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Address
                if (_viewModel.Address != _viewModel.OriginalPatient.Address)
                {
                    bool isAddressUpdated = await _viewModel.UpdateAddress(_viewModel.Address);
                    if (isAddressUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Phone Number
                if (_viewModel.PhoneNumber != _viewModel.OriginalPatient.PhoneNumber)
                {
                    bool isPhoneNumberUpdated = await _viewModel.UpdatePhoneNumber(_viewModel.PhoneNumber);
                    if (isPhoneNumberUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Emergency Contact
                if (_viewModel.EmergencyContact != _viewModel.OriginalPatient.EmergencyContact)
                {
                    bool isEmergencyContactUpdated = await _viewModel.UpdateEmergencyContact(_viewModel.EmergencyContact);
                    if (isEmergencyContactUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Weight
                if (_viewModel.Weight != _viewModel.OriginalPatient.Weight)
                {
                    bool isWeightUpdated = await _viewModel.UpdateWeight(_viewModel.Weight);
                    if (isWeightUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Height
                if (_viewModel.Height != _viewModel.OriginalPatient.Height)
                {
                    bool isHeightUpdated = await _viewModel.UpdateHeight(_viewModel.Height);
                    if (isHeightUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Password
                bool passwordChanged = false;
                if (_viewModel.Password != _viewModel.OriginalPatient.Password)
                {
                    bool isPasswordUpdated = await _viewModel.UpdatePassword(_viewModel.Password);
                    if (isPasswordUpdated)
                    {
                        changeMade = true;
                        passwordChanged = true;
                    }
                }

                if (changeMade)
                {
                    if (passwordChanged)
                        await _viewModel.LogUpdate(_viewModel.UserId, ActionType.CHANGE_PASSWORD);
                    else
                        await _viewModel.LogUpdate(_viewModel.UserId, ActionType.UPDATE_PROFILE);
                    var validationDialog = new ContentDialog
                    {
                        Title = "Success",
                        Content = "Changes applied successfully",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await validationDialog.ShowAsync();
                }

                else
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "No changes made",
                        Content = "Please modify the fields you want to update",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await validationDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                if (_viewModel != null)
                {
                    _viewModel.Name = _viewModel.OriginalPatient.PatientName;
                    _viewModel.Email = _viewModel.OriginalPatient.Mail;
                    _viewModel.Username = _viewModel.OriginalPatient.Username;
                    _viewModel.Address = _viewModel.OriginalPatient.Address;
                    _viewModel.PhoneNumber = _viewModel.OriginalPatient.PhoneNumber;
                    _viewModel.EmergencyContact = _viewModel.OriginalPatient.EmergencyContact;
                    _viewModel.Weight = _viewModel.OriginalPatient.Weight;
                    _viewModel.Height = _viewModel.OriginalPatient.Height;
                    _viewModel.Password = _viewModel.OriginalPatient.Password;

                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"{ex.Message}",
                        CloseButtonText = "OK"
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                    await _viewModel.LoadPatientInfoByUserIdAsync(_viewModel.UserId);
                }
            }
            // Add additional handling (e.g., show a message to the user after all fields are updated)
        }
        private void Logout(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }

    }
}
