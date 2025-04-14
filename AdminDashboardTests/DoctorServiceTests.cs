using Hospital.DatabaseServices;
using Hospital.Doctor_Dashboard;
using Hospital.Managers;
using Hospital.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDashboardTests
{
    public class DoctorServiceTests
    {
        private Mock<IDoctorsDatabaseHelper> _mockDbHelper;
        private DoctorService _doctorService;
        private const int TestUserId = 123;

        [SetUp]
        public void Setup()
        {
            _mockDbHelper = new Mock<IDoctorsDatabaseHelper>();
            _doctorService = new DoctorService(_mockDbHelper.Object);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenDbHelperIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DoctorService(null));
        }

        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            Assert.IsNotNull(_doctorService.DoctorList);
            Assert.AreEqual(0, _doctorService.DoctorList.Count);
            Assert.AreEqual(DoctorModel.Default, _doctorService.DoctorInformation);
        }
        
        [Test]
        public async Task GetDoctorsByDepartment_ShouldCallDatabaseHelper()
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

            _mockDbHelper.Setup(db => db.GetDoctorsByDepartment(departmentId))
                .ReturnsAsync(expectedDoctors);

            var result = await _doctorService.GetDoctorsByDepartment(departmentId);

            Assert.AreEqual(expectedDoctors, result);
            _mockDbHelper.Verify(db => db.GetDoctorsByDepartment(departmentId), Times.Once);
        }

        [Test]
        public async Task GetAllDoctorsAsync_ShouldCallDatabaseHelper()
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

            _mockDbHelper.Setup(db => db.GetAllDoctors())
                .ReturnsAsync(expectedDoctors);

            var result = await _doctorService.GetAllDoctorsAsync();

            Assert.AreEqual(expectedDoctors, result);
            _mockDbHelper.Verify(db => db.GetAllDoctors(), Times.Once);
        }
        
        [Test]
        public async Task LoadDoctorInformationByUserId_ShouldReturnTrue_WhenDoctorExists()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = TestUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            _mockDbHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(expectedDoctor);

            bool result = await _doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedDoctor, _doctorService.DoctorInformation);
            _mockDbHelper.Verify(db => db.GetDoctorById(TestUserId), Times.Once);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_ShouldReturnFalse_WhenDoctorNotFound()
        {
            _mockDbHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(DoctorModel.Default);

            bool result = await _doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.IsFalse(result);
            Assert.AreEqual(DoctorModel.Default, _doctorService.DoctorInformation);
            _mockDbHelper.Verify(db => db.GetDoctorById(TestUserId), Times.Once);
        }

        [Test]
        public void LoadDoctorInformationByUserId_ShouldRethrowException_WhenDbHelperThrows()
        {
            var expectedException = new Exception("Database error");
            _mockDbHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _doctorService.LoadDoctorInformationByUserId(TestUserId));

            Assert.IsTrue(exception.Message.Contains("Error loading doctor info"));
            Assert.AreEqual(expectedException, exception.InnerException);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_ShouldReturnTrue_WhenDoctorsFound()
        {
            string departmentName = "Card";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith", DepartmentName = "Cardiology" }
            };

            _mockDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            bool result = await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedDoctors, _doctorService.DoctorList);
            _mockDbHelper.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_ShouldReturnFalse_WhenNoDoctorsFound()
        {
            string departmentName = "Nonexistent";

            _mockDbHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await _doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.IsFalse(result);
            _mockDbHelper.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_ShouldReturnTrue_WhenDoctorsFound()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            _mockDbHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            bool result = await _doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedDoctors, _doctorService.DoctorList);
            _mockDbHelper.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_ShouldReturnFalse_WhenNoDoctorsFound()
        {
            string doctorName = "Nonexistent";

            _mockDbHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await _doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.IsFalse(result);
            _mockDbHelper.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task UpdateDoctorName_ShouldCallDatabaseHelper_WhenNameIsValid()
        {
            string validName = "Dr. John Doe";

            _mockDbHelper.Setup(db => db.UpdateDoctorName(TestUserId, validName))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateDoctorName(TestUserId, validName);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorName(TestUserId, validName), Times.Once);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("John")]
        [TestCase("Dr.John")]
        public void UpdateDoctorName_ShouldThrowArgumentException_WhenNameIsInvalid(string invalidName)
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(TestUserId, invalidName));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
            Assert.AreEqual("name", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UpdateDoctorName_ShouldThrowArgumentException_WhenNameIsTooLong()
        {
            string tooLongName = new string('A', 50) + " " + new string('B', 51); // 102 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateDoctorName(TestUserId, tooLongName));

            Assert.That(exception.Message, Does.StartWith("Doctor name is too long."));
            Assert.AreEqual("name", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateDepartment_ShouldCallDatabaseHelper()
        {
            int departmentId = 456;

            _mockDbHelper.Setup(db => db.UpdateDoctorDepartment(TestUserId, departmentId))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateDepartment(TestUserId, departmentId);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorDepartment(TestUserId, departmentId), Times.Once);
        }

        [Test]
        public async Task UpdateRatingAsync_ShouldCallDatabaseHelper_WhenRatingIsValid()
        {
            double validRating = 4.5;

            _mockDbHelper.Setup(db => db.UpdateDoctorRating(TestUserId, validRating))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateRatingAsync(TestUserId, validRating);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorRating(TestUserId, validRating), Times.Once);
        }

        [TestCase(-0.1)]
        [TestCase(5.1)]
        public void UpdateRatingAsync_ShouldThrowArgumentOutOfRangeException_WhenRatingIsInvalid(double invalidRating)
        {
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _doctorService.UpdateRatingAsync(TestUserId, invalidRating));

            Assert.That(exception.Message, Does.Contain("Rating must be between 0 and 5."));
            Assert.AreEqual("rating", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorRating(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        }

        [Test]
        public async Task UpdateCareerInfo_ShouldCallDatabaseHelper()
        {
            string careerInfo = "Experienced cardiologist with 10+ years of practice";

            _mockDbHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, careerInfo))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateCareerInfo(TestUserId, careerInfo);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorCareerInfo(TestUserId, careerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_ShouldHandleNullCareerInfo()
        {
            _mockDbHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateCareerInfo(TestUserId, null);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorCareerInfo(TestUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrl_ShouldCallDatabaseHelper_WhenUrlIsValid()
        {
            string validUrl = "https://example.com/avatar.jpg";

            _mockDbHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, validUrl))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateAvatarUrl(TestUserId, validUrl);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorAvatarUrl(TestUserId, validUrl), Times.Once);
        }

        [Test]
        public void UpdateAvatarUrl_ShouldThrowArgumentException_WhenUrlIsTooLong()
        {
            string tooLongUrl = new string('a', 256); 

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateAvatarUrl(TestUserId, tooLongUrl));

            Assert.That(exception.Message, Does.StartWith("Avatar URL is too long."));
            Assert.AreEqual("avatarUrl", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateAvatarUrl_ShouldHandleNullUrl()
        {
            _mockDbHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateAvatarUrl(TestUserId, null);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorAvatarUrl(TestUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdatePhoneNumber_ShouldCallDatabaseHelper_WhenPhoneNumberIsValid()
        {
            string validPhoneNumber = "1234567890";

            _mockDbHelper.Setup(db => db.UpdateDoctorPhoneNumber(TestUserId, validPhoneNumber))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdatePhoneNumber(TestUserId, validPhoneNumber);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorPhoneNumber(TestUserId, validPhoneNumber), Times.Once);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("123456789")]  
        [TestCase("12345678901")] 
        [TestCase("123-456-789")] 
        public void UpdatePhoneNumber_ShouldThrowArgumentException_WhenPhoneNumberHasIncorrectLength(string invalidPhoneNumber)
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(TestUserId, invalidPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
            Assert.AreEqual("phoneNumber", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorPhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UpdatePhoneNumber_ShouldThrowArgumentException_WhenPhoneNumberContainsNonDigits()
        {
            string invalidPhoneNumber = "123456789a"; 

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdatePhoneNumber(TestUserId, invalidPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must contain only digits."));
            Assert.AreEqual("phoneNumber", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorPhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateEmail_ShouldCallDatabaseHelper_WhenEmailIsValid()
        {
            string validEmail = "doctor@hospital.com";

            _mockDbHelper.Setup(db => db.UpdateDoctorEmail(TestUserId, validEmail))
                .ReturnsAsync(true);

            bool result = await _doctorService.UpdateEmail(TestUserId, validEmail);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateDoctorEmail(TestUserId, validEmail), Times.Once);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void UpdateEmail_ShouldThrowArgumentException_WhenEmailIsEmpty(string invalidEmail)
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail cannot be empty."));
            Assert.AreEqual("email", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [TestCase("doctor")]
        [TestCase("doctor@hospital")]
        [TestCase("doctor.hospital")]
        public void UpdateEmail_ShouldThrowArgumentException_WhenEmailIsInvalidFormat(string invalidEmail)
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
            Assert.AreEqual("email", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UpdateEmail_ShouldThrowArgumentException_WhenEmailIsTooLong()
        {
            string tooLongEmail = new string('a', 90) + "@hospital.com"; // More than 100 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _doctorService.UpdateEmail(TestUserId, tooLongEmail));

            Assert.That(exception.Message, Does.StartWith("Mail is too long."));
            Assert.AreEqual("email", exception.ParamName);
            _mockDbHelper.Verify(db => db.UpdateDoctorEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task LogUpdate_ShouldCallDatabaseHelper()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            _mockDbHelper.Setup(db => db.UpdateLogService(TestUserId, actionType))
                .ReturnsAsync(true);

            bool result = await _doctorService.LogUpdate(TestUserId, actionType);

            Assert.IsTrue(result);
            _mockDbHelper.Verify(db => db.UpdateLogService(TestUserId, actionType), Times.Once);
        }
    }
}