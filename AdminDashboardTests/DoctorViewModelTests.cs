using Hospital.Repositories;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Hospital.Services;

namespace AdminDashboardTests
{
    public class DoctorViewModelTests
    {
        private Mock<IDoctorService> _mockDoctorService;
        private DoctorViewModel _doctorViewModel;
        private const int _testUserId = 123;

        [SetUp]
        public void Setup()
        {
            _mockDoctorService = new Mock<IDoctorService>();

            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdatePhoneNumber(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockDoctorService.Setup(service => service.LogUpdate(It.IsAny<int>(), It.IsAny<ActionType>()))
                .ReturnsAsync(true);

            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            _doctorViewModel = new DoctorViewModel(_mockDoctorService.Object, _testUserId);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_ReturnsTrue()
        {
            bool result = await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesDoctorName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Dr. Jane Doe", _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesDepartmentName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Cardiology", _doctorViewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesRating()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual(4.5, _doctorViewModel.Rating);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesCareerInfo()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Experienced Cardiologist", _doctorViewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesAvatarUrl()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("https://example.com/avatar.jpg", _doctorViewModel.AvatarUrl);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesPhoneNumber()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("1234567890", _doctorViewModel.PhoneNumber);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesEmail()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("doctor@example.com", _doctorViewModel.Mail);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            bool result = await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_SetsDoctorNameToNotFound()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Doctor profile not found", _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_SetsDepartmentNameToNotAvailable()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("N/A", _doctorViewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            bool result = await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsDoctorNameToErrorMessage()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Error loading profile", _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsDepartmentNameToError()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Error", _doctorViewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsCareerInfoToErrorMessage()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Please try again later", _doctorViewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithDefaultDoctorInformation_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(DoctorModel.Default);

            bool result = await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithDefaultDoctorInformation_SetsDoctorNameToNotFound()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(DoctorModel.Default);

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Doctor profile not found", _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_DuringExecution_SetsIsLoadingToTrue()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .Callback(() => loadingDuringExecution = _doctorViewModel.IsLoading)
                .ReturnsAsync(true);

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsTrue(loadingDuringExecution);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_AfterExecution_SetsIsLoadingToFalse()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.IsFalse(_doctorViewModel.IsLoading);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullDoctorName_SetsDefaultDoctorName()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = null,
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Not specified", _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullDepartmentName_SetsDefaultDepartmentName()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = null,
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("N/A", _doctorViewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullCareerInfo_SetsDefaultCareerInfo()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = null,
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("N/A", _doctorViewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullAvatarUrl_SetsDefaultAvatarUrl()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = null,
                    PhoneNumber = "1234567890",
                    Mail = "doctor@example.com"
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("https://picsum.photos/200", _doctorViewModel.AvatarUrl);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullPhoneNumber_SetsDefaultPhoneNumber()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = null,
                    Mail = "doctor@example.com"
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Not provided", _doctorViewModel.PhoneNumber);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithNullEmail_SetsDefaultEmail()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = "Dr. Jane Doe",
                    DepartmentId = 456,
                    DepartmentName = "Cardiology",
                    Rating = 4.5,
                    CareerInfo = "Experienced Cardiologist",
                    AvatarUrl = "https://example.com/avatar.jpg",
                    PhoneNumber = "1234567890",
                    Mail = null
                });

            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            Assert.AreEqual("Not provided", _doctorViewModel.Mail);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_ReturnsTrue()
        {
            var newName = "Dr. John Doe";

            var result = await _doctorViewModel.UpdateDoctorNameAsync(newName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_UpdatesDoctorNameProperty()
        {
            var newName = "Dr. John Doe";

            await _doctorViewModel.UpdateDoctorNameAsync(newName);

            Assert.AreEqual(newName, _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_UpdatesOriginalDoctorName()
        {
            var newName = "Dr. John Doe";

            await _doctorViewModel.UpdateDoctorNameAsync(newName);

            Assert.AreEqual(newName, _doctorViewModel.OriginalDoctor.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newName = "Dr. John Doe";

            await _doctorViewModel.UpdateDoctorNameAsync(newName);

            _mockDoctorService.Verify(s => s.UpdateDoctorName(_testUserId, newName), Times.Once);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_ReturnsTrue()
        {
            var newDepartmentId = 789;

            var result = await _doctorViewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_UpdatesDepartmentIdProperty()
        {
            var newDepartmentId = 789;

            await _doctorViewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.AreEqual(newDepartmentId, _doctorViewModel.DepartmentId);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_UpdatesOriginalDepartmentId()
        {
            var newDepartmentId = 789;

            await _doctorViewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.AreEqual(newDepartmentId, _doctorViewModel.OriginalDoctor.DepartmentId);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newDepartmentId = 789;

            await _doctorViewModel.UpdateDepartmentAsync(newDepartmentId);

            _mockDoctorService.Verify(s => s.UpdateDepartment(_testUserId, newDepartmentId), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_ReturnsTrue()
        {
            var newCareerInfo = "Updated career information";

            var result = await _doctorViewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_UpdatesCareerInfoProperty()
        {
            var newCareerInfo = "Updated career information";

            await _doctorViewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.AreEqual(newCareerInfo, _doctorViewModel.CareerInfo);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_UpdatesOriginalCareerInfo()
        {
            var newCareerInfo = "Updated career information";

            await _doctorViewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.AreEqual(newCareerInfo, _doctorViewModel.OriginalDoctor.CareerInfo);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newCareerInfo = "Updated career information";

            await _doctorViewModel.UpdateCareerInfoAsync(newCareerInfo);

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(_testUserId, newCareerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_ReturnsTrue()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            var result = await _doctorViewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_UpdatesAvatarUrlProperty()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _doctorViewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.AreEqual(newAvatarUrl, _doctorViewModel.AvatarUrl);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_UpdatesOriginalAvatarUrl()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _doctorViewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.AreEqual(newAvatarUrl, _doctorViewModel.OriginalDoctor.AvatarUrl);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _doctorViewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(_testUserId, newAvatarUrl), Times.Once);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_ReturnsTrue()
        {
            var newPhoneNumber = "9876543210";

            var result = await _doctorViewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_UpdatesPhoneNumberProperty()
        {
            var newPhoneNumber = "9876543210";

            await _doctorViewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.AreEqual(newPhoneNumber, _doctorViewModel.PhoneNumber);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_UpdatesOriginalPhoneNumber()
        {
            var newPhoneNumber = "9876543210";

            await _doctorViewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.AreEqual(newPhoneNumber, _doctorViewModel.OriginalDoctor.PhoneNumber);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newPhoneNumber = "9876543210";

            await _doctorViewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(_testUserId, newPhoneNumber), Times.Once);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_ReturnsTrue()
        {
            var newEmail = "newemail@example.com";

            var result = await _doctorViewModel.UpdateMailAsync(newEmail);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_UpdatesMailProperty()
        {
            var newEmail = "newemail@example.com";

            await _doctorViewModel.UpdateMailAsync(newEmail);

            Assert.AreEqual(newEmail, _doctorViewModel.Mail);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_UpdatesOriginalMail()
        {
            var newEmail = "newemail@example.com";

            await _doctorViewModel.UpdateMailAsync(newEmail);

            Assert.AreEqual(newEmail, _doctorViewModel.OriginalDoctor.Mail);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newEmail = "newemail@example.com";

            await _doctorViewModel.UpdateMailAsync(newEmail);

            _mockDoctorService.Verify(s => s.UpdateEmail(_testUserId, newEmail), Times.Once);
        }

        [Test]
        public async Task LogDoctorUpdateAsync_WhenCalled_ReturnsTrue()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            var result = await _doctorViewModel.LogDoctorUpdateAsync(actionType);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LogDoctorUpdateAsync_WhenCalled_CallsServiceWithCorrectParameters()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            await _doctorViewModel.LogDoctorUpdateAsync(actionType);

            _mockDoctorService.Verify(s => s.LogUpdate(_testUserId, actionType), Times.Once);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresDoctorName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalDoctorName = _doctorViewModel.OriginalDoctor.DoctorName;
            _doctorViewModel.DoctorName = "Different Name";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalDoctorName, _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresDepartmentName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalDepartmentName = _doctorViewModel.OriginalDoctor.DepartmentName;
            _doctorViewModel.DepartmentName = "Different Department";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalDepartmentName, _doctorViewModel.DepartmentName);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresCareerInfo()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalCareerInfo = _doctorViewModel.OriginalDoctor.CareerInfo;
            _doctorViewModel.CareerInfo = "Different Career Info";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalCareerInfo, _doctorViewModel.CareerInfo);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresAvatarUrl()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalAvatarUrl = _doctorViewModel.OriginalDoctor.AvatarUrl;
            _doctorViewModel.AvatarUrl = "Different URL";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalAvatarUrl, _doctorViewModel.AvatarUrl);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresPhoneNumber()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalPhoneNumber = _doctorViewModel.OriginalDoctor.PhoneNumber;
            _doctorViewModel.PhoneNumber = "Different Phone";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalPhoneNumber, _doctorViewModel.PhoneNumber);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresMail()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalMail = _doctorViewModel.OriginalDoctor.Mail;
            _doctorViewModel.Mail = "Different Mail";

            _doctorViewModel.RevertChanges();

            Assert.AreEqual(originalMail, _doctorViewModel.Mail);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_ReturnsSuccessTrue()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsTrue(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_ReturnsNullErrorMessage()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateDoctorName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(_testUserId, "New Doctor Name"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateDepartment()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDepartment(_testUserId, 789), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateCareerInfo()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(_testUserId, "New Career Info"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateAvatarUrl()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(_testUserId, "New Avatar URL"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdatePhoneNumber()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(_testUserId, "New Phone"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateEmail()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateEmail(_testUserId, "new.email@example.com"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsLogUpdate()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.DepartmentId = 789;
            _doctorViewModel.DepartmentName = "New Department";
            _doctorViewModel.CareerInfo = "New Career Info";
            _doctorViewModel.AvatarUrl = "New Avatar URL";
            _doctorViewModel.PhoneNumber = "New Phone";
            _doctorViewModel.Mail = "new.email@example.com";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(_testUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_ReturnsFalse()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_ReturnsNullErrorMessage()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateDoctorName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateDepartment()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateCareerInfo()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateAvatarUrl()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdatePhoneNumber()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateEmail()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallLogUpdate()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(It.IsAny<int>(), It.IsAny<ActionType>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOnlyDoctorNameChanged_CallsUpdateDoctorName()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(_testUserId, "New Doctor Name"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOnlyPhoneNumberChanged_CallsUpdatePhoneNumber()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.PhoneNumber = "New Phone";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(_testUserId, "New Phone"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_ReturnsTrue()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.PhoneNumber = "New Phone";

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsTrue(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_ReturnsNullErrorMessage()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.PhoneNumber = "New Phone";

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_OnlyCallsRelevantUpdateMethods()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.PhoneNumber = "New Phone";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(_testUserId, "New Doctor Name"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(_testUserId, "New Phone"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_CallsLogUpdate()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";
            _doctorViewModel.PhoneNumber = "New Phone";

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(_testUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_ReturnsFalse()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_ReturnsErrorMessage()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            _doctorViewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var (updateSuccessful, errorMessage) = await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual("Test exception", errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_RevertsToDoctorNameOriginalValue()
        {
            await _doctorViewModel.LoadDoctorInformationAsync(_testUserId);
            string originalDoctorName = _doctorViewModel.OriginalDoctor.DoctorName;
            _doctorViewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _doctorViewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual(originalDoctorName, _doctorViewModel.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenExceptionOccurs_ThrowsException()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _doctorViewModel.UpdateDoctorNameAsync("New Name"));

            Assert.AreEqual("Test exception", exception.Message);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_DuringExecution_SetsIsLoadingToTrue()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .Callback(() => loadingDuringExecution = _doctorViewModel.IsLoading)
                .ReturnsAsync(true);

            await _doctorViewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsTrue(loadingDuringExecution);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_AfterExecution_SetsIsLoadingToFalse()
        {
            await _doctorViewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsFalse(_doctorViewModel.IsLoading);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenServiceReturnsFalse_ReturnsFalse()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _doctorViewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenServiceReturnsFalse_DoesNotUpdateDoctorName()
        {
            string originalDoctorName = _doctorViewModel.DoctorName;
            string testName = "New Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            await _doctorViewModel.UpdateDoctorNameAsync(testName);

            Assert.AreEqual(originalDoctorName, _doctorViewModel.DoctorName);
        }

        [Test]
        public void PropertyChangedEvent_WhenUserIdChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.UserId = 456;

            Assert.Contains("UserId", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDoctorNameChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.DoctorName = "New Name";

            Assert.Contains("DoctorName", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDepartmentIdChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.DepartmentId = 789;

            Assert.Contains("DepartmentId", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDepartmentNameChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.DepartmentName = "New Department";

            Assert.Contains("DepartmentName", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenRatingChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.Rating = 5.0;

            Assert.Contains("Rating", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenCareerInfoChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.CareerInfo = "New Career Info";

            Assert.Contains("CareerInfo", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenAvatarUrlChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.AvatarUrl = "New Avatar URL";

            Assert.Contains("AvatarUrl", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenPhoneNumberChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.PhoneNumber = "New Phone";

            Assert.Contains("PhoneNumber", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenMailChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.Mail = "new.email@example.com";

            Assert.Contains("Mail", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenIsLoadingChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _doctorViewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _doctorViewModel.IsLoading = true;

            Assert.Contains("IsLoading", propertiesChanged);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOriginalDoctorIsNull_ReturnsFalse()
        {
            var viewModel = new DoctorViewModel(_mockDoctorService.Object, _testUserId);
            var property = typeof(DoctorViewModel).GetProperty("OriginalDoctor");
            property.SetValue(viewModel, null);

            var (updateSuccessful, errorMessage) = await viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOriginalDoctorIsNull_ReturnsCorrectErrorMessage()
        {
            var viewModel = new DoctorViewModel(_mockDoctorService.Object, _testUserId);
            var property = typeof(DoctorViewModel).GetProperty("OriginalDoctor");
            property.SetValue(viewModel, null);

            var (updateSuccessful, errorMessage) = await viewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual("Original doctor data is not initialized.", errorMessage);
        }
    }
}