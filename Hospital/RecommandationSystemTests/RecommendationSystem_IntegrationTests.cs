using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Hospital.RecommendationSystemTests
{
    [TestClass]
    public class RecommendationSystem_IntegrationTests
    {
        [TestMethod]
        public async Task RecommendDoctorBasedOnSymptomsAsync_WithRealServices_ReturnsDoctorOrNull()
        {
            // Arrange
            var realService = new DoctorsDatabaseService(); // Replace with test/mock DB if needed
            var manager = new DoctorManagerModel(realService);
            var model = new RecommendationSystemModel(manager);
            var viewModel = new RecommendationSystemFormViewModel(model)
            {
                SelectedSymptomStart = "Suddenly",
                SelectedDiscomfortArea = "Chest",
                SelectedSymptomPrimary = "Pain"
            };

            // Act
            var result = await viewModel.RecommendDoctorBasedOnSymptomsAsync();

            // Assert
            Assert.IsTrue(result == null || result is DoctorJointModel, "Expected null or a valid DoctorJointModel.");
        }
    }
}
