using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Hospital.ViewModels;
using Hospital.Models;
using Hospital.Managers;

using System.Threading.Tasks;
using Hospital.DatabaseServices;
using System;
using Hospital.Configs;
using Hospital.Interfaces;

namespace Hospital.Views
{
    public sealed partial class RecommendationView : Page
    {
        private readonly RecommendationSystemFormViewModel _formViewModel;
        private readonly RecommendationSystemModel _recommendationSystem;

        public RecommendationView()
        {
            _formViewModel = new RecommendationSystemFormViewModel();
            this.DataContext = _formViewModel;
            this.InitializeComponent();

            IConfigProvider configProvider = Config.GetInstance();
            var doctorDBService = new DoctorsDatabaseService(configProvider);

            _recommendationSystem = new RecommendationSystemModel(new DoctorManagerModel(doctorDBService));

        }

        private async void RecommendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button) button.IsEnabled = false;

                var recommendedDoctor = await _recommendationSystem.RecommendDoctor(_formViewModel);

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
            catch (Exception ex)
            {
                DoctorNameText.Text = "Error getting recommendation";
                DepartmentText.Text = ex.Message;
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