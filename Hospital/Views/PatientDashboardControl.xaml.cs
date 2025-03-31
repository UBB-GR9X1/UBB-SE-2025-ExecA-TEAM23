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
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace Hospital.Views
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel _viewModel;

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
            // Update Name
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
            bool isHeightUpdated = await _viewModel.UpdateHeight(_viewModel.Height);
            if (isHeightUpdated)
            {
                // Success message or action
            }
            else
            {
                // Error handling
            }


            // Optionally, handle password update if needed, using similar logic
            bool isPasswordUpdated = await _viewModel.UpdatePassword(_viewModel.Password);
            if (isPasswordUpdated)
            {
                // Success message or action
            }
            else
            {
                // Error handling
            }

            // Add additional handling (e.g., show a message to the user after all fields are updated)
        }

    }
}
