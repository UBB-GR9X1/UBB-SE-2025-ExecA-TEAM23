using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Hospital.Services;

namespace Hospital.Views
{
    public sealed partial class SearchDoctorsView : UserControl
    {
        public SearchDoctorsViewModel ViewModel { get; private set; }

        private CancellationTokenSource? _searchDebounceTokenSource;
        private const int SearchDebounceDelayMilliseconds = 300;
        private const string DefaultProfileImagePath = "ms-appx:///Assets/default-profile.png";

        public SearchDoctorsView()
        {
            this.InitializeComponent();

            var doctorSearchService = new SearchDoctorsService(new DoctorsDatabaseHelper());
            ViewModel = new SearchDoctorsViewModel(doctorSearchService, string.Empty);

            this.DataContext = ViewModel;

            ViewModel.PropertyChanged += OnViewModelPropertyChanged;

            _ = ViewModel.LoadDoctors();
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedDoctor) || e.PropertyName == nameof(ViewModel.IsProfileOpen))
            {
                RenderProfileView();
            }
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await PerformDebouncedSearch();
        }

        private async Task PerformDebouncedSearch()
        {
            _searchDebounceTokenSource?.Cancel();
            _searchDebounceTokenSource = new CancellationTokenSource();
            var cancellationToken = _searchDebounceTokenSource.Token;

            try
            {
                await Task.Delay(SearchDebounceDelayMilliseconds, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    ViewModel.DepartmentPartialName = SearchTextBox.Text;
                    await ViewModel.LoadDoctors();
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Error in search: {exception.Message}");
            }
        }

        private void DoctorsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e?.ClickedItem is DoctorModel selectedDoctor)
                {
                    ViewModel.ShowDoctorProfile(selectedDoctor);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing doctor profile: {exception.Message}");
            }
        }

        private void RenderProfileView()
        {
            try
            {
                var selectedDoctor = ViewModel.SelectedDoctor;

                if (selectedDoctor != DoctorModel.Default && ViewModel.IsProfileOpen)
                {
                    RenderDoctorProfileData(selectedDoctor);
                }
                else
                {
                    ClearProfileDisplay();
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating doctor profile UI: {exception.Message}");
            }
        }

        private void RenderDoctorProfileData(DoctorModel doctor)
        {
            LoadProfileImage(doctor.AvatarUrl);

            DoctorNameText.Text = doctor.DoctorName ?? "Doctor";
            DepartmentText.Text = doctor.DepartmentName ?? string.Empty;
            RatingText.Text = doctor.Rating.ToString();
            EmailText.Text = doctor.Mail ?? string.Empty;
            PhoneText.Text = doctor.PhoneNumber ?? string.Empty;
            CareerInfoText.Text = doctor.CareerInfo ?? string.Empty;
        }

        private void LoadProfileImage(string? imageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    Uri imageUri = new Uri(imageUrl);
                    DoctorProfileImage.Source = new BitmapImage(imageUri);
                }
                else
                {
                    DoctorProfileImage.Source = new BitmapImage(new Uri(DefaultProfileImagePath));
                }
            }
            catch
            {
                DoctorProfileImage.Source = new BitmapImage(new Uri(DefaultProfileImagePath));
            }
        }

        private void ClearProfileDisplay()
        {
            DoctorProfileImage.Source = new BitmapImage(new Uri(DefaultProfileImagePath));
            DoctorNameText.Text = string.Empty;
            DepartmentText.Text = string.Empty;
            RatingText.Text = string.Empty;
            EmailText.Text = string.Empty;
            PhoneText.Text = string.Empty;
            CareerInfoText.Text = string.Empty;
        }

        private void CloseProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseDoctorProfile();
        }

        private void ProfileOverlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((Grid)sender == ProfileOverlay)
            {
                ViewModel.CloseDoctorProfile();
                System.Diagnostics.Debug.WriteLine("Profile closed by overlay tap");
                e.Handled = true;
            }
        }

        private void ProfilePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}