using Hospital.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using Hospital.Doctor_Dashboard;

namespace Hospital.Views
{
    public sealed partial class DoctorDashboardControl : UserControl
    {
        private IDoctorViewModel? _viewModel;

        public event Action? LogoutButtonClicked;

        public DoctorDashboardControl()
        {
            InitializeComponent();
        } 

        public DoctorDashboardControl(IDoctorViewModel doctorViewModel)
        {
            InitializeComponent();
            _viewModel = doctorViewModel;
            this.DataContext = _viewModel;

            // Refresh data when loaded
            this.Loaded += async (sender, e) =>
            {
                await this._viewModel.LoadDoctorInformationAsync(this._viewModel.UserId);
            };
        }

        // Update Button Click Handler
        private async void OnUpdateButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (this._viewModel == null)
            {
                return;
            }

            var (success, errorMessage) = await this._viewModel.TryUpdateDoctorProfileAsync();

            var dialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                CloseButtonText = "OK"
            };

            if (!string.IsNullOrEmpty(errorMessage))
            {
                dialog.Title = "Error";
                dialog.Content = errorMessage;
            }
            else if (success)
            {
                dialog.Title = "Success";
                dialog.Content = "Changes applied successfully.";
            }
            else
            {
                dialog.Title = "No changes made";
                dialog.Content = "Please modify the fields you want to update.";
            }

            await dialog.ShowAsync();
        }

        private void OnLogOutButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }
    }
}
