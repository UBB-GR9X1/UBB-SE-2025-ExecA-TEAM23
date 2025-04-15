using Hospital.Models;
using Hospital.Services;
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
        private SearchDoctorsViewModel _searchDoctorsViewModel;
        private const string _initialDepartmentName = "Cardio";

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

            _searchDoctorsViewModel = new SearchDoctorsViewModel(_mockSearchDoctorsService.Object, _initialDepartmentName);
        }

        [Test]
        public async Task LoadDoctors_WhenCalled_CallsService()
        {
            await _searchDoctorsViewModel.LoadDoctors();

            _mockSearchDoctorsService.Verify(service => service.LoadDoctors(_initialDepartmentName), Times.Once);
        }

        [Test]
        public async Task LoadDoctors_WhenCalled_PopulatesDoctorListWithCorrectCount()
        {
            await _searchDoctorsViewModel.LoadDoctors();

            Assert.AreEqual(2, _searchDoctorsViewModel.Doctors.Count);
        }

        [Test]
        public async Task LoadDoctors_WhenCalled_PopulatesDoctorListWithCorrectData()
        {
            await _searchDoctorsViewModel.LoadDoctors();

            Assert.AreEqual("Dr. Jane Smith", _searchDoctorsViewModel.Doctors[0].DoctorName);
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

            _searchDoctorsViewModel.ShowDoctorProfile(doctor);

            Assert.AreEqual(doctor, _searchDoctorsViewModel.SelectedDoctor);
        }

        [Test]
        public void ShowDoctorProfile_WithValidDoctor_SetsIsProfileOpenToTrue()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Show", DepartmentName = "Demo" };

            _searchDoctorsViewModel.ShowDoctorProfile(doctor);

            Assert.IsTrue(_searchDoctorsViewModel.IsProfileOpen);
        }

        [Test]
        public void CloseDoctorProfile_WhenCalled_ResetsSelectedDoctorToDefault()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Close", DepartmentName = "Demo" };
            _searchDoctorsViewModel.ShowDoctorProfile(doctor);

            _searchDoctorsViewModel.CloseDoctorProfile();

            Assert.AreEqual(DoctorModel.Default, _searchDoctorsViewModel.SelectedDoctor);
        }

        [Test]
        public void CloseDoctorProfile_WhenCalled_SetsIsProfileOpenToFalse()
        {
            var doctor = new DoctorModel { DoctorName = "Dr. Close", DepartmentName = "Demo" };
            _searchDoctorsViewModel.ShowDoctorProfile(doctor);

            _searchDoctorsViewModel.CloseDoctorProfile();

            Assert.IsFalse(_searchDoctorsViewModel.IsProfileOpen);
        }

        [Test]
        public void DepartmentPartialName_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _searchDoctorsViewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_searchDoctorsViewModel.DepartmentPartialName))
                    propertyChangedFired = true;
            };

            _searchDoctorsViewModel.DepartmentPartialName = "NewDept";

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void DepartmentSearchTerm_GetterReturnsInitialValue_AfterInitialization()
        {
            Assert.AreEqual(_initialDepartmentName, _searchDoctorsViewModel.DepartmentPartialName);
        }

        [Test]
        public void SelectedDoctor_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _searchDoctorsViewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_searchDoctorsViewModel.SelectedDoctor))
                    propertyChangedFired = true;
            };

            _searchDoctorsViewModel.SelectedDoctor = new DoctorModel { DoctorName = "New" };

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void IsProfileOpen_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            _searchDoctorsViewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_searchDoctorsViewModel.IsProfileOpen))
                    propertyChangedFired = true;
            };

            _searchDoctorsViewModel.IsProfileOpen = true;

            Assert.IsTrue(propertyChangedFired);
        }

        [Test]
        public void Doctors_WhenChanged_TriggersPropertyChanged()
        {
            bool propertyChangedFired = false;
            var newList = new ObservableCollection<DoctorModel>();
            _searchDoctorsViewModel.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(_searchDoctorsViewModel.Doctors))
                    propertyChangedFired = true;
            };

            _searchDoctorsViewModel.GetType().GetProperty("Doctors")!
                .SetValue(_searchDoctorsViewModel, newList);

            Assert.IsTrue(propertyChangedFired);
        }


    }
}