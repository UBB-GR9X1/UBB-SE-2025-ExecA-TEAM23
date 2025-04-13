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

namespace Hospital.Views
{
    public sealed partial class SearchDoctorsView : UserControl
    {
        public SearchDoctorsViewModel ViewModel { get; private set; }

        // For debouncing search input
        private CancellationTokenSource? _searchDebounceTokenSource;
        private const int SearchDebounceDelayMilliseconds = 300;
        private const string DefaultProfileImagePath = "ms-appx:///Assets/default-profile.png";

        public SearchDoctorsView()
        {
            this.InitializeComponent();

            // This would normally be injected through dependency injection
            var doctorSearchService = new SearchDoctorsService(new DoctorsDatabaseHelper());
            ViewModel = new SearchDoctorsViewModel(doctorSearchService, string.Empty);

            this.DataContext = ViewModel;

            // Register for property changed events to update UI when SelectedDoctor changes
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;

            // Load initial empty search results
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
            // Cancel any previous search operation
            _searchDebounceTokenSource?.Cancel();
            _searchDebounceTokenSource = new CancellationTokenSource();
            var cancellationToken = _searchDebounceTokenSource.Token;

            try
            {
                // Wait before executing the search to avoid too many searches while typing
                await Task.Delay(SearchDebounceDelayMilliseconds, cancellationToken);

                // If the token was canceled during the delay, this won't execute
                if (!cancellationToken.IsCancellationRequested)
                {
                    ViewModel.DepartmentPartialName = SearchTextBox.Text;
                    await ViewModel.LoadDoctors();
                }
            }
            catch (TaskCanceledException)
            {
                // This is expected when debouncing - no need to handle
            }
            catch (Exception exception)
            {
                // Add general exception handling to catch any other errors
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
                    // The UI will be updated in the PropertyChanged event
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
            // Load profile image
            LoadProfileImage(doctor.AvatarUrl);

            // Populate UI text fields with doctor data
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
                // If image loading fails, use default image
                DoctorProfileImage.Source = new BitmapImage(new Uri(DefaultProfileImagePath));
            }
        }

        private void ClearProfileDisplay()
        {
            // Reset UI elements when no doctor is selected
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
            // Make sure this event is only triggered when tapping the overlay itself
            if ((Grid)sender == ProfileOverlay)
            {
                // Close profile when clicking outside the profile panel
                ViewModel.CloseDoctorProfile();
                System.Diagnostics.Debug.WriteLine("Profile closed by overlay tap");
                e.Handled = true;
            }
        }

        private void ProfilePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Prevent taps on the profile panel from reaching the overlay
            e.Handled = true;
        }
    }
}