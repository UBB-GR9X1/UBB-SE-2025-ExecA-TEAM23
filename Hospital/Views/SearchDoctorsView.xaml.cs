using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

using Hospital.ViewModels;
using Hospital.Managers;
using Hospital.Models;
using Hospital.DatabaseServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hospital.Views
{
    public sealed partial class SearchDoctorsView : UserControl
    {
        public SearchDoctorsViewModel ViewModel { get; private set; }

        // For debouncing search input
        private CancellationTokenSource _debounceTokenSource;
        private readonly int _debounceDelay = 300; // milliseconds

        public SearchDoctorsView()
        {
            this.InitializeComponent();

            // This would normally be injected through dependency injection
            var searchManager = new SearchDoctorsManagerModel(new SearchDoctorsDatabaseService());
            ViewModel = new SearchDoctorsViewModel(searchManager, string.Empty);

            this.DataContext = ViewModel;

            // Register for property changed events to update UI when SelectedDoctor changes
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Load initial empty search results
            _ = ViewModel.LoadDoctors();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedDoctor) || e.PropertyName == nameof(ViewModel.IsProfileOpen))
            {
                UpdateDoctorProfileUI();
            }
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel any previous search operation
            _debounceTokenSource?.Cancel();
            _debounceTokenSource = new CancellationTokenSource();
            var token = _debounceTokenSource.Token;

            try
            {
                // Wait before executing the search to avoid too many searches while typing
                await Task.Delay(_debounceDelay, token);

                // If the token was canceled during the delay, this won't execute
                if (!token.IsCancellationRequested)
                {
                    ViewModel.DepartmentPartialName = SearchTextBox.Text;
                    await ViewModel.LoadDoctors();
                }
            }
            catch (Exception ex)
            {
                // Add general exception handling to catch any other errors
                System.Diagnostics.Debug.WriteLine($"Error in search: {ex.Message}");
            }
        }

        private void DoctorsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e?.ClickedItem is DoctorDisplayModel doctor)
                {
                    ViewModel.ShowDoctorProfile(doctor);
                    // The UI will be updated in the PropertyChanged event
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing doctor profile: {ex.Message}");
            }
        }

        private void UpdateDoctorProfileUI()
        {
            try
            {
                var doctor = ViewModel.SelectedDoctor;
                if (doctor != null && ViewModel.IsProfileOpen)
                {
                    // Set doctor profile image
                    try
                    {
                        if (!string.IsNullOrEmpty(doctor.AvatarUrl))
                        {
                            Uri imageUri = new Uri(doctor.AvatarUrl);
                            DoctorProfileImage.Source = new BitmapImage(imageUri);
                        }
                        else
                        {
                            DoctorProfileImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/default-profile.png"));
                        }
                    }
                    catch
                    {
                        // If image loading fails, use default image
                        DoctorProfileImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/default-profile.png"));
                    }

                    // Set text values
                    DoctorNameText.Text = doctor.DoctorName ?? "Doctor";
                    DepartmentText.Text = doctor.DepartmentName ?? string.Empty;
                    RatingText.Text = doctor.Rating.ToString();
                    EmailText.Text = doctor.Mail ?? string.Empty;
                    PhoneText.Text = doctor.PhoneNumber ?? string.Empty;
                    CareerInfoText.Text = doctor.CareerInfo ?? string.Empty;

                    // For debugging
                    System.Diagnostics.Debug.WriteLine($"Displaying doctor: {doctor.DoctorName}, {doctor.DepartmentName}");
                }
                else
                {
                    // Clear UI if no doctor is selected or profile is closed
                    DoctorProfileImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/default-profile.png"));
                    DoctorNameText.Text = string.Empty;
                    DepartmentText.Text = string.Empty;
                    RatingText.Text = string.Empty;
                    EmailText.Text = string.Empty;
                    PhoneText.Text = string.Empty;
                    CareerInfoText.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating doctor profile UI: {ex.Message}");
            }
        }

        private void CloseProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseDoctorProfile();
        }

        private void ProfileOverlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Make sure this event is only triggered when tapping the overlay itself
            if (sender == ProfileOverlay)
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
