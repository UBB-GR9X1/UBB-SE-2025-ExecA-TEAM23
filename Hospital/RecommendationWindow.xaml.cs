using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Hospital.Models;

namespace Hospital.Views
{
    public sealed partial class RecommendationWindow : Page
    {
        public RecommendationWindow()
        {
            this.InitializeComponent();
        }

        private void OpenRecommendationView_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to RecommendationView.xaml
            Frame.Navigate(typeof(RecommendationView));
        }

        // Receive recommendation result
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is DoctorJointModel recommendedDoctor && recommendedDoctor != null)
            {
                RecommendedDoctorTextBlock.Text = $"Recommended Doctor: {recommendedDoctor.GetDoctorName} (Rating: {recommendedDoctor.GetDoctorRating})";
                RecommendedDoctorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}

