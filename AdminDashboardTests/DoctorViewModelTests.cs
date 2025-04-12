using Hospital.Doctor_Dashboard;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using NUnit.Framework;

namespace AdminDashboardTests
{
    public class DoctorViewModelTests
    {
        private Mock<IDoctorService> _mockDoctorService;
        private DoctorViewModel _viewModel;
        private const int TestUserId = 123;

        [SetUp]
        public void Setup()
        {
            _mockDoctorService = new Mock<IDoctorService>();

            _mockDoctorService.Setup(service => service.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true); 

            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true); 

            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            _viewModel = new DoctorViewModel(_mockDoctorService.Object, TestUserId);
        }

        [Test]
        public void UpdateDoctorName_ShouldUpdateDoctorName_WhenValid()
        {
            var newName = "Dr. John Doe";

            var result = _viewModel.UpdateDoctorNameAsync(newName).Result;

            Assert.IsTrue(result);
            Assert.AreEqual(newName, _viewModel.DoctorName);
        }
    }
}
