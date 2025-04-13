using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.RecommendationSystemTests
{
    [TestClass]
    public class RecommendationSystemModelTests
    {
        [TestMethod]
        public async Task RecommendDoctorAsync_NoMatchingSymptoms_ReturnsNull()
        {
            // Arrange
            var mockDoctorManager = new Mock<DoctorManagerModel>();
            var model = new RecommendationSystemModel(mockDoctorManager.Object);

            var vm = new RecommendationSystemFormViewModel(model)
            {
                SelectedSymptomStart = "???",
                SelectedDiscomfortArea = "???",
                SelectedSymptomPrimary = "???"
            };

            // Act
            var result = await model.RecommendDoctorAsync(vm);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task RecommendDoctorAsync_ValidSymptoms_CallsDoctorManager()
        {
            // Arrange
            var mockDoctorManager = new Mock<DoctorManagerModel>();
            mockDoctorManager
                .Setup(m => m.GetDoctorsByDepartment(It.IsAny<int>()))
                .ReturnsAsync(new List<DoctorJointModel>());

            var model = new RecommendationSystemModel(mockDoctorManager.Object);

            var vm = new RecommendationSystemFormViewModel(model)
            {
                SelectedSymptomStart = "Suddenly",
                SelectedDiscomfortArea = "Chest",
                SelectedSymptomPrimary = "Pain"
            };

            // Act
            await model.RecommendDoctorAsync(vm);

            // Assert
            mockDoctorManager.Verify(m => m.GetDoctorsByDepartment(It.IsAny<int>()), Times.Once);
        }
    }
}
