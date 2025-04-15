using Hospital.Services;

namespace Hospital.Views
{
    using Hospital.DatabaseServices;
    using Hospital.Managers;
    using Hospital.Models;
    using Hospital.ViewModels;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// The view class for recommendation system.
    /// </summary>
    public sealed partial class RecommendationView : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationView"/> class.
        /// </summary>
        public RecommendationView()
        {
            var doctorDbService = new DoctorsDatabaseHelper();
            DoctorService doctorManager = new DoctorService(doctorDbService);
            RecommendationSystemModel recommendationSystem = new RecommendationSystemModel(doctorManager);


            this.DataContext = new RecommendationSystemFormViewModel(recommendationSystem);
            this.InitializeComponent();
        }
    }
}
