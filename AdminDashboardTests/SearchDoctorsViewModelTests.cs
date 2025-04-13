using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AdminDashboardTests
{
    [TestFixture]
    public class SearchDoctorsViewModelTests
    {
        private Mock<ISearchDoctorsService> _mockSearchDoctorsService;
        private SearchDoctorsViewModel _viewModel;
        private const string InitialDepartmentName = "Cardio";

        [SetUp]
        public void Setup()
        {
            _mockSearchDoctorsService = new Mock<ISearchDoctorsService>();

            var mockDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorName = "Dr. Jane Smith", DepartmentName = "Cardiology" },
                new DoctorModel { DoctorName = "Dr. John Doe", DepartmentName = "Cardiology" }
            };

            _mockSearchDoctorsService.Setup(service => service.LoadDoctors(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _mockSearchDoctorsService.Setup(service => service.GetSearchedDoctors())
                .Returns(mockDoctors);

            _viewModel = new SearchDoctorsViewModel(_mockSearchDoctorsService.Object, InitialDepartmentName);
        }

        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Assert
            Assert.IsNotNull(_viewModel.DoctorList);
            Assert.IsFalse(_viewModel.IsProfileOpen);
            Assert.AreEqual(InitialDepartmentName, _viewModel.DepartmentPartialName);
            Assert.AreEqual(DoctorModel.Default, _viewModel.SelectedDoctor);
        }

        [Test]
        public async Task LoadDoctors_ShouldCallServiceAndPopulateDoctorList()
        {
            // Act
            await _viewModel.LoadDoctors();

            // Assert
            _mockSearchDoctorsService.Verify(s => s.LoadDoctors(InitialDepartmentName), Times.Once);
            Assert.AreEqual(2, _viewModel.DoctorList.Count);
            Assert.AreEqual("Dr. Jane Smith", _viewModel.DoctorList[0].DoctorName);
        }

        [Test]
        public async Task LoadDoctors_ShouldHandleServiceExceptionGracefully()
        {
            // Arrange
            _mockSearchDoctorsService.Setup(service => service.LoadDoctors(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test error"));

            var vm = new SearchDoctorsViewModel(_mockSearchDoctorsService.Object, "Any");

            // Act & Assert (no exception should be thrown)
            Assert.DoesNotThrowAsync(async () => await vm.LoadDoctors());
        }

        [Test]
        public void ShowDoctorProfile_ShouldSetSelectedDoctorAndOpenProfile()
        {
            // Arrange
            var doctor = new DoctorModel { DoctorName = "Dr. Show", DepartmentName = "Demo" };

            // Act
            _viewModel.ShowDoctorProfile(doctor);

            // Assert
            Assert.AreEqual(doctor, _viewModel.SelectedDoctor);
            Assert.IsTrue(_viewModel.IsProfileOpen);
        }

        [Test]
        public void CloseDoctorProfile_ShouldResetSelectedDoctorAndCloseProfile()
        {
            // Arrange
            var doctor = new DoctorModel { DoctorName = "Dr. Close", DepartmentName = "Demo" };
            _viewModel.ShowDoctorProfile(doctor);

            // Act
            _viewModel.CloseDoctorProfile();

            // Assert
            Assert.AreEqual(DoctorModel.Default, _viewModel.SelectedDoctor);
            Assert.IsFalse(_viewModel.IsProfileOpen);
        }

        [Test]
        public void Setting_DepartmentPartialName_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var triggered = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.DepartmentPartialName)) triggered = true;
            };

            // Act
            _viewModel.DepartmentPartialName = "NewDept";

            // Assert
            Assert.IsTrue(triggered);
        }

        [Test]
        public void Setting_SelectedDoctor_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var triggered = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedDoctor)) triggered = true;
            };

            // Act
            _viewModel.SelectedDoctor = new DoctorModel { DoctorName = "New" };

            // Assert
            Assert.IsTrue(triggered);
        }

        [Test]
        public void Setting_IsProfileOpen_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var triggered = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.IsProfileOpen)) triggered = true;
            };

            // Act
            _viewModel.IsProfileOpen = true;

            // Assert
            Assert.IsTrue(triggered);
        }

        [Test]
        public void Setting_DoctorList_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var triggered = false;
            var newList = new ObservableCollection<DoctorModel>();
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.DoctorList)) triggered = true;
            };

            // Act
            _viewModel.GetType().GetProperty("DoctorList")!
                .SetValue(_viewModel, newList);

            // Assert
            Assert.IsTrue(triggered);
        }
    }
}
