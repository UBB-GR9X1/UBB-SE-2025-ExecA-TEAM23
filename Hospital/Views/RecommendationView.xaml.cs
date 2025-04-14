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
        public RecommendationView()
        {
            var doctorDbService = new DoctorsDatabaseService();
            DoctorManagerModel doctorManager = new DoctorManagerModel(doctorDbService);
            IRecommendationSystem recommendationSystem = new RecommendationSystemModel(doctorManager);


            this.DataContext = new RecommendationSystemFormViewModel(recommendationSystem);
            InitializeComponent();
        }
    }
}
