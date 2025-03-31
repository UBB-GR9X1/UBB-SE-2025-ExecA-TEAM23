using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Hospital.ViewModels;
using Hospital.Models;
using System.Threading.Tasks;
using Hospital.DatabaseServices;

namespace Hospital.Views
{
    public sealed partial class RecommendationView : Page
    {
        private RecommendationSystemFormViewModel _formViewModel;
        private RecommendationSystemModel _recommendationSystem;

        public RecommendationView()
        {
            this.InitializeComponent();  // This initializes the XAML components

            _formViewModel = new RecommendationSystemFormViewModel();
            this.DataContext = _formViewModel;
            _recommendationSystem = new RecommendationSystemModel(new DoctorManagerModel());
        }

        private async void RecommendButton_Click(object sender, RoutedEventArgs e)
        {
            DoctorJointModel? recommendedDoctor = await _recommendationSystem.RecommendDoctor(_formViewModel);

            if (recommendedDoctor != null)
            {
                ResultTextBlock.Text = $"Recommended Doctor: {recommendedDoctor.GetDoctorName} (Rating: {recommendedDoctor.GetDoctorRating})";
                ResultTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ResultTextBlock.Text = "No suitable doctor found.";
                ResultTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
