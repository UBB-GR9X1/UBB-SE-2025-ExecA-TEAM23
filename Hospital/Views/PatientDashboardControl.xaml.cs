using Hospital.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;


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
                bool changeMade = false;

                // Update Name
                if (_viewModel.Name != _viewModel._originalPatient.PatientName)
                {
                    bool isNameUpdated = await _viewModel.UpdateName(_viewModel.Name);
                    if (isNameUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Email
                if (_viewModel.Email != _viewModel._originalPatient.Mail)
                {
                    bool isEmailUpdated = await _viewModel.UpdateEmail(_viewModel.Email);
                    if (isEmailUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Username
                if (_viewModel.Username != _viewModel._originalPatient.Username)
                {
                    bool isUsernameUpdated = await _viewModel.UpdateUsername(_viewModel.Username);
                    if (isUsernameUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Address
                if (_viewModel.Address != _viewModel._originalPatient.Address)
                {
                    bool isAddressUpdated = await _viewModel.UpdateAddress(_viewModel.Address);
                    if (isAddressUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Phone Number
                if (_viewModel.PhoneNumber != _viewModel._originalPatient.PhoneNumber)
                {
                    bool isPhoneNumberUpdated = await _viewModel.UpdatePhoneNumber(_viewModel.PhoneNumber);
                    if (isPhoneNumberUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Emergency Contact
                if (_viewModel.EmergencyContact != _viewModel._originalPatient.EmergencyContact)
                {
                    bool isEmergencyContactUpdated = await _viewModel.UpdateEmergencyContact(_viewModel.EmergencyContact);
                    if (isEmergencyContactUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Weight
                if (_viewModel.Weight != _viewModel._originalPatient.Weight)
                {
                    bool isWeightUpdated = await _viewModel.UpdateWeight(_viewModel.Weight);
                    if (isWeightUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Height
                if (_viewModel.Height != _viewModel._originalPatient.Height)
                {
                    bool isHeightUpdated = await _viewModel.UpdateHeight(_viewModel.Height);
                    if (isHeightUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Password
                if (_viewModel.Password != _viewModel._originalPatient.Password)
                {
                    bool isPasswordUpdated = await _viewModel.UpdatePassword(_viewModel.Password);
                    if (isPasswordUpdated)
                    {
                        changeMade = true;
                    }
                }

                if (changeMade)
                {
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
                _viewModel.Name = _viewModel._originalPatient.PatientName;
                _viewModel.Email = _viewModel._originalPatient.Mail;
                _viewModel.Username = _viewModel._originalPatient.Username;
                _viewModel.Address = _viewModel._originalPatient.Address;
                _viewModel.PhoneNumber = _viewModel._originalPatient.PhoneNumber;
                _viewModel.EmergencyContact = _viewModel._originalPatient.EmergencyContact;
                _viewModel.Weight = _viewModel._originalPatient.Weight;
                _viewModel.Height = _viewModel._originalPatient.Height;
                _viewModel.Password = _viewModel._originalPatient.Password;
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
