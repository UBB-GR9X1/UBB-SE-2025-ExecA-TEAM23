using Hospital.Doctor_Dashboard;
using Hospital.Managers;
using Hospital.Models;
using Hospital.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDashboardTests
{
    public class DoctorServiceTests
    {
        private Mock<IDoctorsDatabaseHelper> mockDatabaseHelper;
        private DoctorService doctorService;
        private const int TestUserId = 123;

        [SetUp]
        public void Setup()
        {
            mockDatabaseHelper = new Mock<IDoctorsDatabaseHelper>();
            doctorService = new DoctorService(mockDatabaseHelper.Object);
        }

        [Test]
        public void Constructor_WhenDatabaseHelperIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DoctorService(null));
        }

        [Test]
        public void Constructor_WhenCalled_InitializesDoctorListAsEmpty()
        {
            Assert.AreEqual(0, doctorService.DoctorList.Count);
        }

        [Test]
        public void Constructor_WhenCalled_InitializesDoctorInformationWithDefaultValue()
        {
            Assert.AreEqual(DoctorModel.Default, doctorService.DoctorInformation);
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

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartment(departmentId))
                .ReturnsAsync(expectedDoctors);

            var result = await doctorService.GetDoctorsByDepartment(departmentId);

            mockDatabaseHelper.Verify(db => db.GetDoctorsByDepartment(departmentId), Times.Once);
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

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartment(departmentId))
                .ReturnsAsync(expectedDoctors);

            var result = await doctorService.GetDoctorsByDepartment(departmentId);

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

            mockDatabaseHelper.Setup(db => db.GetAllDoctors())
                .ReturnsAsync(expectedDoctors);

            await doctorService.GetAllDoctorsAsync();

            mockDatabaseHelper.Verify(db => db.GetAllDoctors(), Times.Once);
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

            mockDatabaseHelper.Setup(db => db.GetAllDoctors())
                .ReturnsAsync(expectedDoctors);

            var result = await doctorService.GetAllDoctorsAsync();

            Assert.AreEqual(expectedDoctors, result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_ReturnsTrue()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = TestUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(expectedDoctor);

            bool result = await doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_UpdatesDoctorInformation()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = TestUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(expectedDoctor);

            await doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.AreEqual(expectedDoctor, doctorService.DoctorInformation);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorExists_CallsDatabaseHelper()
        {
            var expectedDoctor = new DoctorModel
            {
                DoctorId = TestUserId,
                DoctorName = "Dr. John Doe",
                DepartmentId = 456,
                DepartmentName = "Cardiology"
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(expectedDoctor);

            await doctorService.LoadDoctorInformationByUserId(TestUserId);

            mockDatabaseHelper.Verify(db => db.GetDoctorById(TestUserId), Times.Once);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_ReturnsFalse()
        {
            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(DoctorModel.Default);

            bool result = await doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_SetsDoctorInformationToDefault()
        {
            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(DoctorModel.Default);

            await doctorService.LoadDoctorInformationByUserId(TestUserId);

            Assert.AreEqual(DoctorModel.Default, doctorService.DoctorInformation);
        }

        [Test]
        public async Task LoadDoctorInformationByUserId_WhenDoctorNotFound_CallsDatabaseHelper()
        {
            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ReturnsAsync(DoctorModel.Default);

            await doctorService.LoadDoctorInformationByUserId(TestUserId);

            mockDatabaseHelper.Verify(db => db.GetDoctorById(TestUserId), Times.Once);
        }

        [Test]
        public void LoadDoctorInformationByUserId_WhenDatabaseThrowsException_RethrowsWithAppropriateMessage()
        {
            var expectedException = new Exception("Database error");
            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await doctorService.LoadDoctorInformationByUserId(TestUserId));

            Assert.IsTrue(exception.Message.Contains("Error loading doctor info"));
        }

        [Test]
        public void LoadDoctorInformationByUserId_WhenDatabaseThrowsException_PreservesOriginalExceptionAsInnerException()
        {
            var expectedException = new Exception("Database error");
            mockDatabaseHelper.Setup(db => db.GetDoctorById(TestUserId))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await doctorService.LoadDoctorInformationByUserId(TestUserId));

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

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            bool result = await doctorService.SearchDoctorsByDepartmentAsync(departmentName);

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

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            await doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.AreEqual(expectedDoctors, doctorService.DoctorList);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenDoctorsFound_CallsDatabaseHelper()
        {
            string departmentName = "Card";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith", DepartmentName = "Cardiology" }
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync(expectedDoctors);

            await doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            mockDatabaseHelper.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenNoDoctorsFound_ReturnsFalse()
        {
            string departmentName = "Nonexistent";

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SearchDoctorsByDepartmentAsync_WhenNoDoctorsFound_CallsDatabaseHelper()
        {
            string departmentName = "Nonexistent";

            mockDatabaseHelper.Setup(db => db.GetDoctorsByDepartmentPartialName(departmentName))
                .ReturnsAsync((List<DoctorModel>)null);

            await doctorService.SearchDoctorsByDepartmentAsync(departmentName);

            mockDatabaseHelper.Verify(db => db.GetDoctorsByDepartmentPartialName(departmentName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenDoctorsFound_ReturnsTrue()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            bool result = await doctorService.SearchDoctorsByNameAsync(doctorName);

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

            mockDatabaseHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            await doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.AreEqual(expectedDoctors, doctorService.DoctorList);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenDoctorsFound_CallsDatabaseHelper()
        {
            string doctorName = "Smith";
            var expectedDoctors = new List<DoctorModel>
            {
                new DoctorModel { DoctorId = 1, DoctorName = "Dr. Smith" }
            };

            mockDatabaseHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync(expectedDoctors);

            await doctorService.SearchDoctorsByNameAsync(doctorName);

            mockDatabaseHelper.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenNoDoctorsFound_ReturnsFalse()
        {
            string doctorName = "Nonexistent";

            mockDatabaseHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync((List<DoctorModel>)null);

            bool result = await doctorService.SearchDoctorsByNameAsync(doctorName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SearchDoctorsByNameAsync_WhenNoDoctorsFound_CallsDatabaseHelper()
        {
            string doctorName = "Nonexistent";

            mockDatabaseHelper.Setup(db => db.GetDoctorsByPartialDoctorName(doctorName))
                .ReturnsAsync((List<DoctorModel>)null);

            await doctorService.SearchDoctorsByNameAsync(doctorName);

            mockDatabaseHelper.Verify(db => db.GetDoctorsByPartialDoctorName(doctorName), Times.Once);
        }

        [Test]
        public async Task UpdateDoctorName_WhenNameIsValid_ReturnsTrue()
        {
            string validName = "Dr. John Doe";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorName(TestUserId, validName))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateDoctorName(TestUserId, validName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDoctorName_WhenNameIsValid_CallsDatabaseHelper()
        {
            string validName = "Dr. John Doe";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorName(TestUserId, validName))
                .ReturnsAsync(true);

            await doctorService.UpdateDoctorName(TestUserId, validName);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorName(TestUserId, validName), Times.Once);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsEmpty_ThrowsArgumentException()
        {
            string emptyName = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, emptyName));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsEmpty_IncludesCorrectParameterName()
        {
            string emptyName = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, emptyName));

            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsWhitespace_ThrowsArgumentException()
        {
            string whitespace = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, whitespace));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsMissingLastName_ThrowsArgumentException()
        {
            string onlyFirstName = "John";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, onlyFirstName));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameHasNoPropperSpacing_ThrowsArgumentException()
        {
            string noSpacing = "Dr.John";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, noSpacing));

            Assert.That(exception.Message, Does.StartWith("Doctor name must include at least a first and last name."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidName = "";

            try
            {
                doctorService.UpdateDoctorName(TestUserId, invalidName).Wait();
            }
            catch { /* Expected exception */ }

            mockDatabaseHelper.Verify(db => db.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsTooLong_ThrowsArgumentException()
        {
            string tooLongName = new string('A', 50) + " " + new string('B', 51); // 102 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, tooLongName));

            Assert.That(exception.Message, Does.StartWith("Doctor name is too long."));
        }

        [Test]
        public void UpdateDoctorName_WhenNameIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongName = new string('A', 50) + " " + new string('B', 51); // 102 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateDoctorName(TestUserId, tooLongName));

            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public async Task UpdateDepartment_WhenCalled_ReturnsTrue()
        {
            int departmentId = 456;

            mockDatabaseHelper.Setup(db => db.UpdateDoctorDepartment(TestUserId, departmentId))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateDepartment(TestUserId, departmentId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDepartment_WhenCalled_CallsDatabaseHelper()
        {
            int departmentId = 456;

            mockDatabaseHelper.Setup(db => db.UpdateDoctorDepartment(TestUserId, departmentId))
                .ReturnsAsync(true);

            await doctorService.UpdateDepartment(TestUserId, departmentId);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorDepartment(TestUserId, departmentId), Times.Once);
        }

        [Test]
        public async Task UpdateRatingAsync_WhenRatingIsValid_ReturnsTrue()
        {
            double validRating = 4.5;

            mockDatabaseHelper.Setup(db => db.UpdateDoctorRating(TestUserId, validRating))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateRatingAsync(TestUserId, validRating);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateRatingAsync_WhenRatingIsValid_CallsDatabaseHelper()
        {
            double validRating = 4.5;

            mockDatabaseHelper.Setup(db => db.UpdateDoctorRating(TestUserId, validRating))
                .ReturnsAsync(true);

            await doctorService.UpdateRatingAsync(TestUserId, validRating);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorRating(TestUserId, validRating), Times.Once);
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsNegative_ThrowsArgumentOutOfRangeException()
        {
            double negativeRating = -0.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await doctorService.UpdateRatingAsync(TestUserId, negativeRating));

            Assert.That(exception.Message, Does.Contain("Rating must be between 0 and 5."));
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsNegative_IncludesCorrectParameterName()
        {
            double negativeRating = -0.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await doctorService.UpdateRatingAsync(TestUserId, negativeRating));

            Assert.AreEqual("rating", exception.ParamName);
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsAboveMaximum_ThrowsArgumentOutOfRangeException()
        {
            double tooHighRating = 5.1;

            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await doctorService.UpdateRatingAsync(TestUserId, tooHighRating));

            Assert.That(exception.Message, Does.Contain("Rating must be between 0 and 5."));
        }

        [Test]
        public void UpdateRatingAsync_WhenRatingIsInvalid_DoesNotCallDatabaseHelper()
        {
            double invalidRating = -0.1;

            try
            {
                doctorService.UpdateRatingAsync(TestUserId, invalidRating).Wait();
            }
            catch { /* Expected exception */ }

            mockDatabaseHelper.Verify(db => db.UpdateDoctorRating(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCalled_ReturnsTrue()
        {
            string careerInfo = "Experienced cardiologist with 10+ years of practice";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, careerInfo))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateCareerInfo(TestUserId, careerInfo);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCalled_CallsDatabaseHelper()
        {
            string careerInfo = "Experienced cardiologist with 10+ years of practice";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, careerInfo))
                .ReturnsAsync(true);

            await doctorService.UpdateCareerInfo(TestUserId, careerInfo);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorCareerInfo(TestUserId, careerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCareerInfoIsNull_PassesEmptyStringToDatabaseHelper()
        {
            mockDatabaseHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, string.Empty))
                .ReturnsAsync(true);

            await doctorService.UpdateCareerInfo(TestUserId, null);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorCareerInfo(TestUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_WhenCareerInfoIsNull_ReturnsTrue()
        {
            mockDatabaseHelper.Setup(db => db.UpdateDoctorCareerInfo(TestUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateCareerInfo(TestUserId, null);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsValid_ReturnsTrue()
        {
            string validUrl = "https://example.com/avatar.jpg";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, validUrl))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateAvatarUrl(TestUserId, validUrl);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsValid_CallsDatabaseHelper()
        {
            string validUrl = "https://example.com/avatar.jpg";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, validUrl))
                .ReturnsAsync(true);

            await doctorService.UpdateAvatarUrl(TestUserId, validUrl);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorAvatarUrl(TestUserId, validUrl), Times.Once);
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_ThrowsArgumentException()
        {
            string tooLongUrl = new string('a', 256);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateAvatarUrl(TestUserId, tooLongUrl));

            Assert.That(exception.Message, Does.StartWith("Avatar URL is too long."));
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongUrl = new string('a', 256);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateAvatarUrl(TestUserId, tooLongUrl));

            Assert.AreEqual("avatarUrl", exception.ParamName);
        }

        [Test]
        public void UpdateAvatarUrl_WhenUrlIsTooLong_DoesNotCallDatabaseHelper()
        {
            string tooLongUrl = new string('a', 256);

            try
            {
                doctorService.UpdateAvatarUrl(TestUserId, tooLongUrl).Wait();
            }
            catch { /* Expected exception */ }

            mockDatabaseHelper.Verify(db => db.UpdateDoctorAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsNull_PassesEmptyStringToDatabaseHelper()
        {
            mockDatabaseHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, string.Empty))
                .ReturnsAsync(true);

            await doctorService.UpdateAvatarUrl(TestUserId, null);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorAvatarUrl(TestUserId, string.Empty), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrl_WhenUrlIsNull_ReturnsTrue()
        {
            mockDatabaseHelper.Setup(db => db.UpdateDoctorAvatarUrl(TestUserId, string.Empty))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateAvatarUrl(TestUserId, null);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumber_WhenPhoneNumberIsValid_ReturnsTrue()
        {
            string validPhoneNumber = "1234567890";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorPhoneNumber(TestUserId, validPhoneNumber))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdatePhoneNumber(TestUserId, validPhoneNumber);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumber_WhenPhoneNumberIsValid_CallsDatabaseHelper()
        {
            string validPhoneNumber = "1234567890";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorPhoneNumber(TestUserId, validPhoneNumber))
                .ReturnsAsync(true);

            await doctorService.UpdatePhoneNumber(TestUserId, validPhoneNumber);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorPhoneNumber(TestUserId, validPhoneNumber), Times.Once);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsEmpty_ThrowsArgumentException()
        {
            string emptyPhoneNumber = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, emptyPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsEmpty_IncludesCorrectParameterName()
        {
            string emptyPhoneNumber = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, emptyPhoneNumber));

            Assert.AreEqual("phoneNumber", exception.ParamName);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsWhitespace_ThrowsArgumentException()
        {
            string whitespacePhoneNumber = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, whitespacePhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsTooShort_ThrowsArgumentException()
        {
            string tooShortPhoneNumber = "123456789";  // 9 digits

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, tooShortPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsTooLong_ThrowsArgumentException()
        {
            string tooLongPhoneNumber = "12345678901"; // 11 digits

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, tooLongPhoneNumber));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsNonDigits_ThrowsArgumentException()
        {
            string phoneNumberWithNonDigits = "123-456-789";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, phoneNumberWithNonDigits));

            Assert.That(exception.Message, Does.StartWith("Phone number must be exactly 10 digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsLetters_ThrowsArgumentException()
        {
            string phoneNumberWithLetters = "123456789a";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, phoneNumberWithLetters));

            Assert.That(exception.Message, Does.StartWith("Phone number must contain only digits."));
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberContainsLetters_IncludesCorrectParameterName()
        {
            string phoneNumberWithLetters = "123456789a";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdatePhoneNumber(TestUserId, phoneNumberWithLetters));

            Assert.AreEqual("phoneNumber", exception.ParamName);
        }

        [Test]
        public void UpdatePhoneNumber_WhenPhoneNumberIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidPhoneNumber = "123456789";

            try
            {
                doctorService.UpdatePhoneNumber(TestUserId, invalidPhoneNumber).Wait();
            }
            catch { /* Expected exception */ }

            mockDatabaseHelper.Verify(db => db.UpdateDoctorPhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateEmail_WhenEmailIsValid_ReturnsTrue()
        {
            string validEmail = "doctor@hospital.com";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorEmail(TestUserId, validEmail))
                .ReturnsAsync(true);

            bool result = await doctorService.UpdateEmail(TestUserId, validEmail);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateEmail_WhenEmailIsValid_CallsDatabaseHelper()
        {
            string validEmail = "doctor@hospital.com";

            mockDatabaseHelper.Setup(db => db.UpdateDoctorEmail(TestUserId, validEmail))
                .ReturnsAsync(true);

            await doctorService.UpdateEmail(TestUserId, validEmail);

            mockDatabaseHelper.Verify(db => db.UpdateDoctorEmail(TestUserId, validEmail), Times.Once);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsEmpty_ThrowsArgumentException()
        {
            string emptyEmail = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, emptyEmail));

            Assert.That(exception.Message, Does.StartWith("Mail cannot be empty."));
        }

        [Test]
        public void UpdateEmail_WhenEmailIsEmpty_IncludesCorrectParameterName()
        {
            string emptyEmail = "";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, emptyEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsWhitespace_ThrowsArgumentException()
        {
            string whitespaceEmail = "   ";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, whitespaceEmail));

            Assert.That(exception.Message, Does.StartWith("Mail cannot be empty."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingAtSymbol_ThrowsArgumentException()
        {
            string invalidEmail = "doctor";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingDomain_ThrowsArgumentException()
        {
            string invalidEmail = "doctor@hospital";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingDot_ThrowsArgumentException()
        {
            string invalidEmail = "doctor.hospital";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.That(exception.Message, Does.StartWith("Mail must contain '@' and '.'."));
        }

        [Test]
        public void UpdateEmail_WhenEmailMissingAtSymbol_IncludesCorrectParameterName()
        {
            string invalidEmail = "doctor";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, invalidEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsTooLong_ThrowsArgumentException()
        {
            string tooLongEmail = new string('a', 90) + "@hospital.com"; // More than 100 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, tooLongEmail));

            Assert.That(exception.Message, Does.StartWith("Mail is too long."));
        }

        [Test]
        public void UpdateEmail_WhenEmailIsTooLong_IncludesCorrectParameterName()
        {
            string tooLongEmail = new string('a', 90) + "@hospital.com"; // More than 100 characters

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await doctorService.UpdateEmail(TestUserId, tooLongEmail));

            Assert.AreEqual("email", exception.ParamName);
        }

        [Test]
        public void UpdateEmail_WhenEmailIsInvalid_DoesNotCallDatabaseHelper()
        {
            string invalidEmail = "doctor";

            try
            {
                doctorService.UpdateEmail(TestUserId, invalidEmail).Wait();
            }
            catch { /* Expected exception */ }

            mockDatabaseHelper.Verify(db => db.UpdateDoctorEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task LogUpdate_WhenCalled_ReturnsTrue()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            mockDatabaseHelper.Setup(db => db.UpdateLogService(TestUserId, actionType))
                .ReturnsAsync(true);

            bool result = await doctorService.LogUpdate(TestUserId, actionType);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LogUpdate_WhenCalled_CallsDatabaseHelper()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            mockDatabaseHelper.Setup(db => db.UpdateLogService(TestUserId, actionType))
                .ReturnsAsync(true);

            await doctorService.LogUpdate(TestUserId, actionType);

            mockDatabaseHelper.Verify(db => db.UpdateLogService(TestUserId, actionType), Times.Once);
        }
    }
}