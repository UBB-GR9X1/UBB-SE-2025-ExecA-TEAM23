using Hospital.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Hospital.Views
{
    public sealed partial class DoctorDashboardControl : UserControl
    {
        private DoctorViewModel? _viewModel;
        public event Action? LogoutButtonClicked;

        public DoctorDashboardControl()
        {
            InitializeComponent();
        }

        public DoctorDashboardControl(DoctorViewModel doctorViewModel)
        {
            InitializeComponent();
            _viewModel = doctorViewModel;
            this.DataContext = _viewModel;

            // Refresh data when loaded
            this.Loaded += async (sender, e) =>
            {
                await _viewModel.LoadDoctorInfoByUserIdAsync(_viewModel.UserId);
            };
        }

        // Update Button Click Handler
        private async void OnUpdateButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {

                if (_viewModel == null)
                    throw new Exception("Doctor is not initialized");

                bool changeMade = false;

                // Update Doctor Name
                if (_viewModel.DoctorName != _viewModel._originalDoctor.DoctorName)
                {
                    bool isNameUpdated = await _viewModel.UpdateDoctorName(_viewModel.DoctorName);
                    if (isNameUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Department
                if (_viewModel.DepartmentName != _viewModel._originalDoctor.DepartmentName)
                {
                    bool isDepartmentUpdated = await _viewModel.UpdateDepartment(_viewModel.DepartmentId);
                    if (isDepartmentUpdated)
                    {
                        changeMade = true;
                    }
                }
                // Update Career Info
                if (_viewModel.CareerInfo != _viewModel._originalDoctor.CareerInfo)
                {
                    bool isCareerInfoUpdated = await _viewModel.UpdateCareerInfo(_viewModel.CareerInfo);
                    if (isCareerInfoUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Avatar URL
                if (_viewModel.AvatarUrl != _viewModel._originalDoctor.AvatarUrl)
                {
                    bool isAvatarUrlUpdated = await _viewModel.UpdateAvatarUrl(_viewModel.AvatarUrl);
                    if (isAvatarUrlUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Phone Number
                if (_viewModel.PhoneNumber != _viewModel._originalDoctor.PhoneNumber)
                {
                    bool isPhoneNumberUpdated = await _viewModel.UpdatePhoneNumber(_viewModel.PhoneNumber);
                    if (isPhoneNumberUpdated)
                    {
                        changeMade = true;
                    }
                }

                // Update Email
                if (_viewModel.Mail != _viewModel._originalDoctor.Mail)
                {
                    bool isEmailUpdated = await _viewModel.UpdateMail(_viewModel.Mail);
                    if (isEmailUpdated)
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
                if (_viewModel != null)
                {
                    _viewModel.DoctorName = _viewModel._originalDoctor.DoctorName;
                    _viewModel.DepartmentName = _viewModel._originalDoctor.DepartmentName;
                    _viewModel.CareerInfo = _viewModel._originalDoctor.CareerInfo;
                    _viewModel.AvatarUrl = _viewModel._originalDoctor.AvatarUrl;
                    _viewModel.PhoneNumber = _viewModel._originalDoctor.PhoneNumber;
                    _viewModel.Mail = _viewModel._originalDoctor.Mail;
                }
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await validationDialog.ShowAsync();
            }
        }

        private void OnLogOutButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }
    }
}
