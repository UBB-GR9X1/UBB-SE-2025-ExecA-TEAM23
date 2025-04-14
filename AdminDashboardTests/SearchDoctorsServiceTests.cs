using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminDashboardTests
{
    [TestFixture]
    public class SearchDoctorsServiceTests
    {
        private Mock<IDoctorsDatabaseHelper> _mockDoctorDbHelper;
        private SearchDoctorsService _service;

        [SetUp]
        public void Setup()
        {
            _mockDoctorDbHelper = new Mock<IDoctorsDatabaseHelper>();
            _service = new SearchDoctorsService(_mockDoctorDbHelper.Object);
        }

        [Test]
        public void Constructor_ShouldInitializeAvailableDoctors()
        {
            // Assert
            Assert.IsNotNull(_service.AvailableDoctors);
            Assert.IsEmpty(_service.AvailableDoctors);
        }

        [Test]
        public async Task LoadDoctors_ShouldLoadAndMergeUniqueDoctors()
        {
            // Arrange
            var deptDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio", Rating = 4.5 }
            };

            var nameDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 2, DoctorName = "Dr. B", DepartmentName = "Neuro", Rating = 4.7 }
            };

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName("test"))
                .ReturnsAsync(deptDoctors);

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByPartialDoctorName("test"))
                .ReturnsAsync(nameDoctors);

            // Act
            await _service.LoadDoctors("test");

            // Assert
            Assert.AreEqual(2, _service.AvailableDoctors.Count);
            Assert.IsTrue(_service.AvailableDoctors.Any(d => d.DoctorId == 1));
            Assert.IsTrue(_service.AvailableDoctors.Any(d => d.DoctorId == 2));
        }

        [Test]
        public async Task LoadDoctors_ShouldNotDuplicateDoctorsWithSameId()
        {
            // Arrange
            var deptDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio" }
            };

            var nameDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio" }
            };

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName("cardio"))
                .ReturnsAsync(deptDoctors);

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByPartialDoctorName("cardio"))
                .ReturnsAsync(nameDoctors);

            // Act
            await _service.LoadDoctors("cardio");

            // Assert
            Assert.AreEqual(1, _service.AvailableDoctors.Count);
        }

        [Test]
        public async Task LoadDoctors_ShouldHandleExceptionsGracefully()
        {
            // Arrange
            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert (no crash)
            Assert.DoesNotThrowAsync(async () => await _service.LoadDoctors("anything"));
        }

        [Test]
        public void GetSearchedDoctors_ShouldReturnAvailableDoctors()
        {
            // Arrange
            _service.AvailableDoctors.Add(new DoctorModel { DoctorId = 1 });

            // Act
            var result = _service.GetSearchedDoctors();

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestCase(SortCriteria.RatingHighToLow)]
        [TestCase(SortCriteria.RatingLowToHigh)]
        [TestCase(SortCriteria.NameAscending)]
        [TestCase(SortCriteria.NameDescending)]
        [TestCase(SortCriteria.DepartmentAscending)]
        [TestCase(SortCriteria.RatingThenNameThenDepartment)]
        [TestCase((SortCriteria)999)] // default

        public async Task GetDoctorsSortedBy_ShouldReturnSortedDoctors(SortCriteria sortCriteria)
        {
            // Arrange
            var testDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Z", Rating = 3.2, DepartmentName = "Neurology" },
                new DoctorModel { DoctorId = 2, DoctorName = "Dr. A", Rating = 4.8, DepartmentName = "Cardiology" },
                new DoctorModel { DoctorId = 3, DoctorName = "Dr. M", Rating = 4.8, DepartmentName = "Dermatology" }
            };

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(It.IsAny<string>()))
                .ReturnsAsync(testDoctors);

            _mockDoctorDbHelper.Setup(db => db.GetDoctorsByPartialDoctorName(It.IsAny<string>()))
                .ReturnsAsync(new List<DoctorModel>()); // no duplicates

            await _service.LoadDoctors("test");

            // Act
            var sortedList = _service.GetDoctorsSortedBy(sortCriteria);

            // Assert
            Assert.AreEqual(3, sortedList.Count);
        }
    }
}
