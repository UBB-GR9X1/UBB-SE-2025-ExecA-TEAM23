using Hospital.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Hospital.Models;

namespace Hospital.RecommandationSystemTests
{
    [TestClass]
    public class RecommendationSystemFormViewModelTests
    {
        [TestMethod]
        public void ValidateSymptomSelection_AllUnique_ReturnsTrue()
        {
            // Arrange
            var mockSystem = new Mock<IRecommendationSystem>();
            var vm = new RecommendationSystemFormViewModel(mockSystem.Object)
            {
                SelectedSymptomStart = "Pain",
                SelectedDiscomfortArea = "Numbness",
                SelectedSymptomPrimary = "Coloration",
                SelectedSymptomSecondary = "Inflammation",
                SelectedSymptomTertiary = "Tenderness"
            };

            // Act
            var result = vm.ValidateSymptomSelection();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateSymptomSelection_Duplicates_ReturnsFalse()
        {
            // Arrange
            var mockSystem = new Mock<IRecommendationSystem>();
            var vm = new RecommendationSystemFormViewModel(mockSystem.Object)
            {
                SelectedSymptomStart = "Pain",
                SelectedDiscomfortArea = "Pain",
                SelectedSymptomPrimary = "Pain",
                SelectedSymptomSecondary = "Pain",
                SelectedSymptomTertiary = "Pain"
            };

            // Act
            var result = vm.ValidateSymptomSelection();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
