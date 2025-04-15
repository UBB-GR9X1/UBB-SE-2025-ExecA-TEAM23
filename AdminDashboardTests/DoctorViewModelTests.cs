using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Hospital.Doctor_Dashboard;

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

            _viewModel = new DoctorViewModel(_mockDoctorService.Object, TestUserId);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_ReturnsTrue()
        {
            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesDoctorName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Dr. Jane Doe", _viewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesDepartmentName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Cardiology", _viewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesRating()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual(4.5, _viewModel.Rating);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesCareerInfo()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Experienced Cardiologist", _viewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesAvatarUrl()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("https://example.com/avatar.jpg", _viewModel.AvatarUrl);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesPhoneNumber()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("1234567890", _viewModel.PhoneNumber);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenSuccessful_UpdatesEmail()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("doctor@example.com", _viewModel.Mail);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_SetsDoctorNameToNotFound()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Doctor profile not found", _viewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenDoctorNotFound_SetsDepartmentNameToNotAvailable()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("N/A", _viewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsDoctorNameToErrorMessage()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Error loading profile", _viewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsDepartmentNameToError()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Error", _viewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WhenExceptionOccurs_SetsCareerInfoToErrorMessage()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Please try again later", _viewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithDefaultDoctorInformation_ReturnsFalse()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(DoctorModel.Default);

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_WithDefaultDoctorInformation_SetsDoctorNameToNotFound()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(DoctorModel.Default);

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Doctor profile not found", _viewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_DuringExecution_SetsIsLoadingToTrue()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .Callback(() => loadingDuringExecution = _viewModel.IsLoading)
                .ReturnsAsync(true);

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsTrue(loadingDuringExecution);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_AfterExecution_SetsIsLoadingToFalse()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(_viewModel.IsLoading);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Not specified", _viewModel.DoctorName);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("N/A", _viewModel.DepartmentName);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("N/A", _viewModel.CareerInfo);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("https://picsum.photos/200", _viewModel.AvatarUrl);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Not provided", _viewModel.PhoneNumber);
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

            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.AreEqual("Not provided", _viewModel.Mail);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_ReturnsTrue()
        {
            var newName = "Dr. John Doe";

            var result = await _viewModel.UpdateDoctorNameAsync(newName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_UpdatesDoctorNameProperty()
        {
            var newName = "Dr. John Doe";

            await _viewModel.UpdateDoctorNameAsync(newName);

            Assert.AreEqual(newName, _viewModel.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_UpdatesOriginalDoctorName()
        {
            var newName = "Dr. John Doe";

            await _viewModel.UpdateDoctorNameAsync(newName);

            Assert.AreEqual(newName, _viewModel.OriginalDoctor.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newName = "Dr. John Doe";

            await _viewModel.UpdateDoctorNameAsync(newName);

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, newName), Times.Once);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_ReturnsTrue()
        {
            var newDepartmentId = 789;

            var result = await _viewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_UpdatesDepartmentIdProperty()
        {
            var newDepartmentId = 789;

            await _viewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.AreEqual(newDepartmentId, _viewModel.DepartmentId);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_UpdatesOriginalDepartmentId()
        {
            var newDepartmentId = 789;

            await _viewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.AreEqual(newDepartmentId, _viewModel.OriginalDoctor.DepartmentId);
        }

        [Test]
        public async Task UpdateDepartmentAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newDepartmentId = 789;

            await _viewModel.UpdateDepartmentAsync(newDepartmentId);

            _mockDoctorService.Verify(s => s.UpdateDepartment(TestUserId, newDepartmentId), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_ReturnsTrue()
        {
            var newCareerInfo = "Updated career information";

            var result = await _viewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_UpdatesCareerInfoProperty()
        {
            var newCareerInfo = "Updated career information";

            await _viewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.AreEqual(newCareerInfo, _viewModel.CareerInfo);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_UpdatesOriginalCareerInfo()
        {
            var newCareerInfo = "Updated career information";

            await _viewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.AreEqual(newCareerInfo, _viewModel.OriginalDoctor.CareerInfo);
        }

        [Test]
        public async Task UpdateCareerInfoAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newCareerInfo = "Updated career information";

            await _viewModel.UpdateCareerInfoAsync(newCareerInfo);

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(TestUserId, newCareerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_ReturnsTrue()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            var result = await _viewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_UpdatesAvatarUrlProperty()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _viewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.AreEqual(newAvatarUrl, _viewModel.AvatarUrl);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_UpdatesOriginalAvatarUrl()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _viewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.AreEqual(newAvatarUrl, _viewModel.OriginalDoctor.AvatarUrl);
        }

        [Test]
        public async Task UpdateAvatarUrlAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            await _viewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(TestUserId, newAvatarUrl), Times.Once);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_ReturnsTrue()
        {
            var newPhoneNumber = "9876543210";

            var result = await _viewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_UpdatesPhoneNumberProperty()
        {
            var newPhoneNumber = "9876543210";

            await _viewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.AreEqual(newPhoneNumber, _viewModel.PhoneNumber);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_UpdatesOriginalPhoneNumber()
        {
            var newPhoneNumber = "9876543210";

            await _viewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.AreEqual(newPhoneNumber, _viewModel.OriginalDoctor.PhoneNumber);
        }

        [Test]
        public async Task UpdatePhoneNumberAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newPhoneNumber = "9876543210";

            await _viewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, newPhoneNumber), Times.Once);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_ReturnsTrue()
        {
            var newEmail = "newemail@example.com";

            var result = await _viewModel.UpdateMailAsync(newEmail);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_UpdatesMailProperty()
        {
            var newEmail = "newemail@example.com";

            await _viewModel.UpdateMailAsync(newEmail);

            Assert.AreEqual(newEmail, _viewModel.Mail);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_UpdatesOriginalMail()
        {
            var newEmail = "newemail@example.com";

            await _viewModel.UpdateMailAsync(newEmail);

            Assert.AreEqual(newEmail, _viewModel.OriginalDoctor.Mail);
        }

        [Test]
        public async Task UpdateMailAsync_WhenValid_CallsServiceWithCorrectParameters()
        {
            var newEmail = "newemail@example.com";

            await _viewModel.UpdateMailAsync(newEmail);

            _mockDoctorService.Verify(s => s.UpdateEmail(TestUserId, newEmail), Times.Once);
        }

        [Test]
        public async Task LogDoctorUpdateAsync_WhenCalled_ReturnsTrue()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            var result = await _viewModel.LogDoctorUpdateAsync(actionType);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task LogDoctorUpdateAsync_WhenCalled_CallsServiceWithCorrectParameters()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            await _viewModel.LogDoctorUpdateAsync(actionType);

            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, actionType), Times.Once);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresDoctorName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalDoctorName = _viewModel.OriginalDoctor.DoctorName;
            _viewModel.DoctorName = "Different Name";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresDepartmentName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalDepartmentName = _viewModel.OriginalDoctor.DepartmentName;
            _viewModel.DepartmentName = "Different Department";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalDepartmentName, _viewModel.DepartmentName);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresCareerInfo()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalCareerInfo = _viewModel.OriginalDoctor.CareerInfo;
            _viewModel.CareerInfo = "Different Career Info";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalCareerInfo, _viewModel.CareerInfo);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresAvatarUrl()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalAvatarUrl = _viewModel.OriginalDoctor.AvatarUrl;
            _viewModel.AvatarUrl = "Different URL";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalAvatarUrl, _viewModel.AvatarUrl);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresPhoneNumber()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalPhoneNumber = _viewModel.OriginalDoctor.PhoneNumber;
            _viewModel.PhoneNumber = "Different Phone";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalPhoneNumber, _viewModel.PhoneNumber);
        }

        [Test]
        public async Task RevertChanges_WhenCalled_RestoresMail()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalMail = _viewModel.OriginalDoctor.Mail;
            _viewModel.Mail = "Different Mail";

            _viewModel.RevertChanges();

            Assert.AreEqual(originalMail, _viewModel.Mail);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_ReturnsSuccessTrue()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsTrue(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_ReturnsNullErrorMessage()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateDoctorName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, "New Doctor Name"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateDepartment()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDepartment(TestUserId, 789), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateCareerInfo()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(TestUserId, "New Career Info"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateAvatarUrl()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(TestUserId, "New Avatar URL"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdatePhoneNumber()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, "New Phone"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsUpdateEmail()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateEmail(TestUserId, "new.email@example.com"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenAllFieldsChanged_CallsLogUpdate()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_ReturnsFalse()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_ReturnsNullErrorMessage()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateDoctorName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateDepartment()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateCareerInfo()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateAvatarUrl()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdatePhoneNumber()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallUpdateEmail()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenNoChanges_DoesNotCallLogUpdate()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(It.IsAny<int>(), It.IsAny<ActionType>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOnlyDoctorNameChanged_CallsUpdateDoctorName()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, "New Doctor Name"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOnlyPhoneNumberChanged_CallsUpdatePhoneNumber()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.PhoneNumber = "New Phone";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, "New Phone"), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_ReturnsTrue()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.PhoneNumber = "New Phone";

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsTrue(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_ReturnsNullErrorMessage()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.PhoneNumber = "New Phone";

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsNull(errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_OnlyCallsRelevantUpdateMethods()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.PhoneNumber = "New Phone";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, "New Doctor Name"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, "New Phone"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenPartialChanges_CallsLogUpdate()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.PhoneNumber = "New Phone";

            await _viewModel.TryUpdateDoctorProfileAsync();

            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_ReturnsFalse()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_ReturnsErrorMessage()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual("Test exception", errorMessage);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenExceptionOccurs_RevertsToDoctorNameOriginalValue()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            string originalDoctorName = _viewModel.OriginalDoctor.DoctorName;
            _viewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenExceptionOccurs_ThrowsException()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _viewModel.UpdateDoctorNameAsync("New Name"));

            Assert.AreEqual("Test exception", exception.Message);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_DuringExecution_SetsIsLoadingToTrue()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .Callback(() => loadingDuringExecution = _viewModel.IsLoading)
                .ReturnsAsync(true);

            await _viewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsTrue(loadingDuringExecution);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_AfterExecution_SetsIsLoadingToFalse()
        {
            await _viewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsFalse(_viewModel.IsLoading);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenServiceReturnsFalse_ReturnsFalse()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _viewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateDoctorNameAsync_WhenServiceReturnsFalse_DoesNotUpdateDoctorName()
        {
            string originalDoctorName = _viewModel.DoctorName;
            string testName = "New Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            await _viewModel.UpdateDoctorNameAsync(testName);

            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName);
        }

        [Test]
        public void PropertyChangedEvent_WhenUserIdChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.UserId = 456;

            Assert.Contains("UserId", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDoctorNameChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.DoctorName = "New Name";

            Assert.Contains("DoctorName", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDepartmentIdChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.DepartmentId = 789;

            Assert.Contains("DepartmentId", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenDepartmentNameChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.DepartmentName = "New Department";

            Assert.Contains("DepartmentName", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenRatingChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.Rating = 5.0;

            Assert.Contains("Rating", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenCareerInfoChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.CareerInfo = "New Career Info";

            Assert.Contains("CareerInfo", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenAvatarUrlChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.AvatarUrl = "New Avatar URL";

            Assert.Contains("AvatarUrl", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenPhoneNumberChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.PhoneNumber = "New Phone";

            Assert.Contains("PhoneNumber", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenMailChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.Mail = "new.email@example.com";

            Assert.Contains("Mail", propertiesChanged);
        }

        [Test]
        public void PropertyChangedEvent_WhenIsLoadingChanges_RaisesPropertyChangedEvent()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.IsLoading = true;

            Assert.Contains("IsLoading", propertiesChanged);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOriginalDoctorIsNull_ReturnsFalse()
        {
            var viewModel = new DoctorViewModel(_mockDoctorService.Object, TestUserId);
            var property = typeof(DoctorViewModel).GetProperty("OriginalDoctor");
            property.SetValue(viewModel, null);

            var (updateSuccessful, errorMessage) = await viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_WhenOriginalDoctorIsNull_ReturnsCorrectErrorMessage()
        {
            var viewModel = new DoctorViewModel(_mockDoctorService.Object, TestUserId);
            var property = typeof(DoctorViewModel).GetProperty("OriginalDoctor");
            property.SetValue(viewModel, null);

            var (updateSuccessful, errorMessage) = await viewModel.TryUpdateDoctorProfileAsync();

            Assert.AreEqual("Original doctor data is not initialized.", errorMessage);
        }
    }
}