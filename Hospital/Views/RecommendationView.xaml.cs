using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Hospital.ViewModels;
using Hospital.Models;
using Hospital.Managers;
using Hospital.DatabaseServices;
using System;
using System.Threading.Tasks;

namespace Hospital.Views
{
    public sealed partial class RecommendationView : Page
    {
        private readonly RecommendationSystemFormViewModel _symptomFormViewModel;
        private readonly RecommendationSystemModel _doctorRecommendationSystem;

        public RecommendationView()
        {
            _symptomFormViewModel = new RecommendationSystemFormViewModel();
            DataContext = _symptomFormViewModel;
            InitializeComponent();

            var doctorDatabaseService = new DoctorsDatabaseService();
            _doctorRecommendationSystem = new RecommendationSystemModel(new DoctorManagerModel(doctorDatabaseService));
        }

        private async void OnRecommendButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button) button.IsEnabled = false;

                var recommendedDoctor = await _doctorRecommendationSystem.RecommendDoctorBasedOnSymptomsAsync(_symptomFormViewModel);

                if (recommendedDoctor != null)
                {
                    DoctorNameText.Text = $"Doctor: {recommendedDoctor.GetDoctorName()}";
                    DepartmentText.Text = $"Department: {recommendedDoctor.GetDoctorDepartment()}";
                    RatingText.Text = $"Rating: {recommendedDoctor.GetDoctorRating():0.0}";
                }
                else
                {
                    DoctorNameText.Text = "No suitable doctor found";
                    DepartmentText.Text = string.Empty;
                    RatingText.Text = string.Empty;
                }

                ResultPanel.Visibility = Visibility.Visible;
            }
            catch (Exception exception)
            {
                DoctorNameText.Text = "Error during recommendation";
                DepartmentText.Text = exception.Message;
                RatingText.Text = string.Empty;
                ResultPanel.Visibility = Visibility.Visible;
            }
            finally
            {
                if (sender is Button button) button.IsEnabled = true;
            }
        }
    }
}
