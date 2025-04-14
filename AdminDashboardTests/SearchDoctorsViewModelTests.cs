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
        public async Task LoadDoctors_WhenCalled_CallsService()
        {
            await _viewModel.LoadDoctors();

            _mockSearchDoctorsService.Verify(service => service.LoadDoctors(InitialDepartmentName), Times.Once);
        }

        [Test]
        public async Task LoadDoctors_WhenCalled_PopulatesDoctorListWithCorrectCount()
        {
            await _viewModel.LoadDoctors();

            Assert.AreEqual(2, _viewModel.DoctorList.Count);
        }

        [Test]
        public async Task LoadDoctors_WhenCalled_PopulatesDoctorListWithCorrectData()
        {
            await _viewModel.LoadDoctors();

            Assert.AreEqual("Dr. Jane Smith", _viewModel.DoctorList[0].DoctorName);
        }

        [Test]
        public async Task LoadDoctors_WhenServiceThrowsException_DoesNotPropagateException()
        {
            _mockSearchDoctorsService.Setup(service => service.LoadDoctors(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test error"));

            var viewModel = new SearchDoctorsViewModel(_mockSearchDoctorsService.Object, "Any");

            Assert.DoesNotThrowAsync(async () => await viewModel.LoadDoctors());
        }

        [Test]
        public void ShowDoctorProfile_WithValidDoctor_SetsSelectedDoctor()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Show", DepartmentName = "Demo" };

            _viewModel.ShowDoctorProfile(doctor);

            Assert.AreEqual(doctor, _viewModel.SelectedDoctor);
        }

        [Test]
        public void ShowDoctorProfile_WithValidDoctor_SetsIsProfileOpenToTrue()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Show", DepartmentName = "Demo" };

            _viewModel.ShowDoctorProfile(doctor);

            Assert.IsTrue(_viewModel.IsProfileOpen);
        }

        [Test]
        public void CloseDoctorProfile_WhenCalled_ResetsSelectedDoctorToDefault()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Close", DepartmentName = "Demo" };
            _viewModel.ShowDoctorProfile(doctor);

            _viewModel.CloseDoctorProfile();

            Assert.AreEqual(DoctorModel.Default, _viewModel.SelectedDoctor);
        }

        [Test]
        public void CloseDoctorProfile_WhenCalled_SetsIsProfileOpenToFalse()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Close", DepartmentName = "Demo" };
            _viewModel.ShowDoctorProfile(doctor);

            _viewModel.CloseDoctorProfile();

            Assert.IsFalse(_viewModel.IsProfileOpen);
        }

        [Test]
        public void DepartmentPartialName_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _viewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_viewModel.DepartmentPartialName))
                    propertyChangedFired = true;
            };

            _viewModel.DepartmentPartialName = "NewDept";

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void SelectedDoctor_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _viewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_viewModel.SelectedDoctor))
                    propertyChangedFired = true;
            };

            _viewModel.SelectedDoctor = new DoctorModel { DoctorName = "New" };

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void IsProfileOpen_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _viewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_viewModel.IsProfileOpen))
                    propertyChangedFired = true;
            };

            _viewModel.IsProfileOpen = true;

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void DoctorList_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            var newList = new ObservableCollection<DoctorModel>();
            _viewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_viewModel.DoctorList))
                    propertyChangedFired = true;
            };

            _viewModel.GetType().GetProperty("DoctorList")!
                .SetValue(_viewModel, newList);

            Assert.IsTrue(propertyChangedFired);
        }
    }
}