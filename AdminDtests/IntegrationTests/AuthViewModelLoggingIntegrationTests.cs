using Hospital.Interfaces;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Hospital.Tests.IntegrationTest
{
    [TestClass]
    public class AuthViewModelLoggingIntegrationTests
    {
        private Mock<ILoginService> _mockLoginService;
        private AuthManagerModel _authManager;
        private AuthViewModel _authViewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockLoginService = new Mock<ILoginService>();
            _authManager = new AuthManagerModel(_mockLoginService.Object);
            _authViewModel = new AuthViewModel(_authManager);
        }

        [TestMethod]
        public async Task Login_Success_LogsLoginAction()
        {
            // Arrange
            const string username = "testuser";
            const string password = "password";
            const int userId = 1;
            
            var userModel = new UserAuthModel(userId, username, password, "test@example.com", "Patient");
            
            _mockLoginService.Setup(service => service.GetUserByUsername(username))
                .ReturnsAsync(userModel);
            
            _mockLoginService.Setup(service => service.AuthenticationLogService(userId, ActionType.LOGIN))
                .ReturnsAsync(true);
            
            // Act
            var result = await _authViewModel.Login(username, password);
            
            // Assert
            Assert.IsTrue(result);
            _mockLoginService.Verify(service => service.AuthenticationLogService(userId, ActionType.LOGIN), Times.Once);
        }

        [TestMethod]
        public async Task Logout_Success_LogsLogoutAction()
        {
            // Arrange
            const int userId = 1;
            
            // First login to set the user context
            var userModel = new UserAuthModel(userId, "testuser", "password", "test@example.com", "Patient");
            
            _mockLoginService.Setup(service => service.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(userModel);
            
            _mockLoginService.Setup(service => service.AuthenticationLogService(userId, ActionType.LOGIN))
                .ReturnsAsync(true);
            
            await _authViewModel.Login("testuser", "password");
            
            _mockLoginService.Setup(service => service.AuthenticationLogService(userId, ActionType.LOGOUT))
                .ReturnsAsync(true);
            
            // Act
            var result = await _authViewModel.Logout();
            
            // Assert
            Assert.IsTrue(result);
            _mockLoginService.Verify(service => service.AuthenticationLogService(userId, ActionType.LOGOUT), Times.Once);
        }

        [TestMethod]
        public async Task CreateAccount_Success_LogsCreateAccountAction()
        {
            // Arrange
            const int userId = 1;

            // Create a valid CNP following Romanian format rules:
            // - First digit: 1 (male born 1900-1999) or 2 (female born 1900-1999)
            // - Next 2 digits: year of birth (last 2 digits)
            // - Next 2 digits: month of birth (01-12)
            // - Next 2 digits: day of birth (01-31)
            // - Next 2 digits: county code (01-52)
            // - Next 3 digits: sequence number (001-999)
            // - Last digit: checksum
            string validCnp = "1900101123456"; // CNP for a male born on 1990-01-01

            // Make sure date and CNP match according to the validation rules
            // Year digits in CNP: 90 (position 1-2) must match 1990
            // Month digits in CNP: 01 (position 3-4) must match January
            // Day digits in CNP: 01 (position 5-6) must match day 1
            var birthDate = new DateOnly(1990, 1, 1);

            var model = new UserCreateAccountModel(
                "newuser", "password", "new@example.com", "New User",
                birthDate, validCnp, BloodType.A_Positive,
                "1234567890", 70, 175); // Emergency contact is correctly set to length 10

            _mockLoginService.Setup(service => service.CreateAccount(model))
                .ReturnsAsync(true);

            // Simulate that the user has been created with ID 1
            _mockLoginService.Setup(service => service.GetUserByUsername("newuser"))
                .ReturnsAsync(new UserAuthModel(userId, "newuser", "password", "new@example.com", "Patient"));

            _mockLoginService.Setup(service => service.AuthenticationLogService(userId, ActionType.CREATE_ACCOUNT))
                .ReturnsAsync(true);

            _mockLoginService.Setup(service => service.AuthenticationLogService(userId, ActionType.LOGIN))
                .ReturnsAsync(true);

            // Act
            var result = await _authViewModel.CreateAccount(model);

            // Assert
            Assert.IsTrue(result);
            _mockLoginService.Verify(service => service.AuthenticationLogService(userId, ActionType.CREATE_ACCOUNT), Times.Once);
        }
    }
}
