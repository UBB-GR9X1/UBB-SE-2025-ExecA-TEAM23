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
    public sealed partial class DoctorDashboardControl : UserControl
    {
        private DoctorViewModel _viewModel;

        // Constructor
        public DoctorDashboardControl()
        {
            this.InitializeComponent();
        }

        public DoctorDashboardControl(DoctorViewModel doctorViewModel)
        {
            this.InitializeComponent();
            _viewModel = doctorViewModel;
            this.DataContext = _viewModel; // Bind the ViewModel to the UserControl
        }

        // Update Button Click Handler
        private async void OnUpdateButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Update Doctor Name
            bool isNameUpdated = await _viewModel.UpdateDoctorName(_viewModel.DoctorName);
            if (isNameUpdated)
            {
                // Optionally show a message or do something on success
            }
            else
            {
                // Optionally show an error message if the update fails
            }

            // Update Department
            bool isDepartmentUpdated = await _viewModel.UpdateDepartment(_viewModel.DepartmentId);
            if (isDepartmentUpdated)
            {
                // Success message or action
            }
            else
            {
                // Error handling
            }

            // Update Career Info
            bool isCareerInfoUpdated = await _viewModel.UpdateCareerInfo(_viewModel.CareerInfo);
            if (isCareerInfoUpdated)
            {
                // Success message or action
            }
            else
            {
                // Error handling
            }

            // Update Avatar URL
            bool isAvatarUrlUpdated = await _viewModel.UpdateAvatarUrl(_viewModel.AvatarUrl);
            if (isAvatarUrlUpdated)
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

            // Update Email
            bool isEmailUpdated = await _viewModel.UpdateMail(_viewModel.Mail);
            if (isEmailUpdated)
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