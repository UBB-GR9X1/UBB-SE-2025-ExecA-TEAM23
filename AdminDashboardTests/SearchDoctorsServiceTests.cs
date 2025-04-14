using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.Models;
using Moq;

namespace AdminDashboardTests
{
    [TestFixture]
    public class SearchDoctorsServiceTests
    {
        private Mock<IDoctorsDatabaseHelper> _mockDatabaseHelper;
        private SearchDoctorsService _searchDoctorsService;

        [SetUp]
        public void Setup()
        {
            _mockDatabaseHelper = new Mock<IDoctorsDatabaseHelper>();
            _searchDoctorsService = new SearchDoctorsService(_mockDatabaseHelper.Object);
        }

       
        [Test]
        public async Task LoadDoctors_WhenLoaded_ShouldIncludeDepartmentDoctors()
        {
            var departmentDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio" }
            };
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName("test"))
                .ReturnsAsync(departmentDoctors);
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByPartialDoctorName("test"))
                .ReturnsAsync(new List<DoctorModel>());

            await _searchDoctorsService.LoadDoctors("test");

            Assert.IsTrue(_searchDoctorsService.AvailableDoctors.Any(d => d.DoctorId == 1));
        }

        [Test]
        public async Task LoadDoctors_WhenLoaded_ShouldIncludeNameDoctors()
        {
            var nameDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 2, DoctorName = "Dr. B", DepartmentName = "Neuro" }
            };
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName("test"))
                .ReturnsAsync(new List<DoctorModel>());
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByPartialDoctorName("test"))
                .ReturnsAsync(nameDoctors);

            await _searchDoctorsService.LoadDoctors("test");

            Assert.IsTrue(_searchDoctorsService.AvailableDoctors.Any(d => d.DoctorId == 2));
        }

        [Test]
        public async Task LoadDoctors_WhenSourcesReturnUniqueDoctors_ShouldResultInCountOfTwo()
        {
            var doctorsFromDept = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio" }
            };
            var doctorsFromName = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 2, DoctorName = "Dr. B", DepartmentName = "Neuro" }
            };

            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName("test"))
                .ReturnsAsync(doctorsFromDept);
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByPartialDoctorName("test"))
                .ReturnsAsync(doctorsFromName);

            await _searchDoctorsService.LoadDoctors("test");

            Assert.AreEqual(2, _searchDoctorsService.AvailableDoctors.Count);
        }

        [Test]
        public async Task LoadDoctors_WhenDuplicatesExist_ShouldResultInOnlyOneDoctor()
        {
            var duplicateDoctor = new DoctorModel { DoctorId = 1, DoctorName = "Dr. A", DepartmentName = "Cardio" };
            var doctorList = new List<DoctorModel> { duplicateDoctor };

            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName("duplicate"))
                .ReturnsAsync(doctorList);
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByPartialDoctorName("duplicate"))
                .ReturnsAsync(doctorList);

            await _searchDoctorsService.LoadDoctors("duplicate");

            Assert.AreEqual(1, _searchDoctorsService.AvailableDoctors.Count);
        }

        [Test]
        public async Task LoadDoctors_WhenExceptionOccurs_ShouldNotThrow()
        {
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Simulated error"));

            Assert.DoesNotThrowAsync(async () => await _searchDoctorsService.LoadDoctors("error"));
        }

        [Test]
        public void GetSearchedDoctors_WhenOneDoctorExists_ShouldReturnCountOne()
        {
            _searchDoctorsService.AvailableDoctors.Add(new DoctorModel { DoctorId = 1 });

            var result = _searchDoctorsService.GetSearchedDoctors();

            Assert.AreEqual(1, result.Count);
        }

        [TestCase(SortCriteria.RatingHighToLow)]
        [TestCase(SortCriteria.RatingLowToHigh)]
        [TestCase(SortCriteria.NameAscending)]
        [TestCase(SortCriteria.NameDescending)]
        [TestCase(SortCriteria.DepartmentAscending)]
        [TestCase(SortCriteria.RatingThenNameThenDepartment)]
        [TestCase((SortCriteria)999)]
        public async Task GetDoctorsSortedBy_WhenSortingCalled_ShouldReturnSortedListOfThree(SortCriteria sortCriteria)
        {
            var doctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Z", Rating = 3.2, DepartmentName = "Neurology" },
                new DoctorModel { DoctorId = 2, DoctorName = "Dr. A", Rating = 4.8, DepartmentName = "Cardiology" },
                new DoctorModel { DoctorId = 3, DoctorName = "Dr. M", Rating = 4.8, DepartmentName = "Dermatology" }
            };

            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByDepartmentPartialName(It.IsAny<string>()))
                .ReturnsAsync(doctors);
            _mockDatabaseHelper.Setup(helper => helper.GetDoctorsByPartialDoctorName(It.IsAny<string>()))
                .ReturnsAsync(new List<DoctorModel>());

            await _searchDoctorsService.LoadDoctors("sort");

            var sortedDoctors = _searchDoctorsService.GetDoctorsSortedBy(sortCriteria);

            Assert.AreEqual(3, sortedDoctors.Count);
        }
    }
}
