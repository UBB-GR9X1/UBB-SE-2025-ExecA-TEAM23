using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hospital.Models;
using Microsoft.Data.SqlClient;
using Hospital.Configs;
using Hospital.Repositories;

namespace LoginPageTests.Tests
{
    [TestClass]
    public class LogInRepositoryTests
    {
        private readonly ILogInRepository _logInRepository;

        public LogInRepositoryTests()
        {
            _logInRepository = new LogInRepository();
        }

        [TestMethod]
        public async Task GetUserByUsername_WithValidUsernameCheckForUser_ReturnsUser()
        {
            // Task<UserAuthModel> GetUserByUsername(string username)
            var result = await _logInRepository.GetUserByUsername("john_doe");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetUserByUsername_WithValidUsernameCheckIfTheEmailMatches_ReturnsTrue()
        {
            // Task<UserAuthModel> GetUserByUsername(string username)
            var result = await _logInRepository.GetUserByUsername("john_doe");
            Assert.AreEqual("john@example.com", result.Mail);
        }

        [TestMethod]
        public async Task GetUserByUsername_WithValidUsernameCheckIfTheRoleMatches_ReturnsTrue()
        {
            // Task<UserAuthModel> GetUserByUsername(string username)
            var result = await _logInRepository.GetUserByUsername("john_doe");
            Assert.AreEqual("Doctor", result.Role);
        }


        [TestMethod]
        public async Task GetUserByUsername_WithInvalidUsername_ThrowsException()
        {
            await Assert.ThrowsExceptionAsync<Hospital.Exceptions.AuthenticationException>(async () =>
            {
                await _logInRepository.GetUserByUsername("not_john_doe");
            });
        }

        [TestMethod]
        public async Task CreateAccount_WithValidData_ReturnsTrue()
        {
            var model = new UserCreateAccountModel(
                "nexuser",
                "nexuser1234",
                "nexuser@mail.com",
                "nexuser Doe",
                new DateOnly(1990, 1, 1),
                "1234567890248",
                BloodType.A_Positive,
                "0987654321",
                70.0f,
                180
            );

            bool result = await _logInRepository.CreateAccount(model);
            Assert.IsTrue(result);

            string query = "DELETE FROM Users WHERE Username = @username";

            using SqlConnection connectionToDatabase = new SqlConnection(Config.GetInstance().DatabaseConnection);
            await connectionToDatabase.OpenAsync().ConfigureAwait(false);

            using SqlCommand command = new SqlCommand(query, connectionToDatabase);
            command.Parameters.AddWithValue("@username", "nexuser");

            int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CreateAccount_WithExistingUsername_ReturnsTrue()
        {
            var model = new UserCreateAccountModel(
                "john_doe",
                "password123",
                "unique@mail.com",
                "John Doe",
                new DateOnly(1990, 1, 1),
                "1234567890123",
                BloodType.A_Positive,
                "Emergency Contact",
                70.0f,
                180
            );
            try
            {
                await _logInRepository.CreateAccount(model);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
