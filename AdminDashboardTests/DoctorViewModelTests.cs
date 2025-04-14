using Hospital.Doctor_Dashboard;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

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
        public async Task LoadDoctorInformationAsync_ShouldUpdateProperties_WhenSuccessful()
        {
            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsTrue(result);
            Assert.AreEqual("Dr. Jane Doe", _viewModel.DoctorName);
            Assert.AreEqual("Cardiology", _viewModel.DepartmentName);
            Assert.AreEqual(4.5, _viewModel.Rating);
            Assert.AreEqual("Experienced Cardiologist", _viewModel.CareerInfo);
            Assert.AreEqual("https://example.com/avatar.jpg", _viewModel.AvatarUrl);
            Assert.AreEqual("1234567890", _viewModel.PhoneNumber);
            Assert.AreEqual("doctor@example.com", _viewModel.Mail);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_ShouldReturnDefaultValues_WhenDoctorNotFound()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(false);

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
            Assert.AreEqual("Doctor profile not found", _viewModel.DoctorName);
            Assert.AreEqual("N/A", _viewModel.DepartmentName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_ShouldHandleExceptions()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
            Assert.AreEqual("Error loading profile", _viewModel.DoctorName);
            Assert.AreEqual("Error", _viewModel.DepartmentName);
            Assert.AreEqual("Please try again later", _viewModel.CareerInfo);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_ShouldHandleDefaultDoctorInformation()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(DoctorModel.Default);

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsFalse(result);
            Assert.AreEqual("Doctor profile not found", _viewModel.DoctorName);
        }

        [Test]
        public async Task LoadDoctorInformationAsync_ShouldSetIsLoadingPropertyCorrectly()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .Callback(() => loadingDuringExecution = _viewModel.IsLoading)
                .ReturnsAsync(true);

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsTrue(loadingDuringExecution); 
            Assert.IsFalse(_viewModel.IsLoading); 
        }

        [Test]
        public async Task LoadDoctorInformationAsync_ShouldHandleNullFields()
        {
            _mockDoctorService.Setup(service => service.LoadDoctorInformationByUserId(It.IsAny<int>()))
                .ReturnsAsync(true);
            _mockDoctorService.SetupGet(service => service.DoctorInformation)
                .Returns(new DoctorModel
                {
                    DoctorName = null,
                    DepartmentId = 456,
                    DepartmentName = null,
                    Rating = 0,
                    CareerInfo = null,
                    AvatarUrl = null,
                    PhoneNumber = null,
                    Mail = null
                });

            bool result = await _viewModel.LoadDoctorInformationAsync(TestUserId);

            Assert.IsTrue(result);
            Assert.AreEqual("Not specified", _viewModel.DoctorName);
            Assert.AreEqual("N/A", _viewModel.DepartmentName);
            Assert.AreEqual(0.0, _viewModel.Rating);
            Assert.AreEqual("N/A", _viewModel.CareerInfo);
            Assert.AreEqual("https://picsum.photos/200", _viewModel.AvatarUrl);
            Assert.AreEqual("Not provided", _viewModel.PhoneNumber);
            Assert.AreEqual("Not provided", _viewModel.Mail);
        }

        [Test]
        public void UpdateDoctorName_ShouldUpdateDoctorName_WhenValid()
        {
            var newName = "Dr. John Doe";

            var result = _viewModel.UpdateDoctorNameAsync(newName).Result;

            Assert.IsTrue(result);
            Assert.AreEqual(newName, _viewModel.DoctorName);
            Assert.AreEqual(newName, _viewModel.OriginalDoctor.DoctorName);
            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, newName), Times.Once);
        }

        [Test]
        public async Task UpdateDepartment_ShouldUpdateDepartment_WhenValid()
        {
            var newDepartmentId = 789;

            var result = await _viewModel.UpdateDepartmentAsync(newDepartmentId);

            Assert.IsTrue(result);
            Assert.AreEqual(newDepartmentId, _viewModel.DepartmentId);
            Assert.AreEqual(newDepartmentId, _viewModel.OriginalDoctor.DepartmentId);
            _mockDoctorService.Verify(s => s.UpdateDepartment(TestUserId, newDepartmentId), Times.Once);
        }

        [Test]
        public async Task UpdateCareerInfo_ShouldUpdateCareerInfo_WhenValid()
        {
            var newCareerInfo = "Updated career information";

            var result = await _viewModel.UpdateCareerInfoAsync(newCareerInfo);

            Assert.IsTrue(result);
            Assert.AreEqual(newCareerInfo, _viewModel.CareerInfo);
            Assert.AreEqual(newCareerInfo, _viewModel.OriginalDoctor.CareerInfo);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(TestUserId, newCareerInfo), Times.Once);
        }

        [Test]
        public async Task UpdateAvatarUrl_ShouldUpdateAvatarUrl_WhenValid()
        {
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            var result = await _viewModel.UpdateAvatarUrlAsync(newAvatarUrl);

            Assert.IsTrue(result);
            Assert.AreEqual(newAvatarUrl, _viewModel.AvatarUrl);
            Assert.AreEqual(newAvatarUrl, _viewModel.OriginalDoctor.AvatarUrl);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(TestUserId, newAvatarUrl), Times.Once);
        }

        [Test]
        public async Task UpdatePhoneNumber_ShouldUpdatePhoneNumber_WhenValid()
        {
            var newPhoneNumber = "9876543210";

            var result = await _viewModel.UpdatePhoneNumberAsync(newPhoneNumber);

            Assert.IsTrue(result);
            Assert.AreEqual(newPhoneNumber, _viewModel.PhoneNumber);
            Assert.AreEqual(newPhoneNumber, _viewModel.OriginalDoctor.PhoneNumber);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, newPhoneNumber), Times.Once);
        }

        [Test]
        public async Task UpdateMail_ShouldUpdateMail_WhenValid()
        {
            var newEmail = "newemail@example.com";

            var result = await _viewModel.UpdateMailAsync(newEmail);

            Assert.IsTrue(result);
            Assert.AreEqual(newEmail, _viewModel.Mail);
            Assert.AreEqual(newEmail, _viewModel.OriginalDoctor.Mail);
            _mockDoctorService.Verify(s => s.UpdateEmail(TestUserId, newEmail), Times.Once);
        }

        [Test]
        public async Task LogDoctorUpdate_ShouldCallServiceLogUpdate()
        {
            var actionType = ActionType.UPDATE_PROFILE;

            var result = await _viewModel.LogDoctorUpdateAsync(actionType);

            Assert.IsTrue(result);
            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, actionType), Times.Once);
        }

        [Test]
        public void RevertChanges_ShouldRestoreOriginalValues()
        {
            _viewModel.DoctorName = "New Name";
            _viewModel.DepartmentName = "New Department";
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "New Mail";

            _viewModel.LoadDoctorInformationAsync(TestUserId).Wait();
            string originalDoctorName = _viewModel.OriginalDoctor.DoctorName;
            string originalDepartmentName = _viewModel.OriginalDoctor.DepartmentName;
            string originalCareerInfo = _viewModel.OriginalDoctor.CareerInfo;
            string originalAvatarUrl = _viewModel.OriginalDoctor.AvatarUrl;
            string originalPhoneNumber = _viewModel.OriginalDoctor.PhoneNumber;
            string originalMail = _viewModel.OriginalDoctor.Mail;

            _viewModel.DoctorName = "Different Name";
            _viewModel.DepartmentName = "Different Department";
            _viewModel.CareerInfo = "Different Career Info";
            _viewModel.AvatarUrl = "Different URL";
            _viewModel.PhoneNumber = "Different Phone";
            _viewModel.Mail = "Different Mail";
            _viewModel.RevertChanges();

            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName);
            Assert.AreEqual(originalDepartmentName, _viewModel.DepartmentName);
            Assert.AreEqual(originalCareerInfo, _viewModel.CareerInfo);
            Assert.AreEqual(originalAvatarUrl, _viewModel.AvatarUrl);
            Assert.AreEqual(originalPhoneNumber, _viewModel.PhoneNumber);
            Assert.AreEqual(originalMail, _viewModel.Mail);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_ShouldUpdateAllChangedFields()
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
            Assert.IsNull(errorMessage);

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, "New Doctor Name"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateDepartment(TestUserId, 789), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(TestUserId, "New Career Info"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(TestUserId, "New Avatar URL"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, "New Phone"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateEmail(TestUserId, "new.email@example.com"), Times.Once);
            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_ShouldHandleNoChanges()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful); 
            Assert.IsNull(errorMessage);

            _mockDoctorService.Verify(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.LogUpdate(It.IsAny<int>(), It.IsAny<ActionType>()), Times.Never);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_ShouldHandlePartialChanges()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);

            _viewModel.DoctorName = "New Doctor Name";
            _viewModel.PhoneNumber = "New Phone";

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsTrue(updateSuccessful);
            Assert.IsNull(errorMessage);

            _mockDoctorService.Verify(s => s.UpdateDoctorName(TestUserId, "New Doctor Name"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdatePhoneNumber(TestUserId, "New Phone"), Times.Once);
            _mockDoctorService.Verify(s => s.UpdateDepartment(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateCareerInfo(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateAvatarUrl(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.UpdateEmail(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockDoctorService.Verify(s => s.LogUpdate(TestUserId, ActionType.UPDATE_PROFILE), Times.Once);
        }

        [Test]
        public async Task TryUpdateDoctorProfileAsync_ShouldHandleExceptions()
        {
            await _viewModel.LoadDoctorInformationAsync(TestUserId);
            _viewModel.DoctorName = "New Doctor Name";

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            string originalDoctorName = _viewModel.OriginalDoctor.DoctorName;

            var (updateSuccessful, errorMessage) = await _viewModel.TryUpdateDoctorProfileAsync();

            Assert.IsFalse(updateSuccessful);
            Assert.AreEqual("Test exception", errorMessage);

            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName);
        }

        [Test]
        public async Task UpdateFieldAsync_ShouldHandleExceptions()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await _viewModel.UpdateDoctorNameAsync("New Name"));

            Assert.AreEqual("Test exception", exception.Message);
        }

        [Test]
        public async Task UpdateFieldAsync_ShouldSetIsLoadingCorrectly()
        {
            bool loadingDuringExecution = false;

            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .Callback(() => loadingDuringExecution = _viewModel.IsLoading)
                .ReturnsAsync(true);

            var result = await _viewModel.UpdateDoctorNameAsync("New Name");

            Assert.IsTrue(loadingDuringExecution); 
            Assert.IsFalse(_viewModel.IsLoading); 
        }

        [Test]
        public async Task UpdateFieldAsync_ShouldReturnFalse_WhenServiceReturnsFalse()
        {
            _mockDoctorService.Setup(s => s.UpdateDoctorName(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            string originalDoctorName = _viewModel.DoctorName;
            string testName = "New Name";

            var result = await _viewModel.UpdateDoctorNameAsync(testName);

            Assert.IsFalse(result);
            Assert.AreEqual(originalDoctorName, _viewModel.DoctorName); // Name should not change
        }

        [Test]
        public void PropertyChangedEvents_ShouldBeRaised_WhenPropertiesChange()
        {
            var propertiesChanged = new System.Collections.Generic.List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
                propertiesChanged.Add(args.PropertyName);

            _viewModel.UserId = 456;
            _viewModel.DoctorName = "New Name";
            _viewModel.DepartmentId = 789;
            _viewModel.DepartmentName = "New Department";
            _viewModel.Rating = 5.0;
            _viewModel.CareerInfo = "New Career Info";
            _viewModel.AvatarUrl = "New Avatar URL";
            _viewModel.PhoneNumber = "New Phone";
            _viewModel.Mail = "new.email@example.com";
            _viewModel.IsLoading = true;

            Assert.AreEqual(10, propertiesChanged.Count);
            Assert.Contains("UserId", propertiesChanged);
            Assert.Contains("DoctorName", propertiesChanged);
            Assert.Contains("DepartmentId", propertiesChanged);
            Assert.Contains("DepartmentName", propertiesChanged);
            Assert.Contains("Rating", propertiesChanged);
            Assert.Contains("CareerInfo", propertiesChanged);
            Assert.Contains("AvatarUrl", propertiesChanged);
            Assert.Contains("PhoneNumber", propertiesChanged);
            Assert.Contains("Mail", propertiesChanged);
            Assert.Contains("IsLoading", propertiesChanged);
        }

        [Test]
        public void TryUpdateDoctorProfileAsync_ShouldHandleNullOriginalDoctor()
        {
            var viewModel = new DoctorViewModel(_mockDoctorService.Object, TestUserId);
            var property = typeof(DoctorViewModel).GetProperty("OriginalDoctor");
            property.SetValue(viewModel, null);

            var (updateSuccessful, errorMessage) = viewModel.TryUpdateDoctorProfileAsync().Result;

            Assert.IsFalse(updateSuccessful);
            Assert.AreEqual("Original doctor data is not initialized.", errorMessage);
        }
    }
}