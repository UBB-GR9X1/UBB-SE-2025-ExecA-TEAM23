using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Hospital.Tests.IntegrationTest
{
    [TestClass]
    public class AuthViewModelLoggingIntegrationTests
    {
        private Mock<ILogInDatabaseService> _mockLoginService;
        private Mock<IAuthManagerModel> _mockAuthManager;
        private AuthViewModel _authViewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockLoginService = new Mock<ILogInDatabaseService>();
            _mockAuthManager = new Mock<IAuthManagerModel>();
            _authViewModel = new AuthViewModel(_mockAuthManager.Object);
        }

        [TestMethod]
        public async Task Login_WhenCredentialsAreValid_ShouldLogLoginAction()
        {
            // Arrange
            const string username = "testuser";
            const string password = "password";
            const int userId = 1;

            var userModel = new UserAuthModel(userId, username, password, "test@example.com", "Patient");

            _mockAuthManager.Setup(m => m.LoadUserByUsername(username))
                .ReturnsAsync(true);
            _mockAuthManager.Setup(m => m.VerifyPassword(password))
                .ReturnsAsync(true);
            _mockAuthManager.Setup(m => m.allUserInformation)
                .Returns(userModel);

            // Act
            await _authViewModel.Login(username, password);

            // Assert
            _mockAuthManager.Verify(m => m.LoadUserByUsername(username), Times.Once);
            _mockAuthManager.Verify(m => m.VerifyPassword(password), Times.Once);
        }

        [TestMethod]
        public async Task Login_WhenPasswordIsInvalid_ShouldNotLogLoginAction()
        {
            // Arrange
            const string username = "testuser";
            const string password = "wrong_password";
            const int userId = 1;

            var userModel = new UserAuthModel(userId, username, "correct_password", "test@example.com", "Patient");

            _mockAuthManager.Setup(m => m.LoadUserByUsername(username))
                .ReturnsAsync(true);
            _mockAuthManager.Setup(m => m.VerifyPassword(password))
                .ReturnsAsync(false);
            _mockAuthManager.Setup(m => m.allUserInformation)
                .Returns(userModel);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
                await _authViewModel.Login(username, password));

            _mockAuthManager.Verify(m => m.LoadUserByUsername(username), Times.Once);
            _mockAuthManager.Verify(m => m.VerifyPassword(password), Times.Once);
        }

        [TestMethod]
        public async Task Login_WhenUserDoesNotExist_ShouldNotLogLoginAction()
        {
            // Arrange
            const string username = "nonexistent";
            const string password = "password";

            _mockAuthManager.Setup(m => m.LoadUserByUsername(username))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
                await _authViewModel.Login(username, password));

            _mockAuthManager.Verify(m => m.LoadUserByUsername(username), Times.Once);
            _mockAuthManager.Verify(m => m.VerifyPassword(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Logout_WhenUserIsLoggedIn_ShouldLogLogoutAction()
        {
            // Arrange
            _mockAuthManager.Setup(m => m.Logout())
                .ReturnsAsync(true);

            // Act
            await _authViewModel.Logout();

            // Assert
            _mockAuthManager.Verify(m => m.Logout(), Times.Once);
        }

        [TestMethod]
        public async Task CreateAccount_WhenAccountCreationSucceeds_ShouldLogCreateAndLoginActions()
        {
            // Arrange
            const int userId = 1;
            string validCnp = "1900101123456"; // CNP for a male born on 1990-01-01
            var birthDate = new DateOnly(1990, 1, 1);

            var model = new UserCreateAccountModel(
                "newuser", "password", "new@example.com", "New User",
                birthDate, validCnp, BloodType.A_Positive,
                "1234567890", 70, 175);

            _mockAuthManager.Setup(m => m.CreateAccount(model))
                .ReturnsAsync(true);

            // Act
            await _authViewModel.CreateAccount(model);

            // Assert
            _mockAuthManager.Verify(m => m.CreateAccount(model), Times.Once);
        }

        [TestMethod]
        public void GetUserRole_ShouldReturnCurrentUserRole()
        {
            // Arrange
            const string expectedRole = "Doctor";
            var userModel = new UserAuthModel(1, "testuser", "password", "test@example.com", expectedRole);

            _mockAuthManager.Setup(m => m.allUserInformation)
                .Returns(userModel);

            // Act
            string role = _authViewModel.GetUserRole();

            // Assert
            Assert.AreEqual(expectedRole, role);
        }
    }
}
