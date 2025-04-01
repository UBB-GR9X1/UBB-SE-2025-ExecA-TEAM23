using Hospital.Exceptions;
using Hospital.Managers;
using Hospital.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Linq;

namespace Hospital.Views
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel _viewModel;
        public event Action LogoutButtonClicked;
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
                
                // Update Name
                if (string.IsNullOrWhiteSpace(_viewModel.Name) || _viewModel.Name.Any(char.IsDigit))
                    throw new InputProfileException("Name cannot be empty.");
                bool isNameUpdated = await _viewModel.UpdateName(_viewModel.Name);
                if (isNameUpdated)
                {
                    // Optionally show a message or do something on success
                }
                else
                {
                    // Optionally show an error message if the update fails
                }

                // Update Email
                if (string.IsNullOrWhiteSpace(_viewModel.Email) || !_viewModel.Email.Contains("@"))
                    throw new InputProfileException("Invalid email format.");
                bool isEmailUpdated = await _viewModel.UpdateEmail(_viewModel.Email);
                if (isEmailUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Username
                if (string.IsNullOrWhiteSpace(_viewModel.Username))
                    throw new InputProfileException("Username cannot be empty.");
                bool isUsernameUpdated = await _viewModel.UpdateUsername(_viewModel.Username);
                if (isUsernameUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Address
                if (string.IsNullOrWhiteSpace(_viewModel.Address))
                    throw new InputProfileException("Address cannot be empty.");
                bool isAddressUpdated = await _viewModel.UpdateAddress(_viewModel.Address);
                if (isAddressUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Phone Number
                if (string.IsNullOrWhiteSpace(_viewModel.PhoneNumber) || _viewModel.PhoneNumber.Length < 10 )
                    throw new InputProfileException("Invalid phone number.");
                bool isPhoneNumberUpdated = await _viewModel.UpdatePhoneNumber(_viewModel.PhoneNumber);
                if (isPhoneNumberUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Emergency Contact
                if (string.IsNullOrWhiteSpace(_viewModel.EmergencyContact) || _viewModel.EmergencyContact.Length < 10)
                    throw new InputProfileException("Invalid emergency contact.");
                bool isEmergencyContactUpdated = await _viewModel.UpdateEmergencyContact(_viewModel.EmergencyContact);
                if (isEmergencyContactUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Weight
                if(_viewModel.Weight <= 0 )
                    throw new InputProfileException("Weight must be greater than 0");
                bool isWeightUpdated = await _viewModel.UpdateWeight(_viewModel.Weight);
                if (isWeightUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                // Update Height
                if (_viewModel.Height <= 0)
                    throw new InputProfileException("Height must be greater than 0.");
                bool isHeightUpdated = await _viewModel.UpdateHeight(_viewModel.Height);
                if (isHeightUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }

                if (string.IsNullOrWhiteSpace(_viewModel.Password) || _viewModel.Password.Length < 6)
                    throw new InputProfileException("Password must be at least 6 characters long.");
                bool isPasswordUpdated = await _viewModel.UpdatePassword(_viewModel.Password);
                if (isPasswordUpdated)
                {
                    // Success message or action
                }
                else
                {
                    // Error handling
                }
            }
            catch (InputProfileException ex)
            {
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
            // Add additional handling (e.g., show a message to the user after all fields are updated)
        }
        private void Logout(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }

    }
}
