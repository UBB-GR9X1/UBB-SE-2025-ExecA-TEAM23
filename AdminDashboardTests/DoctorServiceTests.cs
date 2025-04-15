using Hospital.Repositories;
using Hospital.Models;
using Hospital.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Repositories;

namespace AdminDashboardTests
{
    public class DoctorServiceTests
    {
        private Mock<IDoctorRepository> _mockDoctorRepository;
        private DoctorService _doctorService;
        private const int _testUserId = 123;

        [SetUp]
        public void Setup()
        {
            _mockDoctorRepository = new Mock<IDoctorRepository>();
            _doctorService = new DoctorService(_mockDoctorRepository.Object);
        }

        [Test]
        public void Constructor_WhenDatabaseHelperIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DoctorService(null));
        }

        [Test]
        public void Constructor_WhenCalled_InitializesDoctorListAsEmpty()
        {
            Assert.AreEqual(0, _doctorService.DoctorList.Count);
        }

        [Test]
        public void Constructor_WhenCalled_InitializesDoctorInformationWithDefaultValue()
        {
            Assert.AreEqual(DoctorModel.Default, _doctorService.DoctorInformation);
        }

        [Test]
        public async Task GetDoctorsByDepartment_WhenCalled_CallsDatabaseHelper()
        {
            int departmentId = 456;
            var expectedDoctors = new List<DoctorJointModel>
            {
                new DoctorJointModel(
                    doctorId: 1,
                    userId: 101,
                    doctorName: "Dr. Smith",
                    departmentId: departmentId,
                    rating: 4.5,
                    licenseNumber: "LIC123",
                    username: "drsmith",
                    password: "password123",
                    mail: "smith@hospital.com",
                    birthDate: new DateOnly(1980, 1, 15),
                    cnp: "1800115123456",
                    address: "123 Medical St",
                    phoneNumber: "1234567890",
                    registrationDate: new DateTime(2020, 3, 10)
                ),
                new DoctorJointModel(
                    doctorId: 2,
                    userId: 102,
                    doctorName: "Dr. Jones",
                    departmentId: departmentId,
                    rating: 4.8,
                    licenseNumber: "LIC456",
                    username: "drjones",
                    password: "password456",
                    mail: "jones@hospital.com",
                    birthDate: new DateOnly(1975, 5, 20),
                    cnp: "1750520123456",
                    address: "456 Health Ave",
                    phoneNumber: "0987654321",
                    registrationDate: new DateTime(2019, 7, 22)
                )
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartment(departmentId))
                .ReturnsAsync(expectedDoctors);

            var result = await _doctorService.GetDoctorsByDepartment(departmentId);

            _mockDoctorRepository.Verify(db => db.GetDoctorsByDepartment(departmentId), Times.Once);
        }

        [Test]
        public async Task GetDoctorsByDepartment_WhenCalled_ReturnsCorrectDoctors()
        {
            int departmentId = 456;
            var expectedDoctors = new List<DoctorJointModel>
            {
                new DoctorJointModel(
                    doctorId: 1,
                    userId: 101,
                    doctorName: "Dr. Smith",
                    departmentId: departmentId,
                    rating: 4.5,
                    licenseNumber: "LIC123",
                    username: "drsmith",
                    password: "password123",
                    mail: "smith@hospital.com",
                    birthDate: new DateOnly(1980, 1, 15),
                    cnp: "1800115123456",
                    address: "123 Medical St",
                    phoneNumber: "1234567890",
                    registrationDate: new DateTime(2020, 3, 10)
                ),
                new DoctorJointModel(
                    doctorId: 2,
                    userId: 102,
                    doctorName: "Dr. Jones",
                    departmentId: departmentId,
                    rating: 4.8,
                    licenseNumber: "LIC456",
                    username: "drjones",
                    password: "password456",
                    mail: "jones@hospital.com",
                    birthDate: new DateOnly(1975, 5, 20),
                    cnp: "1750520123456",
                    address: "456 Health Ave",
                    phoneNumber: "0987654321",
                    registrationDate: new DateTime(2019, 7, 22)
                )
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartment(departmentId))
                .ReturnsAsync(expectedDoctors);

            var result = await _doctorService.GetDoctorsByDepartment(departmentId);

            Assert.AreEqual(expectedDoctors, result);
        }

        [Test]
        public async Task GetAllDoctorsAsync_WhenCalled_CallsDatabaseHelper()
        {
            var expectedDoctors = new List<DoctorJointModel>
            {
                new DoctorJointModel(
                    doctorId: 1,
                    userId: 101,
                    doctorName: "Dr. Smith",
                    departmentId: 5,
                    rating: 4.5,
                    licenseNumber: "LIC123",
                    username: "drsmith",
                    password: "password123",
                    mail: "smith@hospital.com",
                    birthDate: new DateOnly(1980, 1, 15),
                    cnp: "1800115123456",
                    address: "123 Medical St",
                    phoneNumber: "1234567890",
                    registrationDate: new DateTime(2020, 3, 10)
                ),
                new DoctorJointModel(
                    doctorId: 2,
                    userId: 102,
                    doctorName: "Dr. Jones",
                    departmentId: 5,
                    rating: 4.8,
                    licenseNumber: "LIC456",
                    username: "drjones",
                    password: "password456",
                    mail: "jones@hospital.com",
                    birthDate: new DateOnly(1975, 5, 20),
                    cnp: "1750520123456",
                    address: "456 Health Ave",
                    phoneNumber: "0987654321",
                    registrationDate: new DateTime(2019, 7, 22)
                )
            };

            _mockDoctorRepository.Setup(db => db.GetAllDoctors())
                .ReturnsAsync(expectedDoctors);

            await _doctorService.GetAllDoctorsAsync();

            _mockDoctorRepository.Verify(db => db.GetAllDoctors(), Times.Once);
        }

        [Test]
        public async Task GetAllDoctorsAsync_WhenCalled_ReturnsCorrectDoctors()
        {
            var expectedDoctors = new List<DoctorJointModel>
            {
                new DoctorJointModel(
                    doctorId: 1,
                    userId: 101,
                    doctorName: "Dr. Smith",
                    departmentId: 5,
                    rating: 4.5,
                    licenseNumber: "LIC123",
                    username: "drsmith",
                    password: "password123",
                    mail: "smith@hospital.com",
                    birthDate: new DateOnly(1980, 1, 15),
                    cnp: "1800115123456",
                    address: "123 Medical St",
                    phoneNumber: "1234567890",
                    registrationDate: new DateTime(2020, 3, 10)
                ),
                new DoctorJointModel(
                    doctorId: 2,
                    userId: 102,
                    doctorName: "Dr. Jones",
                    departmentId: 5,
                    rating: 4.8,
                    licenseNumber: "LIC456",
                    username: "drjones",
                    password: "password456",
                    mail: "jones@hospital.com",
                    birthDate: new DateOnly(1975, 5, 20),
                    cnp: "1750520123456",
                    address: "456 Health Ave",
                    phoneNumber: "0987654321",
                    registrationDate: new DateTime(2019, 7, 22)
                )
            };

            _mockDoctorRepository.Setup(db => db.GetAllDoctors())
                .ReturnsAsync(expectedDoctors);

            var result = await _doctorService.GetAllDoctorsAsync();

            Assert.AreEqual(expectedDoctors, result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_ReturnsTrue()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = _testUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(expectedDoctor);

            bool result = await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_UpdatesDoctorInformation()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = _testUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(expectedDoctor);

            await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            Assert.AreEqual(expectedDoctor, _doctorService.DoctorInformation);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_CallsDatabaseHelper()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = _testUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(expectedDoctor);

            await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            _mockDoctorRepository.Verify(db => db.GetDoctorById(_testUserId), Times.Once);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_ReturnsFalse()
        {
            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(DoctorModel.Default);

            bool result = await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_SetsDoctorInformationToDefault()
        {
            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(DoctorModel.Default);

            await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            Assert.AreEqual(DoctorModel.Default, _doctorService.DoctorInformation);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_CallsDatabaseHelper()
        {
            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ReturnsAsync(DoctorModel.Default);

            await _doctorService.LoadDoctorInformationByUserId(_testUserId);

            _mockDoctorRepository.Verify(db => db.GetDoctorById(_testUserId), Times.Once);
        }

        [Test]
        public void LoadDoctorInformationByUserId_WhenDatabaseThrowsException_RethrowsWithAppropriateMessage()
        {
            var expectedException = new Exception("Database error");
            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _doctorService.LoadDoctorInformationByUserId(_testUserId));

            Assert.IsTrue(exception.Message.Contains("Error loading doctor info"));
        }

        [Test]
        public void LoadDoctorInformationByUserId_WhenDatabaseThrowsException_PreservesOriginalExceptionAsInnerException()
        {
            var expectedException = new Exception("Database error");
            _mockDoctorRepository.Setup(db => db.GetDoctorById(_testUserId))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _doctorService.LoadDoctorInformationByUserId(_testUserId));

            Assert.AreEqual(expectedException, exception.InnerException);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenDoctorsFound_ReturnsTrue()
        {
            string departmentName = "Card";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith", DepartmentName = "Cardiology" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            bool result = await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenDoctorsFound_UpdatesDoctorList()
        {
            string departmentName = "Card";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith", DepartmentName = "Cardiology" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.AreEqual(expectedDoctors, _doctorService.DoctorList);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenDoctorsFound_CallsDatabaseHelper()
        {
            string departmentName = "Card";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith", DepartmentName = "Cardiology" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            _mockDoctorRepository.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenNoDoctorsFound_ReturnsFalse()
        {
            string departmentName = "Nonexistent";

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenNoDoctorsFound_CallsDatabaseHelper()
        {
            string departmentName = "Nonexistent";

            _mockDoctorRepository.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync((List<DoctorModel>)null);

            await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            _mockDoctorRepository.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenDoctorsFound_ReturnsTrue()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            bool result = await _doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenDoctorsFound_UpdatesDoctorList()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            await _doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.AreEqual(expectedDoctors, _doctorService.DoctorList);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenDoctorsFound_CallsDatabaseHelper()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            _mockDoctorRepository.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            await _doctorService.SearchDoctorsByNameAsync(doctorName);

            _mockDoctorRepository.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenNoDoctorsFound_ReturnsFalse()
        {
            string doctorName = "Nonexistent";

            _mockDoctorRepository.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await _doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenNoDoctorsFound_CallsDatabaseHelper()
        {
            string doctorName = "Nonexistent";

            _mockDoctorRepository.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync((List<DoctorModel>)null);

            await _doctorService.SearchDoctorsByNameAsync(doctorName);

            _mockDoctorRepository.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task UpdateDoctorName_WhenNameIsValid_ReturnsTrue()
        {
            string validName = "Dr. John Doe";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorName(_testUserId, validName))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateDoctorName(_testUserId, validName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDoctorName_WhenNameIsValid_CallsDatabaseHelper()
        {
            string validName = "Dr. John Doe";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorName(_testUserId, validName))
                .ReturnsAsync(true);

            await _doctorService.UpdateDoctorName(_testUserId, validName);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorName(_testUserId, validName), Times.Once);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsEmpty_ThrowsArgumentException()
        {
            string emptyName = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, emptyName));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsEmpty_IncludesCorrectParameterName()
        {
            string emptyName = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, emptyName));

            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsWhitespace_ThrowsArgumentException()
        {
            string whitespace = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, whitespace));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsMissingLastName_ThrowsArgumentException()
        {
            string onlyFirstName = "John";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, onlyFirstName));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameHasNoPropperSpacing_ThrowsArgumentException()
        {
            string noSpacing = "Dr.John";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, noSpacing));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidName = "";

            try
            {
                _doctorService.UpdateDoctorName(_testUserId, invalidName).Wait();
            }
            catch { /* Expected exception */ }

            _mockDoctorRepository.Verify(db => db.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsTooLong_ThrowsArgumentException()
        {
            string tooLongName = new string('A', 50) + " " + new string('B', 51); // 102 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, tooLongName));

            Assert.That(exception.Message, Does.StartWith("Doctor name is too long."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongName = new string('A', 50) + " " + new string('B', 51); // 102 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(_testUserId, tooLongName));

            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public async Task UpdateDepartment_WhenCalled_ReturnsTrue()
        {
            int departmentId = 456;

            _mockDoctorRepository.Setup(db => db.UpdateDoctorDepartment(_testUserId, departmentId))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateDepartment(_testUserId, departmentId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDepartment_WhenCalled_CallsDatabaseHelper()
        {
            int departmentId = 456;

            _mockDoctorRepository.Setup(db => db.UpdateDoctorDepartment(_testUserId, departmentId))
                .ReturnsAsync(true);

            await _doctorService.UpdateDepartment(_testUserId, departmentId);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorDepartment(_testUserId, departmentId), Times.Once);
        }

        [Test]
        public async Task UpdateRatingAsync_WhenRatingIsValid_ReturnsTrue()
        {
            double validRating = 4.5;

            _mockDoctorRepository.Setup(db => db.UpdateDoctorRating(_testUserId, validRating))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateRatingAsync(_testUserId, validRating);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateRatingAsync_WhenRatingIsValid_CallsDatabaseHelper()
        {
            double validRating = 4.5;

            _mockDoctorRepository.Setup(db => db.UpdateDoctorRating(_testUserId, validRating))
                .ReturnsAsync(true);

            await _doctorService.UpdateRatingAsync(_testUserId, validRating);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorRating(_testUserId, validRating), Times.Once);
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsNegative_ThrowsArgumentOutOfRangeException()
        {
            double negativeRating = -0.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _doctorService.UpdateRatingAsync(_testUserId, negativeRating));

            Assert.That(exception.Message, Does.Contain("Rating must be between 0 and 5."));
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsNegative_IncludesCorrectParameterName()
        {
            double negativeRating = -0.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _doctorService.UpdateRatingAsync(_testUserId, negativeRating));

            Assert.AreEqual("rating", exception.ParamName);
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsAboveMaximum_ThrowsArgumentOutOfRangeException()
        {
            double tooHighRating = 5.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _doctorService.UpdateRatingAsync(_testUserId, tooHighRating));

            Assert.That(exception.Message, Does.Contain("Rating must be between 0 and 5."));
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsInvalid_DoesNotCallDatabaseHelper()
        {
            double invalidRating = -0.1;

            try
            {
                _doctorService.UpdateRatingAsync(_testUserId, invalidRating).Wait();
            }
            catch { /* Expected exception */ }

            _mockDoctorRepository.Verify(db => db.UpdateDoctorRating(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCalled_ReturnsTrue()
        {
            string careerInfo = "Experienced cardiologist with 10+ years of practice";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorCareerInfo(_testUserId, careerInfo))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateCareerInfo(_testUserId, careerInfo);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCalled_CallsDatabaseHelper()
        {
            string careerInfo = "Experienced cardiologist with 10+ years of practice";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorCareerInfo(_testUserId, careerInfo))
                .ReturnsAsync(true);

            await _doctorService.UpdateCareerInfo(_testUserId, careerInfo);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorCareerInfo(_testUserId, careerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCareerInfoIsNull_PassesEmptyStringToDatabaseHelper()
        {
            _mockDoctorRepository.Setup(db => db.UpdateDoctorCareerInfo(_testUserId, string.Empty))
                .ReturnsAsync(true);

            await _doctorService.UpdateCareerInfo(_testUserId, null);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorCareerInfo(_testUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCareerInfoIsNull_ReturnsTrue()
        {
            _mockDoctorRepository.Setup(db => db.UpdateDoctorCareerInfo(_testUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateCareerInfo(_testUserId, null);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsValid_ReturnsTrue()
        {
            string validUrl = "https://example.com/avatar.jpg";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorAvatarUrl(_testUserId, validUrl))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateAvatarUrl(_testUserId, validUrl);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsValid_CallsDatabaseHelper()
        {
            string validUrl = "https://example.com/avatar.jpg";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorAvatarUrl(_testUserId, validUrl))
                .ReturnsAsync(true);

            await _doctorService.UpdateAvatarUrl(_testUserId, validUrl);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorAvatarUrl(_testUserId, validUrl), Times.Once);
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_ThrowsArgumentException()
        {
            string tooLongUrl = new string('a', 256);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateAvatarUrl(_testUserId, tooLongUrl));

            Assert.That(exception.Message, Does.StartWith("Avatar URL is too long."));
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongUrl = new string('a', 256);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateAvatarUrl(_testUserId, tooLongUrl));

            Assert.AreEqual("avatarUrl", exception.ParamName);
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_DoesNotCallDatabaseHelper()
        {
            string tooLongUrl = new string('a', 256);

            try
            {
                _doctorService.UpdateAvatarUrl(_testUserId, tooLongUrl).Wait();
            }
            catch { /* Expected exception */ }

            _mockDoctorRepository.Verify(db => db.UpdateDoctorAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsNull_PassesEmptyStringToDatabaseHelper()
        {
            _mockDoctorRepository.Setup(db => db.UpdateDoctorAvatarUrl(_testUserId, string.Empty))
                .ReturnsAsync(true);

            await _doctorService.UpdateAvatarUrl(_testUserId, null);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorAvatarUrl(_testUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsNull_ReturnsTrue()
        {
            _mockDoctorRepository.Setup(db => db.UpdateDoctorAvatarUrl(_testUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateAvatarUrl(_testUserId, null);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumber_WhenPhoneNumberIsValid_ReturnsTrue()
        {
            string validPhoneNumber = "1234567890";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorPhoneNumber(_testUserId, validPhoneNumber))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdatePhoneNumber(_testUserId, validPhoneNumber);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumber_WhenPhoneNumberIsValid_CallsDatabaseHelper()
        {
            string validPhoneNumber = "1234567890";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorPhoneNumber(_testUserId, validPhoneNumber))
                .ReturnsAsync(true);

            await _doctorService.UpdatePhoneNumber(_testUserId, validPhoneNumber);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorPhoneNumber(_testUserId, validPhoneNumber), Times.Once);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsEmpty_ThrowsArgumentException()
        {
            string emptyPhoneNumber = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, emptyPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsEmpty_IncludesCorrectParameterName()
        {
            string emptyPhoneNumber = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, emptyPhoneNumber));

            Assert.AreEqual("phoneNumber", exception.ParamName);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsWhitespace_ThrowsArgumentException()
        {
            string whitespacePhoneNumber = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, whitespacePhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsTooShort_ThrowsArgumentException()
        {
            string tooShortPhoneNumber = "123456789";  // 9 digits

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, tooShortPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsTooLong_ThrowsArgumentException()
        {
            string tooLongPhoneNumber = "12345678901"; // 11 digits

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, tooLongPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsNonDigits_ThrowsArgumentException()
        {
            string phoneNumberWithNonDigits = "123-456-789";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, phoneNumberWithNonDigits));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsLetters_ThrowsArgumentException()
        {
            string phoneNumberWithLetters = "123456789a";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, phoneNumberWithLetters));

            Assert.That(exception.Message, Does.StartWith("Phone number must contain only digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsLetters_IncludesCorrectParameterName()
        {
            string phoneNumberWithLetters = "123456789a";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(_testUserId, phoneNumberWithLetters));

            Assert.AreEqual("phoneNumber", exception.ParamName);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidPhoneNumber = "123456789";

            try
            {
                _doctorService.UpdatePhoneNumber(_testUserId, invalidPhoneNumber).Wait();
            }
            catch { /* Expected exception */ }

            _mockDoctorRepository.Verify(db => db.UpdateDoctorPhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateEmail_WhenEmailIsValid_ReturnsTrue()
        {
            string validEmail = "doctor@hospital.com";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorEmail(_testUserId, validEmail))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateEmail(_testUserId, validEmail);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateEmail_WhenEmailIsValid_CallsDatabaseHelper()
        {
            string validEmail = "doctor@hospital.com";

            _mockDoctorRepository.Setup(db => db.UpdateDoctorEmail(_testUserId, validEmail))
                .ReturnsAsync(true);

            await _doctorService.UpdateEmail(_testUserId, validEmail);

            _mockDoctorRepository.Verify(db => db.UpdateDoctorEmail(_testUserId, validEmail), Times.Once);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsEmpty_ThrowsArgumentException()
        {
            string emptyEmail = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, emptyEmail));

            Assert.That(exception.Message, Does.StartWith("Mail cannot be empty."));
        }

        [Test]
        public void UpdateEmail_WhenEmailIsEmpty_IncludesCorrectParameterName()
        {
            string emptyEmail = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, emptyEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsWhitespace_ThrowsArgumentException()
        {
            string whitespaceEmail = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, whitespaceEmail));

            Assert.That(exception.Message, Does.StartWith("Mail cannot be empty."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingAtSymbol_ThrowsArgumentException()
        {
            string invalidEmail = "doctor";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingDomain_ThrowsArgumentException()
        {
            string invalidEmail = "doctor@hospital";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingDot_ThrowsArgumentException()
        {
            string invalidEmail = "doctor.hospital";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingAtSymbol_IncludesCorrectParameterName()
        {
            string invalidEmail = "doctor";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, invalidEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsTooLong_ThrowsArgumentException()
        {
            string tooLongEmail = new string('a', 90) + "@hospital.com"; // More than 100 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, tooLongEmail));

            Assert.That(exception.Message, Does.StartWith("Mail is too long."));
        }

        [Test]
        public void UpdateEmail_WhenEmailIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongEmail = new string('a', 90) + "@hospital.com"; // More than 100 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(_testUserId, tooLongEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidEmail = "doctor";

            try
            {
                _doctorService.UpdateEmail(_testUserId, invalidEmail).Wait();
            }
            catch { /* Expected exception */ }

            _mockDoctorRepository.Verify(db => db.UpdateDoctorEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task LogUpdate_WhenCalled_ReturnsTrue()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            _mockDoctorRepository.Setup(db => db.UpdateLogService(_testUserId, actionType))
                .ReturnsAsync(true);

            bool result = await _doctorService.LogUpdate(_testUserId, actionType);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LogUpdate_WhenCalled_CallsDatabaseHelper()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            _mockDoctorRepository.Setup(db => db.UpdateLogService(_testUserId, actionType))
                .ReturnsAsync(true);

            await _doctorService.LogUpdate(_testUserId, actionType);

            _mockDoctorRepository.Verify(db => db.UpdateLogService(_testUserId, actionType), Times.Once);
        }
    }
}